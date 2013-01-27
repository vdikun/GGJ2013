﻿using System;
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
    class MiniGame2State : GameState
    {
        static Texture2D tapTexture;
        static Texture2D waterTexture;
        static Texture2D heartTexture;
        static Texture2D heartDirty1Texture;
        static Texture2D heartDirty2Texture;
        static Texture2D currentHeartTexture;

        Vector2 tapPosition = new Vector2(50, 10);
        Vector2 waterPosition;
        Vector2 heartPosition = new Vector2(0, 480);
        int tapGoal = 0;

        int currentFrame = 0;

        int countdown = 21;
        int frameCountdown = 1;
        int quitTimer = 40;

        public static void LoadContent(ContentManager manager)
        {
            tapTexture = manager.Load<Texture2D>("Minigames/Tap");
            waterTexture = manager.Load<Texture2D>("Minigames/Water");
            heartTexture = manager.Load<Texture2D>("Minigames/heart");
            heartDirty1Texture = manager.Load<Texture2D>("Minigames/heartDirty1");
            heartDirty2Texture = manager.Load<Texture2D>("Minigames/heartDirty2");
            currentHeartTexture = heartDirty2Texture;
        }

        void GameState.Update(PlatformerGame game, GameTime gameTime)
        {
            if (game.keyboard.IsKeyDown(Keys.Escape))
            {
                game.currentState = new MenuState();
            }

            float posDiff = Mouse.GetState().X - heartPosition.X;
            heartPosition.X += posDiff * 0.1f;
            if (heartPosition.X < 0) heartPosition.X = 0;
            if (heartPosition.X > 1280 - heartTexture.Width) heartPosition.X = 1280 - heartTexture.Width;
            
            if (countdown != 0)
            {
                frameCountdown--;
                if (frameCountdown == 0)
                {
                    frameCountdown = 80;
                    countdown--;
                    Random random = new Random();
                    tapGoal = random.Next(50, 1000);
                }
                tapPosition.X += (tapGoal - tapPosition.X) * 0.1f;
                waterPosition = new Vector2(tapPosition.X-23, tapPosition.Y + 200);
            }

            currentFrame++;
            if (currentFrame > 7) currentFrame = 0;

        }

        void GameState.Draw(PlatformerGame game, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(waterTexture, waterPosition, new Rectangle(currentFrame/4 * 45, 100, waterTexture.Width / 2, waterTexture.Height-100), Color.White, 0f, Vector2.Zero, 2f, SpriteEffects.None, 0f);
            spriteBatch.Draw(tapTexture, tapPosition, null, Color.White, 0f, Vector2.Zero, 2f, SpriteEffects.None, 0f);
            spriteBatch.Draw(currentHeartTexture, heartPosition, Color.White);

            spriteBatch.DrawString(game.font, "Mini Game 2 State", new Vector2(10, 10), Color.White);
        }
    }
}
