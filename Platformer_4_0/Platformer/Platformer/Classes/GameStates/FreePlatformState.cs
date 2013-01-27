using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Platformer
{

    class FreePlatformState : GameState
    {
        readonly static float RUN_SPEED = Util.scale(30);
        readonly static float DIRECTIONAL_INFLUENCE = Util.scale(10);
        readonly static float SPEED_INFLUENCE = 0.008f;
        readonly static float SPEED_ON_HIT = 0.9f;

        readonly static float LEFT_LIMIT = Util.scale(50);
        readonly static float RIGHT_LIMIT = Util.scale(900);
        readonly static float CENTER = 75;

        readonly static float RIGHT_HITZONE = Util.scale(200);
        readonly static float LEFT_HITZONE = Util.scale(300);
        readonly static float PUNCHZONE = RIGHT_HITZONE + Util.scale(170);

        readonly static float GROUND_HEIGHT = Util.offsetY(Util.scale(350));
        readonly static float JUMP_HEIGHT = Util.offsetY(Util.scale(150));

        readonly static float OBSTACLE_CUT_OFF = -10.0f;
        readonly static float OBSTACLE_DEADZONE = OBSTACLE_CUT_OFF - 100.0f;
        readonly static int   OBSTACLE_MAX_ON_SCREEN = 2;
        readonly static int   OBSTACLE_MIN_TILL_ER = 4;
        readonly static int   OBSTACLE_MAX_TILL_ER = 6;

        readonly static float JUMP_DURATION = 40.0f;
        readonly static float JUMP_MID_DURATION = JUMP_DURATION / 2;
        readonly static float JUMP_BUFFER = 10.0f;
        readonly static float PUNCH_DURATION = 10.0f;
        readonly static float PUNCH_BUFFER = 5.0f;

        readonly static float KNOCKBACK_DISTANCE = Util.scale(200);
        readonly static float KNOCKBACK_DURATION = 20.0f;
        readonly static float KNOCKBACK_BUFFER = -60.0f;

        readonly static float QUIT_DURATION = 100.0f;

        readonly static float VOICE_HIT_CHANCE = 1.0f;
        readonly static float VOICE_PUNCH_CHANCE = 0.45f;
        readonly static float VOICE_RECOVER_CHANCE = 0.6f;
        readonly static float VOICE_FAILURE_CHANCE = 1.0f;
        readonly static float VOICE_SUCCESS_CHANCE = 1.0f;
        readonly static float VOICE_RANDOM_CHANCE = 0.0008f;
        readonly static float VOICE_OBSTACLE_RANDOM_CHANCE = 0.002f;

        static Texture2D hitTexture;
        static Texture2D jumpTexture;
        static Texture2D jumpAnimTexture;
        static Texture2D punchTexture;
        static Texture2D runTexture;
        static Texture2D runAnimTexture;
        static Texture2D slideTexture;
        static Texture2D standTexture;
        static Texture2D currentSprite;
        static Texture2D uibg;
        static Texture2D monitorBlip;
        static Texture2D obstacleGourneyTexture;
        static Texture2D obstaclePatientTexture;
        static Texture2D obstaclePatientDestroyedTexture;
        static Texture2D obstacleWiresTexture;

        static Random random = new Random();

        Vector2 playerPosition;
        Vector2 backgroundPosition;

        float jumpTimer = -50;
        float punchTimer = -50;
        float knockbackTimer = -50;
        float quitTimer = QUIT_DURATION;

        int obstaclesUntilER;
        bool stopAddingRooms = false;

        float screenAdjustment;

        Boolean failure = false;

        int currentFrame = 0;

        Rectangle[] animBoxes = {new Rectangle(0, 0, 300, 300),
                               new Rectangle(300, 0, 300, 300),
                               new Rectangle(600, 0, 300, 300),
                               new Rectangle(900, 0, 300, 300),
                               new Rectangle(0, 300, 300, 300),
                               new Rectangle(300, 300, 300, 300),
                               new Rectangle(600, 300, 300, 300),
                               new Rectangle(900, 300, 300, 300)};

        Heart heart = new Heart();

        //Background Panels
        static Texture2D[] backgroundTextures;
        static Texture2D firstBackgroundTexture;
        static Texture2D lastBackgroundTexture;
        static float backgroundWidth;
        Queue<Texture2D> backgrounds = new Queue<Texture2D>();

        //Controls
        static Keys[] jumpKeys  = new Keys[] { Keys.W, Keys.Space };
        static Keys[] leftKeys  = new Keys[] { };
        static Keys[] rightKeys = new Keys[] { };
        static Keys[] slideKeys = new Keys[] { Keys.S };
        static Keys[] punchKeys = new Keys[] { Keys.D };

        //Sound Effects
        static SoundEffectInstance currentSound;
        static Sound[] voiceHit;
        static Sound[] voicePunch;
        static Sound[] voiceRecover;
        static Sound[] voiceFailure;
        static Sound[] voiceSuccess;
        static Sound[] voiceRandom;

        struct Sound
        {
            public SoundEffect effect;
            public float chance;

            public Sound(ContentManager manager, string path, float chance)
            {
                this.effect = manager.Load<SoundEffect>(path);
                this.chance = chance;
            }
        }

        //Obstacles
        static Obstacle currentObstacle;

        struct Obstacle 
        {
            public enum Counter { PUNCH, SLIDE, JUMP }

            public Counter[] counters;
            public Texture2D sprite;
            public Texture2D destroyedSprite;
            public Vector2 position;

            public Obstacle(Vector2 position)
            {
                this.counters = null;
                this.sprite = null;
                this.destroyedSprite = null;
                this.position = Vector2.Zero;
                Randomize();
                if (position != Vector2.Zero)
                    this.position = position;
            }

            public void Randomize()
            {
                position.X = random.Next((int)(PlatformerGame.SCREEN_WIDTH), (int)(PlatformerGame.SCREEN_WIDTH*1.4));

                switch (random.Next(0, 3)) {
                    case 0:
                        sprite = obstacleGourneyTexture;
                        position.Y = GROUND_HEIGHT + Util.offsetY(Util.scale(-300));
                        counters = new Counter[] { Counter.JUMP };
                        break;
                    case 1:
                        sprite = obstacleWiresTexture;
                        position.Y = Util.offsetY(Util.scale(-120));
                        counters = new Counter[] { Counter.SLIDE };
                        break;
                    case 2:
                        sprite = obstaclePatientTexture;
                        destroyedSprite = obstaclePatientDestroyedTexture;
                        position.Y = GROUND_HEIGHT + Util.offsetY(Util.scale(-350));
                        counters = new Counter[] { Counter.PUNCH };
                        break;
                }
            }

            public bool hasCounter(Counter counter)
            {
                return counters.Contains<Counter>(counter);
            }
        }

        //Intro
        static Sound voiceDrWontNeedThis;
        static Sound voiceDrNotASprinter;
        static Sound voiceDrDozerToTheRescue;
        static Sound voiceDrThatsRidiculous;
        static Sound voiceNurseTimeOfDeath;
        static Sound voiceNurseHeartToER01;
        static Sound voiceNurseHeartToER02;
        static Sound voiceNurseHospitalOverflowing;
        static Sound voiceNurseHeWillDie;
        static Sound voiceNurseLetsDoThis01;
        static Sound voiceNurseLetsDoThis02;
        static Sound voiceNurseLetsGo;
        static Sound voiceNurseSqueal;

        /*struct Conversation
        {
            enum Stage { WAIT_1, DOCTOR, WAIT_2, NURSE, WAIT_3, RIP, RUN }

            Sound[] voiceDoctor;
            Sound[] voiceNurse;
            SoundEffectInstance sound;
            float chance;
            float timer;

            public Conversation(Sound[] voiceDoctor, Sound[] voiceNurse, float chance)
            {
                this.voiceDoctor = voiceDoctor;
                this.voiceNurse = voiceNurse;
                this.chance = chance;
                this.timer = 30.0f;
            }

            public void Update()
            {
                if ( timer <= 0 )
                {
                    if (sound.State == SoundState.Stopped)
                    {

                    }
                }
            }
        }*/


        public FreePlatformState()
        {
            obstaclesUntilER = random.Next(OBSTACLE_MIN_TILL_ER, OBSTACLE_MAX_TILL_ER);
            playerPosition = new Vector2(CENTER, GROUND_HEIGHT);
            backgroundPosition = new Vector2(0, Util.offsetY(0));
            currentObstacle = new Obstacle(Vector2.Zero);

            backgrounds.Enqueue(firstBackgroundTexture);
            float coverage = Util.scale(backgroundWidth);

            for (int i = 0; coverage < PlatformerGame.SCREEN_WIDTH * 3; i++)
            {
                backgrounds.Enqueue(backgroundTextures[random.Next(0, backgroundTextures.Length)]);
                coverage += Util.scale(backgroundWidth);
            }
        }

        public static void LoadContent(ContentManager manager)
        {
            hitTexture = manager.Load<Texture2D>("Sprites/Player/Hit");
            jumpTexture = manager.Load<Texture2D>("Sprites/Player/Jumping");
            punchTexture = manager.Load<Texture2D>("Sprites/Player/Punching");
            runTexture = manager.Load<Texture2D>("Sprites/Player/Running");
            slideTexture = manager.Load<Texture2D>("Sprites/Player/Sliding");
            standTexture = manager.Load<Texture2D>("Sprites/Player/Standing");
            obstacleGourneyTexture = manager.Load<Texture2D>("Sprites/GourneyJump");
            obstaclePatientTexture = manager.Load<Texture2D>("Sprites/PatientPunch");
            obstaclePatientDestroyedTexture = manager.Load<Texture2D>("Sprites/PatientPunchDestroyed");
            obstacleWiresTexture = manager.Load<Texture2D>("Sprites/WiresDuck");

            runAnimTexture = manager.Load<Texture2D>("Sprites/Player/RunningAnim");
            jumpAnimTexture = manager.Load<Texture2D>("Sprites/Player/JumpingAnim");

            backgroundTextures = new Texture2D[] {
                manager.Load<Texture2D>("Backgrounds/background1"),
                manager.Load<Texture2D>("Backgrounds/background2"),
                manager.Load<Texture2D>("Backgrounds/background3"),
                manager.Load<Texture2D>("Backgrounds/background4"),
                manager.Load<Texture2D>("Backgrounds/background5"),
            };
            firstBackgroundTexture = manager.Load<Texture2D>("Backgrounds/Splash");
            lastBackgroundTexture = manager.Load<Texture2D>("Backgrounds/Splash");

            uibg = manager.Load<Texture2D>("Sprites/UIBG");
            monitorBlip = manager.Load<Texture2D>("Sprites/MonitorBlip");

            backgroundWidth = backgroundTextures[0].Width;

            voiceHit = new Sound[] {
                new Sound(manager, "Voices/dr_oomph_01", 1.0f),
                new Sound(manager, "Voices/dr_oomph_02", 1.0f),
                new Sound(manager, "Voices/dr_oomph_03", 1.0f),
                new Sound(manager, "Voices/dr_oomph_04", 1.0f),
            };

            voicePunch = new Sound[] {
                new Sound(manager, "Voices/dr_bulldozed_01", 0.2f),
                new Sound(manager, "Voices/dr_bulldozed_02", 0.2f),
                new Sound(manager, "Voices/dr_bulldozed_03", 0.3f),
                new Sound(manager, "Voices/dr_kapow_01", 0.5f),
                new Sound(manager, "Voices/dr_kapow_02", 0.4f),
                new Sound(manager, "Voices/dr_outta_the_way_01", 0.5f),
                new Sound(manager, "Voices/dr_outta_the_way_02", 0.5f),
            };

            voiceRecover = new Sound[] {
                new Sound(manager, "Voices/dr_check_infection", 1.0f),
                new Sound(manager, "Voices/dr_crash_cart", 1.0f),
                new Sound(manager, "Voices/dr_hes_crashing", 1.0f),
                new Sound(manager, "Voices/dr_cant_spell", 1.0f),
            };

            voiceFailure = new Sound[] {
                new Sound(manager, "Voices/dr_asystole", 1.0f),
                new Sound(manager, "Voices/dr_its_no_use", 1.0f),
            };

            voiceSuccess = new Sound[] {
                new Sound(manager, "Voices/dr_made_it_01", 0.5f),
                new Sound(manager, "Voices/dr_made_it_02", 0.5f),
                new Sound(manager, "Voices/dr_success_01", 0.5f),
                new Sound(manager, "Voices/dr_success_02", 0.5f),
                new Sound(manager, "Voices/dr_now_for_fun_01", 0.3f),
                new Sound(manager, "Voices/dr_now_for_fun_02", 0.3f),
                new Sound(manager, "Voices/dr_now_for_fun_03", 0.3f),
            };

            voiceRandom = new Sound[] {
                new Sound(manager, "Voices/dr_gangway_01", 0.4f),
                new Sound(manager, "Voices/dr_gangway_02", 0.4f),
                new Sound(manager, "Voices/dr_outta_the_way_01", 0.3f),
                new Sound(manager, "Voices/dr_outta_the_way_02", 0.3f),
                new Sound(manager, "Voices/dr_surgeon_thru_01", 1.0f),
            };
        }

        void GameState.Update(PlatformerGame game, GameTime gameTime)
        {
            if (game.keyboard.IsKeyDown(Keys.Escape))
            {
                game.currentState = new MenuState();
            }

            if (!failure)
            {
                if (backgrounds.Count <= 3)
                {
                    PlaySound(voiceSuccess, 1.0f, true);
                    Util.GotoRandomMinigame(game);
                }

                currentSprite = runAnimTexture;
                playerPosition.Y = GROUND_HEIGHT;
                screenAdjustment = RUN_SPEED - ((CENTER - playerPosition.X) * SPEED_INFLUENCE);

                if (knockbackTimer < 0)
                {
                    HandleMovement(game.keyboard);
                    HandleSlide(game.keyboard);
                    HandlePunch(game.keyboard, game.prevKeyboard);
                    HandleJump(game.keyboard, game.prevKeyboard);
                    HandleObstacles();

                    if (knockbackTimer <= KNOCKBACK_BUFFER || knockbackTimer == KNOCKBACK_DURATION)
                    {
                        MoveScreen(screenAdjustment);
                        PlaySound(voiceRandom, VOICE_RANDOM_CHANCE, false);
                    }
                    else
                    {
                        currentSprite = hitTexture;
                        knockbackTimer--;
                        if (knockbackTimer == KNOCKBACK_BUFFER + 1) PlaySound(voiceRecover, VOICE_RECOVER_CHANCE, true);
                    }
                }
                else
                {
                    currentSprite = hitTexture;
                    knockbackTimer--;
                    MoveScreen(-KNOCKBACK_DISTANCE / KNOCKBACK_DURATION);
                }

                heart.Update(gameTime);
                if (heart.heartMeter == 0) failure = true;
            }
            else
            {
                currentSprite = hitTexture;
                if (quitTimer == QUIT_DURATION) PlaySound(voiceFailure, VOICE_FAILURE_CHANCE, true);
                quitTimer--;
                if (quitTimer == 0) game.currentState = new MenuState();
            }
        }

        void MoveScreen(float screenAdjustment)
        {
            currentObstacle.position.X -= screenAdjustment;
            backgroundPosition.X -= screenAdjustment;

            if (backgroundPosition.X < Util.scale(-backgroundWidth) * 3)
            {
                backgroundPosition.X += Util.scale(backgroundWidth);
                backgrounds.Dequeue();
                if (!stopAddingRooms) backgrounds.Enqueue(backgroundTextures[random.Next(0, backgroundTextures.Length-1)]);
            }
        }

        void HandleMovement(KeyboardState keyboard)
        {
            if (playerPosition.X > LEFT_LIMIT && Util.IsAnyKeyDown(keyboard, leftKeys))
            {
                playerPosition.X -= DIRECTIONAL_INFLUENCE;
            }

            if (playerPosition.X < RIGHT_LIMIT && Util.IsAnyKeyDown(keyboard, rightKeys))
            {
                playerPosition.X += DIRECTIONAL_INFLUENCE;
            }
        }

        void HandlePunch(KeyboardState keyboard, KeyboardState prevKeyboard)
        {
            if (Util.IsAnyKeyPressed(keyboard, prevKeyboard, punchKeys) && punchTimer < -PUNCH_BUFFER)
            {
                punchTimer = PUNCH_DURATION;
                if (knockbackTimer > KNOCKBACK_BUFFER) knockbackTimer = KNOCKBACK_BUFFER+1;
            }
            else
            {
                punchTimer--;
            }

            if (punchTimer > 0)
            {
                currentSprite = punchTexture;
            }
        }

        void HandleJump(KeyboardState keyboard, KeyboardState prevKeyboard)
        {
            if (Util.IsAnyKeyPressed(keyboard, prevKeyboard, jumpKeys) && jumpTimer < -JUMP_BUFFER)
            {
                jumpTimer = JUMP_DURATION;
                if (knockbackTimer > KNOCKBACK_BUFFER) knockbackTimer = KNOCKBACK_BUFFER + 1;
            }
            else
            {
                jumpTimer -= (1 / RUN_SPEED) * screenAdjustment;
            }

            if (jumpTimer > 0)
            {
                if (jumpTimer == JUMP_DURATION) currentFrame = 0;
                currentSprite = jumpAnimTexture;

                float heightModifier;
                if (jumpTimer > JUMP_MID_DURATION)
                {
                    heightModifier = (jumpTimer - JUMP_MID_DURATION) / JUMP_MID_DURATION;
                }
                else
                {
                    heightModifier = (JUMP_MID_DURATION - jumpTimer) / JUMP_MID_DURATION;
                }
                playerPosition.Y = Util.offsetY((GROUND_HEIGHT - Util.OFFSET) * heightModifier);
            }
        }

        void HandleSlide(KeyboardState keyboard)
        {
            if (Util.IsAnyKeyDown(keyboard, slideKeys))
            {
                currentSprite = slideTexture;
                knockbackTimer = KNOCKBACK_BUFFER;
            }
        }

        void HandleObstacles()
        {
            if (currentObstacle.position.X < OBSTACLE_CUT_OFF-currentObstacle.sprite.Width)
            {
                if (backgrounds.Count >= 5)
                {
                    currentObstacle.Randomize();
                    obstaclesUntilER--;

                    if (obstaclesUntilER == 0)
                    {
                        backgrounds.Enqueue(lastBackgroundTexture);
                        stopAddingRooms = true;
                    }
                }
            }

            if (currentObstacle.position.X < playerPosition.X + RIGHT_HITZONE && currentObstacle.position.X > playerPosition.X - LEFT_HITZONE)
            {
                if (currentObstacle.counters.Length != 0
                    && (currentSprite != jumpTexture  || !currentObstacle.hasCounter(Obstacle.Counter.JUMP))
                    && (currentSprite != slideTexture || !currentObstacle.hasCounter(Obstacle.Counter.SLIDE))
                    && (currentSprite != punchTexture || !currentObstacle.hasCounter(Obstacle.Counter.PUNCH)))
                {
                    currentSprite = hitTexture;
                    screenAdjustment = -KNOCKBACK_DISTANCE;
                    knockbackTimer = KNOCKBACK_DURATION;
                    punchTimer = -PUNCH_BUFFER;
                    jumpTimer = -JUMP_BUFFER;
                    PlaySound(voiceHit, VOICE_HIT_CHANCE, false);
                    heart.hit();

                    if (currentObstacle.hasCounter(Obstacle.Counter.PUNCH))
                    {
                        currentObstacle.counters = new Obstacle.Counter[0];
                        currentObstacle.sprite = currentObstacle.destroyedSprite;
                    }
                }
                else
                {
                    PlaySound(voiceRandom, VOICE_OBSTACLE_RANDOM_CHANCE, false);
                }
            }

            if (currentObstacle.position.X < playerPosition.X + PUNCHZONE && currentObstacle.position.X > playerPosition.X)
            {
                if (currentObstacle.hasCounter(Obstacle.Counter.PUNCH) && currentSprite == punchTexture)
                {
                    currentObstacle.counters = new Obstacle.Counter[0];
                    currentObstacle.sprite = currentObstacle.destroyedSprite;
                    PlaySound(voicePunch, VOICE_PUNCH_CHANCE, false);
                }
            }
        }

        void PlaySound(Sound[] soundBank, float chance, bool priority)
        {
            if (priority || currentSound == null || currentSound.State != SoundState.Playing)
            {
                if (chance >= random.NextDouble())
                {
                    if (currentSound != null)
                    {
                        currentSound.Stop();
                        currentSound = null;
                    }

                    Sound sound;
                    do
                    {
                        sound = soundBank[random.Next(0, soundBank.Length)];
                    } while (sound.chance < random.NextDouble());
                
                    currentSound = sound.effect.CreateInstance();
                    currentSound.Play();
                }
            }
        }

        void GameState.Draw(PlatformerGame game, SpriteBatch spriteBatch)
        {
            int i = 0;
            foreach (Texture2D background in backgrounds) 
            {
                spriteBatch.Draw(background, new Vector2(backgroundPosition.X + (Util.scale(backgroundWidth) * i), backgroundPosition.Y), null, Color.White, 0f, Vector2.Zero, Util.SCALE, SpriteEffects.None, 0f);
                i++;
            }

            spriteBatch.Draw(currentObstacle.sprite, currentObstacle.position, null, Color.White, 0f, Vector2.Zero, Util.SCALE, SpriteEffects.None, 0f);

            if (currentSprite != runAnimTexture && currentSprite != jumpAnimTexture)
                spriteBatch.Draw(currentSprite, playerPosition, null, Color.White, 0f, Vector2.Zero, Util.SCALE, SpriteEffects.None, 0f);
            else
            {
                Rectangle rect = new Rectangle(0, 0, 0, 0);
                if (currentSprite == runAnimTexture) rect = animBoxes[currentFrame/3];
                if (currentSprite == jumpAnimTexture) rect = animBoxes[(int)(currentFrame/(JUMP_DURATION/8))];
                spriteBatch.Draw(currentSprite, playerPosition, rect, Color.White, 0f, Vector2.Zero, Util.SCALE, SpriteEffects.None, 0f);
                currentFrame++;
                if (currentSprite == runAnimTexture) if (currentFrame == 8*3) currentFrame = 0;
                if (currentSprite == jumpAnimTexture) if (currentFrame == 6*(JUMP_DURATION/8)) currentFrame = 0;
            }

            Color uiColor;
            uiColor = Color.White;
            if (heart.heartMeter < 35) uiColor = Color.Yellow;
            if (heart.heartMeter < 20) uiColor = Color.Orange;
            if (heart.heartMeter < 10) uiColor = Color.Red;

            spriteBatch.Draw(uibg, Vector2.Zero, uiColor);

            spriteBatch.DrawString(game.font, heart.performance.ToString(), new Vector2(10, 150), uiColor, 0f, Vector2.Zero, 2f, SpriteEffects.None, 0f);

            heart.Draw(spriteBatch, 1080, 0);

            spriteBatch.DrawString(game.font, "Free Platform State", new Vector2(10, 10), Color.White);
        }
    }
}
