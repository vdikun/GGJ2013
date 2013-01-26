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
        readonly static float CENTER = (RIGHT_LIMIT - LEFT_LIMIT) / 2 + (LEFT_LIMIT/3);

        readonly static float RIGHT_HITZONE = Util.scale(200);
        readonly static float LEFT_HITZONE = Util.scale(300);
        readonly static float PUNCHZONE = RIGHT_HITZONE + Util.scale(100);

        readonly static float GROUND_HEIGHT = Util.offsetY(Util.scale(300));
        readonly static float JUMP_HEIGHT = Util.offsetY(Util.scale(150));

        readonly static float OBSTACLE_CUT_OFF = -400.0f;
        readonly static float OBSTACLE_DEADZONE = OBSTACLE_CUT_OFF - 100.0f;

        readonly static float JUMP_DURATION = 40.0f;
        readonly static float JUMP_MID_DURATION = JUMP_DURATION/2;
        readonly static float JUMP_BUFFER = 10.0f;
        readonly static float PUNCH_DURATION = 10.0f;
        readonly static float PUNCH_BUFFER = 5.0f;

        static Texture2D hitTexture;
        static Texture2D jumpTexture;
        static Texture2D punchTexture;
        static Texture2D runTexture;
        static Texture2D slideTexture;
        static Texture2D standTexture;
        static Texture2D currentSprite;
        static Texture2D bgTexture;
        static Texture2D jumpObstacleTexture;
        static Texture2D slideObstacleTexture;
        static Texture2D punchObstacleTexture;
        static Texture2D currentObstacle;

        Vector2 playerPosition;
        Vector2 bgPosition;
        Vector2 obstaclePosition;

        float jumpTimer;
        float punchTimer;

        //Controls
        static Keys[] jumpKeys  = new Keys[] { Keys.W, Keys.Space };
        static Keys[] leftKeys  = new Keys[] { Keys.A };
        static Keys[] rightKeys = new Keys[] { Keys.D };
        static Keys[] slideKeys = new Keys[] { Keys.S };
        static Keys[] punchKeys = new Keys[] { Keys.E };

        public FreePlatformState()
        {
            playerPosition = new Vector2(CENTER, GROUND_HEIGHT);
            bgPosition = new Vector2(0, Util.offsetY(0));
            obstaclePosition = new Vector2(OBSTACLE_DEADZONE, Util.offsetY(0));
        }

        public static void LoadContent(ContentManager manager)
        {
            hitTexture = manager.Load<Texture2D>("Sprites/Player/Hit");
            jumpTexture = manager.Load<Texture2D>("Sprites/Player/Jumping");
            punchTexture = manager.Load<Texture2D>("Sprites/Player/Punching");
            runTexture = manager.Load<Texture2D>("Sprites/Player/Running");
            slideTexture = manager.Load<Texture2D>("Sprites/Player/Sliding");
            standTexture = manager.Load<Texture2D>("Sprites/Player/Standing");
            bgTexture = manager.Load<Texture2D>("Sprites/Player/Background");
            jumpObstacleTexture = manager.Load<Texture2D>("Sprites/Player/JumpObstacle");
            slideObstacleTexture = manager.Load<Texture2D>("Sprites/Player/SlideObstacle");
            punchObstacleTexture = manager.Load<Texture2D>("Sprites/Player/PunchObstacle");
        }

        void GameState.Update(PlatformerGame game, GameTime gameTime)
        {
            currentSprite = runTexture;
            playerPosition.Y = GROUND_HEIGHT;
            float screenAdjustment = RUN_SPEED;
            screenAdjustment = RUN_SPEED - ((CENTER - playerPosition.X) * SPEED_INFLUENCE);

            // Temporary Check
            if (game.keyboard.IsKeyDown(Keys.Escape))
            {
                game.currentState = new MenuState();
            }

            if (playerPosition.X > LEFT_LIMIT && Util.IsAnyKeyDown(game.keyboard, leftKeys))
            {
                playerPosition.X -= DIRECTIONAL_INFLUENCE;
            }

            if (playerPosition.X < RIGHT_LIMIT &&  Util.IsAnyKeyDown(game.keyboard, rightKeys))
            {
                playerPosition.X += DIRECTIONAL_INFLUENCE;
            }

            if (Util.IsAnyKeyDown(game.keyboard, slideKeys))
            {
                currentSprite = slideTexture;
            }

            if (Util.IsAnyKeyPressed(game.keyboard, game.prevKeyboard, punchKeys) && punchTimer < -PUNCH_BUFFER)
            {
                punchTimer = PUNCH_DURATION;
            }
            else
            {
                punchTimer--;
            }

            if (Util.IsAnyKeyPressed(game.keyboard, game.prevKeyboard, jumpKeys) && jumpTimer < -JUMP_BUFFER)
            {
                jumpTimer = JUMP_DURATION;
            }
            else
            {
                jumpTimer -= (1/RUN_SPEED) * screenAdjustment;
            }

            if (punchTimer > 0)
            {
                currentSprite = punchTexture;
            }

            if (jumpTimer > 0)
            {
                currentSprite = jumpTexture;

                float heightModifier;
                if (jumpTimer > JUMP_MID_DURATION)
                {
                    heightModifier = JUMP_MID_DURATION / (jumpTimer - JUMP_MID_DURATION);
                }
                else
                {
                    heightModifier = JUMP_MID_DURATION / (JUMP_MID_DURATION - jumpTimer);
                }
                Console.WriteLine(heightModifier);
                playerPosition.Y = JUMP_HEIGHT*heightModifier;
            }

            if (obstaclePosition.X < OBSTACLE_CUT_OFF)
            {
                switch (new Random().Next(0, 3))
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
                    || currentObstacle == punchObstacleTexture && currentSprite != punchTexture) {
                    currentSprite = hitTexture;
                    screenAdjustment = (int) (screenAdjustment * SPEED_ON_HIT);
                }
            }

            if (obstaclePosition.X < playerPosition.X + PUNCHZONE && obstaclePosition.X > playerPosition.X)
            {
                if (currentObstacle == punchObstacleTexture && currentSprite == punchTexture)
                {
                    obstaclePosition.X = OBSTACLE_DEADZONE;
                }
            }

            bgPosition.X -= screenAdjustment;
            if (bgPosition.X < Util.scale(-PlatformerGame.SCREEN_WIDTH)) bgPosition.X = 0;

            obstaclePosition.X -= screenAdjustment;
        }

        void GameState.Draw(PlatformerGame game, SpriteBatch spriteBatch)
        {
            float panelWidth = Util.scale(bgTexture.Width);
            float coverage = 0;
            for (int i = 0; coverage < PlatformerGame.SCREEN_WIDTH*1.5; i++)
            {
                spriteBatch.Draw(bgTexture, new Vector2(bgPosition.X + (panelWidth * i), bgPosition.Y), null, Color.White, 0f, Vector2.Zero, Util.SCALE, SpriteEffects.None, 0f);
                coverage += panelWidth;
            }
            //spriteBatch.Draw(bgTexture, bgPosition, null, Color.White, 0f, Vector2.Zero, Util.SCALE, SpriteEffects.None, 0f);
            //spriteBatch.Draw(bgTexture, new Vector2(bgPosition.X + Util.scale(bgTexture.Width), bgPosition.Y), null, Color.White, 0f, Vector2.Zero, Util.SCALE, SpriteEffects.None, 0f);
            spriteBatch.Draw(currentObstacle, obstaclePosition, null, Color.White, 0f, Vector2.Zero, Util.SCALE, SpriteEffects.None, 0f);
            spriteBatch.Draw(currentSprite, playerPosition, null, Color.White, 0f, Vector2.Zero, Util.SCALE, SpriteEffects.None, 0f);

            spriteBatch.DrawString(game.font, "Free Platform State", new Vector2(10, 10), Color.White);
        }
    }
}
