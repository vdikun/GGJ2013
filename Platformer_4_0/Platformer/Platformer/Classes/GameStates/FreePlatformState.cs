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
        readonly static int RUN_SPEED = 20;
        readonly static int DIRECTIONAL_INFLUENCE = 10;
        readonly static float SPEED_INFLUENCE = 0.01f;
        readonly static float SPEED_ON_HIT = 0.9f;

        readonly static int LEFT_LIMIT = 50;
        readonly static int RIGHT_LIMIT = 800;
        readonly static int CENTER = (RIGHT_LIMIT - LEFT_LIMIT)/2 + LEFT_LIMIT;

        readonly int RIGHT_HITZONE = 200;
        readonly int LEFT_HITZONE = 300;

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

        int jumpTimer;
        int punchTimer;

        //Controls
        static Keys[] jumpKeys  = new Keys[] { Keys.W, Keys.Space };
        static Keys[] leftKeys  = new Keys[] { Keys.A };
        static Keys[] rightKeys = new Keys[] { Keys.D };
        static Keys[] slideKeys = new Keys[] { Keys.S };
        static Keys[] punchKeys = new Keys[] { Keys.E };

        public FreePlatformState()
        {
            playerPosition = new Vector2(100, 300);
            bgPosition = new Vector2(0, 0);
            obstaclePosition = new Vector2(-500, 0);
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
            playerPosition.Y = 300;
            int screenAdjustment = RUN_SPEED;
            screenAdjustment = RUN_SPEED - (int) ((CENTER - playerPosition.X) * SPEED_INFLUENCE);

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

            if (Util.IsAnyKeyPressed(game.keyboard, game.prevKeyboard, punchKeys) && punchTimer < -5)
            {
                punchTimer = 10;
            }

            if (Util.IsAnyKeyPressed(game.keyboard, game.prevKeyboard, jumpKeys) && jumpTimer < -10)
            {
                jumpTimer = 40;
            }

            if (punchTimer > 0)
            {
                currentSprite = punchTexture;
            }

            if (jumpTimer > 0)
            {
                currentSprite = jumpTexture;
                playerPosition.Y = 150;
            }

            jumpTimer--;
            punchTimer--;

            if (obstaclePosition.X < -400)
            {
                Random random = new Random();
                int randomNumber = random.Next(0, 3);
                if (randomNumber == 0)
                {
                    currentObstacle = jumpObstacleTexture;
                    obstaclePosition = new Vector2(1280, 550);
                }
                if (randomNumber == 1)
                {
                    currentObstacle = slideObstacleTexture;
                    obstaclePosition = new Vector2(1280, -150);
                }
                if (randomNumber == 2)
                {
                    currentObstacle = punchObstacleTexture;
                    obstaclePosition = new Vector2(1280, 50);
                }
            }

            if (obstaclePosition.X < playerPosition.X + RIGHT_HITZONE && obstaclePosition.X > playerPosition.X - LEFT_HITZONE)
            {
                if (currentObstacle == jumpObstacleTexture && currentSprite != jumpTexture
                    || currentObstacle == slideObstacleTexture && currentSprite != slideTexture
                    || currentObstacle == punchObstacleTexture && currentSprite != punchTexture) {
                    currentSprite = hitTexture;
                    //float f = screenAdjustment * SPEED_ON_HIT;
                    screenAdjustment = (int) (screenAdjustment * SPEED_ON_HIT);
                }
            }

            if (obstaclePosition.X < 500)
            {
                if (currentObstacle == punchObstacleTexture && currentSprite == punchTexture)
                {
                    obstaclePosition.X = -500;
                }
            }

            bgPosition.X -= screenAdjustment;
            if (bgPosition.X <= -1280) bgPosition.X = 0;

            obstaclePosition.X -= screenAdjustment;
        }

        void GameState.Draw(PlatformerGame game, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(bgTexture, bgPosition, Color.White);
            spriteBatch.Draw(bgTexture, new Vector2(bgPosition.X + bgTexture.Width, 0), Color.White);
            spriteBatch.Draw(currentObstacle, obstaclePosition, Color.White);
            spriteBatch.Draw(currentSprite, playerPosition, Color.White);

            spriteBatch.DrawString(game.font, "Free Platform State", new Vector2(10, 10), Color.White);
        }
    }
}
