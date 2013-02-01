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

namespace Dozer
{
    class MiniGame2State : GameState
    {
        static Texture2D tapTexture;
        static Texture2D waterTexture;
        static Texture2D heartTexture;
        static Texture2D heartDirty1Texture;
        static Texture2D heartDirty2Texture;
        static Texture2D currentHeartTexture;

        Vector2 tapPosition = new Vector2(1000, 10);
        Vector2 waterPosition;
        Vector2 heartPosition = new Vector2(300, 480);
        int tapGoal = 500;

        int dirt = 100;

        bool hittingHeart = false;

        int currentFrame = 0;
        int countdown = 5;
        int frameCountdown = 1;

        int quitTimer = 40;

        public MiniGame2State()
        {
            waterPosition = new Vector2(tapPosition.X - 23, tapPosition.Y + 200);
            Main.music = Main.minigameMusic;
            if (Main.musicLoop != null) Main.musicLoop.Stop();
        }

        public static void LoadContent(ContentManager manager)
        {
            tapTexture = manager.Load<Texture2D>("Minigames/Tap");
            waterTexture = manager.Load<Texture2D>("Minigames/Water");
            heartTexture = manager.Load<Texture2D>("Minigames/heart");
            heartDirty1Texture = manager.Load<Texture2D>("Minigames/heartDirty1");
            heartDirty2Texture = manager.Load<Texture2D>("Minigames/heartDirty2");
            currentHeartTexture = heartDirty2Texture;
        }

        void GameState.Update(Main game, GameTime gameTime)
        {
            if (countdown != 0)
            {
                frameCountdown--;
                if (frameCountdown == 0)
                {
                    frameCountdown = 40;
                    countdown--;
                }
            }
            else
            {
                float posDiff = Mouse.GetState().X - heartPosition.X;
                heartPosition.X += posDiff * 0.1f;
                if (heartPosition.X < 0) heartPosition.X = 0;
                if (heartPosition.X > 1280 - heartTexture.Width) heartPosition.X = 1280 - heartTexture.Width;

                if (Math.Abs(tapPosition.X - tapGoal) >= 10)
                {
                    if (tapPosition.X < tapGoal) tapPosition.X += 10;
                    else if (tapPosition.X > tapGoal) tapPosition.X -= 10;
                    waterPosition = new Vector2(tapPosition.X - 23, tapPosition.Y + 200);
                }
                else
                {
                    Random random = new Random();
                    tapGoal = random.Next(50, 1000);
                }

                hittingHeart = false;
                if (heartPosition.X < waterPosition.X + waterTexture.Width / 4 && heartPosition.X > waterPosition.X + waterTexture.Width / 4 - heartTexture.Width)
                {
                    dirt--;
                    hittingHeart = true;
                }

                currentHeartTexture = heartDirty2Texture;
                if (dirt <= 75) currentHeartTexture = heartDirty1Texture;
                if (dirt <= 0)
                {
                    currentHeartTexture = heartTexture;
                    quitTimer--;
                }

                if (quitTimer == 0)
                {
                   Util.gamesWon++;
                    if (Util.gamesWon < Util.GAMES_TO_WIN)
                    {
                        Util.GotoRandomMinigame(game);
                    }
                    else
                    {
                        game.currentState = new CutsceneState();
                    }
                }
            }

            currentFrame++;
            if (currentFrame > 7) currentFrame = 0;
        }

        void GameState.Draw(Main game, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(currentHeartTexture, heartPosition, Color.White);
            if (!hittingHeart)
                spriteBatch.Draw(waterTexture, waterPosition, new Rectangle(currentFrame/4 * 45, 100, waterTexture.Width / 2, waterTexture.Height-100), Color.White, 0f, Vector2.Zero, 2f, SpriteEffects.None, 0f);
            else
                spriteBatch.Draw(waterTexture, waterPosition, new Rectangle(currentFrame / 4 * 45, 200, waterTexture.Width / 2, waterTexture.Height - 200), Color.White, 0f, Vector2.Zero, 2f, SpriteEffects.None, 0f);
            spriteBatch.Draw(tapTexture, tapPosition, null, Color.White, 0f, Vector2.Zero, 2f, SpriteEffects.None, 0f);

            if (countdown == 4)
            {
                spriteBatch.DrawString(game.font, "WASH", new Vector2(480, 100), Color.White, 0f, Vector2.Zero, 5f, SpriteEffects.None, 0f);
            }
            else if (countdown > 0)
            {
                spriteBatch.DrawString(game.font, "" + countdown, new Vector2(630, 100), Color.White, 0f, Vector2.Zero, 5f, SpriteEffects.None, 0f);
            }
        }
    }
}
