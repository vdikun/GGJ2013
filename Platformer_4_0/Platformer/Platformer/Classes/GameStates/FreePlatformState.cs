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
        readonly static float RUN_SPEED = Util.scale(25);
        readonly static float DIRECTIONAL_INFLUENCE = Util.scale(10);
        readonly static float SPEED_INFLUENCE = 0.008f;
        readonly static float SPEED_ON_HIT = 0.9f;

        readonly static float LEFT_LIMIT = Util.scale(50);
        readonly static float RIGHT_LIMIT = Util.scale(900);
        readonly static float CENTER = 75;

        readonly static float RIGHT_HITZONE = Util.scale(200);
        readonly static float LEFT_HITZONE = Util.scale(300);
        readonly static float PUNCHZONE = RIGHT_HITZONE + Util.scale(170);

        readonly static float GROUND_HEIGHT = Util.offsetY(Util.scale(300));
        readonly static float JUMP_HEIGHT = Util.offsetY(Util.scale(150));

        readonly static float OBSTACLE_CUT_OFF = -400.0f;
        readonly static float OBSTACLE_DEADZONE = OBSTACLE_CUT_OFF - 100.0f;

        readonly static float JUMP_DURATION = 40.0f;
        readonly static float JUMP_MID_DURATION = JUMP_DURATION / 2;
        readonly static float JUMP_BUFFER = 10.0f;
        readonly static float PUNCH_DURATION = 10.0f;
        readonly static float PUNCH_BUFFER = 5.0f;

        readonly static float KNOCKBACK_DISTANCE = Util.scale(200);
        readonly static float KNOCKBACK_DURATION = 20.0f;
        readonly static float KNOCKBACK_BUFFER = -60.0f;

        readonly static float VOICE_HIT_CHANCE = 1.0f;
        readonly static float VOICE_PUNCH_CHANCE = 0.45f;
        readonly static float VOICE_RECOVER_CHANCE = 0.6f;
        readonly static float VOICE_FAILURE_CHANCE = 1.0f;
        readonly static float VOICE_SUCCESS_CHANCE = 1.0f;
        readonly static float VOICE_RANDOM_CHANCE = 0.0008f;
        readonly static float VOICE_OBSTACLE_RANDOM_CHANCE = 0.002f;

        static Texture2D hitTexture;
        static Texture2D jumpTexture;
        static Texture2D punchTexture;
        static Texture2D runTexture;
        static Texture2D slideTexture;
        static Texture2D standTexture;
        static Texture2D currentSprite;
        static Texture2D jumpObstacleTexture;
        static Texture2D slideObstacleTexture;
        static Texture2D punchObstacleTexture;
        static Texture2D currentObstacle;
        static Texture2D uibg;
        static Texture2D monitorBlip;

        static Random random = new Random();

        Vector2 playerPosition;
        Vector2 backgroundPosition;
        Vector2 obstaclePosition;

        float jumpTimer = -50;
        float punchTimer = -50;
        float knockbackTimer = -50;

        float screenAdjustment;

        Heart heart = new Heart();

        //Background Panels
        static Texture2D[] backgroundTextures;
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

        public FreePlatformState()
        {
            playerPosition = new Vector2(CENTER, GROUND_HEIGHT);
            backgroundPosition = new Vector2(0, Util.offsetY(0));
            obstaclePosition = new Vector2(OBSTACLE_DEADZONE, Util.offsetY(0));

            float coverage = 0;
            for (int i = 0; coverage < PlatformerGame.SCREEN_WIDTH * 2.5; i++)
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
            jumpObstacleTexture = manager.Load<Texture2D>("Sprites/Player/JumpObstacle");
            slideObstacleTexture = manager.Load<Texture2D>("Sprites/Player/SlideObstacle");
            punchObstacleTexture = manager.Load<Texture2D>("Sprites/Player/PunchObstacle");

            backgroundTextures = new Texture2D[] {
                manager.Load<Texture2D>("Backgrounds/background1"),
                manager.Load<Texture2D>("Backgrounds/background2"),
                manager.Load<Texture2D>("Backgrounds/background3"),
                manager.Load<Texture2D>("Backgrounds/background4"),
                manager.Load<Texture2D>("Backgrounds/background5"),
            };

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
                new Sound(manager, "Voices/dr_bulldozed_01", 0.3f),
                new Sound(manager, "Voices/dr_bulldozed_02", 0.3f),
                new Sound(manager, "Voices/dr_bulldozed_03", 0.4f),
                new Sound(manager, "Voices/dr_kapow_01", 0.4f),
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
                new Sound(manager, "Voices/dr_gangway_01", 0.45f),
                new Sound(manager, "Voices/dr_gangway_02", 0.45f),
                new Sound(manager, "Voices/dr_outta_the_way_01", 0.3f),
                new Sound(manager, "Voices/dr_outta_the_way_02", 0.3f),
                new Sound(manager, "Voices/dr_surgeon_thru_01", 1.0f),
            };
        }

        void GameState.Update(PlatformerGame game, GameTime gameTime)
        {
            currentSprite = runTexture;
            playerPosition.Y = GROUND_HEIGHT;
            screenAdjustment = RUN_SPEED - ((CENTER - playerPosition.X) * SPEED_INFLUENCE);

            // Temporary Check
            if (game.keyboard.IsKeyDown(Keys.Escape))
            {
                game.currentState = new MenuState();
            }

            if (knockbackTimer < 0)
            {
                HandleMovement(game.keyboard);
                HandleSlide(game.keyboard);
                HandlePunch(game.keyboard, game.prevKeyboard);
                HandleJump(game.keyboard, game.prevKeyboard);
                HandleObstacles();

                if (knockbackTimer <= KNOCKBACK_BUFFER || knockbackTimer == KNOCKBACK_DURATION)
                {
                    obstaclePosition.X -= screenAdjustment;
                    backgroundPosition.X -= screenAdjustment;
                    PlaySound(voiceRandom, VOICE_RANDOM_CHANCE, false);
                    if (backgroundPosition.X < Util.scale(-backgroundWidth) * 2)
                    {
                        backgroundPosition.X += Util.scale(backgroundWidth);
                        backgrounds.Dequeue();
                        backgrounds.Enqueue(backgroundTextures[random.Next(0, backgroundTextures.Length)]);
                    }
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

                screenAdjustment = -KNOCKBACK_DISTANCE / KNOCKBACK_DURATION;
                obstaclePosition.X -= screenAdjustment;
                backgroundPosition.X -= screenAdjustment;
                if (backgroundPosition.X < Util.scale(-backgroundWidth) * 2) backgroundPosition.X += Util.scale(backgroundWidth);
            }

            heart.Update(gameTime);
            if (heart.heartMeter == 0) game.currentState = new MenuState();
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
                currentSprite = jumpTexture;

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
            if (obstaclePosition.X < OBSTACLE_CUT_OFF)
            {
                switch (random.Next(0, 3))
                {
                    case 0:
                        currentObstacle = jumpObstacleTexture;
                        obstaclePosition = new Vector2(1280, Util.offsetY(Util.scale(550)));
                        break;
                    case 1:
                        currentObstacle = slideObstacleTexture;
                        obstaclePosition = new Vector2(1280, Util.offsetY(Util.scale(-150)));
                        break;
                    case 2:
                        currentObstacle = punchObstacleTexture;
                        obstaclePosition = new Vector2(1280, Util.offsetY(Util.scale(50)));
                        break;
                }
            }

            if (obstaclePosition.X < playerPosition.X + RIGHT_HITZONE && obstaclePosition.X > playerPosition.X - LEFT_HITZONE)
            {
                if (currentObstacle == jumpObstacleTexture && currentSprite != jumpTexture
                    || currentObstacle == slideObstacleTexture && currentSprite != slideTexture
                    || currentObstacle == punchObstacleTexture && currentSprite != punchTexture)
                {
                    currentSprite = hitTexture;
                    screenAdjustment = -KNOCKBACK_DISTANCE;
                    knockbackTimer = KNOCKBACK_DURATION;
                    punchTimer = -PUNCH_BUFFER;
                    jumpTimer = -JUMP_BUFFER;
                    PlaySound(voiceHit, VOICE_HIT_CHANCE, false);
                    heart.hit();

                    if (currentObstacle == punchObstacleTexture)
                    {
                        obstaclePosition.X = OBSTACLE_DEADZONE;
                    }
                }
                else
                {
                    PlaySound(voiceRandom, VOICE_OBSTACLE_RANDOM_CHANCE, false);
                }
            }

            if (obstaclePosition.X < playerPosition.X + PUNCHZONE && obstaclePosition.X > playerPosition.X)
            {
                if (currentObstacle == punchObstacleTexture && currentSprite == punchTexture)
                {
                    obstaclePosition.X = OBSTACLE_DEADZONE;
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
                    Sound sound;
                    do
                    {
                        sound = soundBank[random.Next(0, voiceHit.Length)];
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
                i++;
                spriteBatch.Draw(background, new Vector2(backgroundPosition.X + (Util.scale(backgroundWidth) * i), backgroundPosition.Y), null, Color.White, 0f, Vector2.Zero, Util.SCALE, SpriteEffects.None, 0f);
            }

            spriteBatch.Draw(currentObstacle, obstaclePosition, null, Color.White, 0f, Vector2.Zero, Util.SCALE, SpriteEffects.None, 0f);
            spriteBatch.Draw(currentSprite, playerPosition, null, Color.White, 0f, Vector2.Zero, Util.SCALE, SpriteEffects.None, 0f);

            Color uiColor;
            uiColor = Color.White;
            if (heart.heartMeter < 35) uiColor = Color.Yellow;
            if (heart.heartMeter < 20) uiColor = Color.Orange;
            if (heart.heartMeter < 10) uiColor = Color.Red;

            spriteBatch.Draw(uibg, Vector2.Zero, uiColor);

            spriteBatch.DrawString(game.font, heart.performance.ToString(), new Vector2(10, 210), uiColor, 0f, Vector2.Zero, 2f, SpriteEffects.None, 0f);

            heart.Draw(spriteBatch, 1080, 0);

            spriteBatch.DrawString(game.font, "Free Platform State", new Vector2(10, 10), Color.White);
        }
    }
}
