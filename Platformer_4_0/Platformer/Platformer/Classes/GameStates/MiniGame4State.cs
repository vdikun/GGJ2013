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
    class MiniGame4State : GameState
    {
        static Texture2D heartTexture0, heartTexture1, heartTexture2;
        Vector2 heartPosition = new Vector2(240, 240);
        MouseState oldMouseState, currentMouseState;
        int shakeCounter;
        int heartState;
        int countdown;
        int frameCounter;

        // constants
        private const int CHANGE_X_PER_FRAME = 400;
        private int SHAKE_COUNTER = 16; // how long you want to move mouse at specified speed to reduce dirt by 1
        

        public MiniGame4State()
        {
            oldMouseState = new MouseState();
            oldMouseState = Mouse.GetState();
            currentMouseState = new MouseState();
            shakeCounter = SHAKE_COUNTER;
            countdown = 4; // number of sprites for dirty hearts
            frameCounter = 40;
        }

        public static void LoadContent(ContentManager manager)
        {
            heartTexture0 = manager.Load<Texture2D>("Minigames/heartDirty2");
            heartTexture1 = manager.Load<Texture2D>("Minigames/heartDirty1");
            heartTexture2 = manager.Load<Texture2D>("Minigames/heart");

        }

        void GameState.Update(PlatformerGame game, GameTime gameTime)
        {
            if (game.keyboard.IsKeyDown(Keys.Escape))
            {
                game.currentState = new MenuState();
            }

            if (countdown != 0)
            {
                frameCounter--;
                if (frameCounter == 0) { countdown--; frameCounter = 40; }
            }
            else
            {
                if (heartState != 2)
                {
                    // Update heart position according to mouse position
                    currentMouseState = Mouse.GetState();
                    float posDiff = currentMouseState.X - heartPosition.X;
                    heartPosition.X += posDiff * 0.1f;
                    if (heartPosition.X < 0) heartPosition.X = 0;
                    if (heartPosition.X > 1280 - heartTexture0.Width) heartPosition.X = 1280 - heartTexture0.Width;
                    posDiff = currentMouseState.Y - heartPosition.Y;
                    heartPosition.Y += posDiff * 0.1f;
                    if (heartPosition.Y < 0) heartPosition.Y = 0;
                    if (heartPosition.Y > 720 - heartTexture0.Height) heartPosition.Y = 720 - heartTexture0.Height;

                    // Check if mouse is moved with a certain velocity
                    if ((currentMouseState.X - oldMouseState.X) > CHANGE_X_PER_FRAME)
                    {
                        shakeCounter--;
                    }

                    // if mouse has been moved with specified velocity, remove dirt (change texture)
                    if (shakeCounter == 8)
                    {
                        heartState = 1;
                    }
                    else if (shakeCounter == 0)
                    {
                        heartState = 2;
                    }

                    oldMouseState = currentMouseState;
                }
            }

        }

        void GameState.Draw(PlatformerGame game, SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(game.font, "Mini Game 3 State", new Vector2(10, 10), Color.White);

            if (countdown != 0) spriteBatch.DrawString(game.font, "" + countdown, new Vector2(630, 100), Color.White, 0f, Vector2.Zero, 5f, SpriteEffects.None, 0f);

            if (shakeCounter >= 3 && shakeCounter <= 8) spriteBatch.DrawString(game.font, "Almost there..", new Vector2(450, 100), Color.White, 0f, Vector2.Zero, 2f, SpriteEffects.None, 0f);
            
            switch (heartState)
            {
                case 0: spriteBatch.Draw(heartTexture0, heartPosition, Color.White);
                    break;
                case 1: spriteBatch.Draw(heartTexture1, heartPosition, Color.White);
                    break;
                case 2: spriteBatch.Draw(heartTexture2, heartPosition, Color.White);
                        spriteBatch.DrawString(game.font, "GOOD!", new Vector2(450, 100), Color.White, 0f, Vector2.Zero, 5f, SpriteEffects.None, 0f);
                    break;
            }
        }
    }
}
