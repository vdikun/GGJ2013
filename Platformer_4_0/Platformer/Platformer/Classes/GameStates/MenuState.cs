using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Input.Touch;

namespace Platformer
{
    class MenuState : GameState
    {
        int selectedButton = 0;
        const int totalButtons = 2;

        void GameState.Update(PlatformerGame game)
        {
            if (game.keyboard.IsKeyDown(Keys.S))
            {
                selectedButton++;
                if (selectedButton > totalButtons) selectedButton = 0;
            }

            if (game.keyboard.IsKeyDown(Keys.W))
            {
                selectedButton--;
                if (selectedButton < 0) selectedButton = totalButtons-1;
            }
        }

        void GameState.Draw(PlatformerGame game, SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(game.font, "Menu State", new Vector2(10, 10), Color.White);


            Color color = Color.White;

            color = selectedButton == 0 ? Color.Red : Color.White;
            spriteBatch.Draw(game.placeholderTexture, new Rectangle(60, 60, 100, 20), color);

            color = selectedButton == 1 ? Color.Red : Color.White;
            spriteBatch.Draw(game.placeholderTexture, new Rectangle(60, 90, 100, 20), color);
        }
    }
}
