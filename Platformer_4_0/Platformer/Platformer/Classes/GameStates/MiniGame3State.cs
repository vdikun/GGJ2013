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
    class MiniGame3State : GameState
    {
        Heart heart = new Heart();
        static SpriteFont spriteFont;
        int heartMeter;

        public MiniGame3State()
        {
        }

        public static void LoadContent(ContentManager manager)
        {
            Heart.LoadContent(manager);
            spriteFont = manager.Load<SpriteFont>("Fonts/Hud");
            
        }

        void GameState.Update(PlatformerGame game, GameTime gameTime)
        {
            if (game.keyboard.IsKeyDown(Keys.Escape))
            {
                game.currentState = new MenuState();
            }
            heart.Update(gameTime);
            heartMeter = heart.heartMeter;
        }

        void GameState.Draw(PlatformerGame game, SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(game.font, "Mini Game 3 State", new Vector2(10, 10), Color.White);
            heart.Draw(spriteBatch, 200, 100);
            DrawText(spriteBatch);
        }

        private void DrawText(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(spriteFont, "Heart power: " + heartMeter, new Vector2(20, 45), Color.White);
        }

    }
}
