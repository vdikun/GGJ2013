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
    class MiniGame1State : GameState
    {
        static SoundEffect music;

        static Texture2D heartTexture;
        static Texture2D syringeTexture;

        Vector2 heartPosition = new Vector2(600, 480);
        Vector2 syringePosition = new Vector2(350, 0);
        Rectangle syringeRect;
        Rectangle heartRect;
        int syringeGoal = 0;

        int countdown = 5;
        int stab_countdown = 6;
        int frameCountdown = 1;
        int quitTimer = 40;

        bool hit = false;
        bool miss = false;

        public MiniGame1State()
        {
            PlatformerGame.minigameMusic = music;
            if (PlatformerGame.musicLoop != null) PlatformerGame.musicLoop.Stop();
        }

        public static void LoadContent(ContentManager manager)
        {
            heartTexture = manager.Load<Texture2D>("Minigames/heart");
            syringeTexture = manager.Load<Texture2D>("Minigames/syringe");
        }

        void GameState.Update(PlatformerGame game, GameTime gameTime)
        {
            if (countdown <= 1)
            {
                if (!hit)
                {
                    float posDiff = Mouse.GetState().X - heartPosition.X;
                    heartPosition.X += posDiff * 0.1f;
                    if (heartPosition.X < 0) heartPosition.X = 0;
                    if (heartPosition.X > 1280 - heartTexture.Width) heartPosition.X = 1280 - heartTexture.Width;
                }

                if (stab_countdown != 0)
                {
                    frameCountdown--;
                    if (frameCountdown == 0)
                    {
                        frameCountdown = 40;
                        stab_countdown--;
                        countdown--;
                        Random random = new Random();
                        if (stab_countdown != 0) syringeGoal = random.Next(0, 1280 - syringeTexture.Width);
                    }
                    syringePosition.X += (syringeGoal - syringePosition.X) * 0.1f;
                }
                else if (!hit)
                {
                    syringePosition.Y += 20;
                }
                if (hit || miss)
                {
                    quitTimer--;
                }
                if (quitTimer == 0)
                {
                    if (hit) {
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
                    else { 
                        Util.GotoRandomMinigame(game);
                    }                        
                }
                
                syringeRect = new Rectangle((int)syringePosition.X + 28, (int)syringePosition.Y + 227, 2, 56);
                heartRect = new Rectangle((int)heartPosition.X, (int)heartPosition.Y + 100, (int)heartTexture.Width, (int)heartTexture.Height - 100);
                if (syringeRect.Intersects(heartRect))
                {
                    hit = true;
                }
                if (syringePosition.Y == 720)
                {
                    miss = true;
                }
            }
            else
            {
                frameCountdown--;
                if (frameCountdown == 0)
                {
                    frameCountdown = 40;
                    countdown--;
                }
            }
        }

        void GameState.Draw(PlatformerGame game, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(heartTexture, heartPosition, Color.White);
            spriteBatch.Draw(syringeTexture, syringePosition, Color.White);

            if (countdown > 3)
            {
                spriteBatch.DrawString(game.font, "STAB", new Vector2(480, 100), Color.White, 0f, Vector2.Zero, 5f, SpriteEffects.None, 0f);
            }
            else if (countdown > 0)
            {
                spriteBatch.DrawString(game.font, "" + countdown, new Vector2(630, 100), Color.White, 0f, Vector2.Zero, 5f, SpriteEffects.None, 0f);
            }

            if (stab_countdown > 0 && stab_countdown < 4)
                spriteBatch.DrawString(game.font, ""+stab_countdown, new Vector2(630, 100), Color.Red, 0f, Vector2.Zero, 5f, SpriteEffects.None, 0f);
            if (hit)
                spriteBatch.DrawString(game.font, "GOOD!", new Vector2(450, 100), Color.White, 0f, Vector2.Zero, 5f, SpriteEffects.None, 0f);
            if (miss)
                spriteBatch.DrawString(game.font, "FAIL!", new Vector2(450, 100), Color.White, 0f, Vector2.Zero, 5f, SpriteEffects.None, 0f);
        }
    }
}
