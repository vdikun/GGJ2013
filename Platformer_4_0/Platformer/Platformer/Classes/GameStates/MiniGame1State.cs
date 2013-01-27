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

        static Texture2D heartTexture;
        static Texture2D syringeTexture;

        Vector2 heartPosition = new Vector2(0, 400);
        Vector2 syringePosition = new Vector2(0, 0);

        public static void LoadContent(ContentManager manager)
        {
            heartTexture = manager.Load<Texture2D>("Minigames/heart");
            syringeTexture = manager.Load<Texture2D>("Minigames/syringe");
        }

        void GameState.Update(PlatformerGame game, GameTime gameTime)
        {
            if (game.keyboard.IsKeyDown(Keys.Escape))
            {
                game.currentState = new MenuState();
            }

            float posDiff = Mouse.GetState().X - heartPosition.X;
            heartPosition.X += posDiff*0.1f;
            if (heartPosition.X < 0) heartPosition.X = 0;
            if (heartPosition.X > 1280 - heartTexture.Width) heartPosition.X = 1280 - heartTexture.Width;
        }

        void GameState.Draw(PlatformerGame game, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(heartTexture, heartPosition, Color.White);

            spriteBatch.DrawString(game.font, "Mini Game 1 State", new Vector2(10, 10), Color.White);
        }
    }
}
