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
        static int selectedButton = 0;
        static int totalButtons = 0;

        private struct Button
        {
            public int index;
            public int x, y;
            public string text;
            public ButtonAction action;

            public Button(string text, int x, int y, ButtonAction action)
            {
                this.index = MenuState.totalButtons;
                totalButtons++;
                this.text = text;
                this.x = x;
                this.y = y;
                this.action = action;
            }
        }

        Button[] buttons = new Button[] {
            new Button("Platforming", 60, 60, delegate(PlatformerGame game) {
                game.currentState = new PlatformState();
            }),
            new Button("Mini Games", 60, 90, delegate(PlatformerGame game) {
                game.currentState = new MiniGameState();
            }),
        };

        private delegate void ButtonAction(PlatformerGame game);

        void GameState.Update(PlatformerGame game)
        {
            if (game.keyboard.IsKeyDown(Keys.Enter))
            {
                buttons[selectedButton].action.BeginInvoke(game, null, null);
            }

            if (game.keyboard.IsKeyDown(Keys.S) && game.prevKeyboard.IsKeyUp(Keys.S))
            {
                selectedButton++;
                if (selectedButton >= totalButtons) selectedButton = 0;
            }

            if (game.keyboard.IsKeyDown(Keys.W) && game.prevKeyboard.IsKeyUp(Keys.W))
            {
                selectedButton--;
                if (selectedButton < 0) selectedButton = totalButtons-1;
            }
        }

        void GameState.Draw(PlatformerGame game, SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(game.font, "Menu State", new Vector2(10, 10), Color.White);

            foreach (Button button in buttons) {
                Color color = selectedButton == button.index ? Color.Red : Color.White;
                spriteBatch.Draw(game.placeholderTexture, new Rectangle(button.x, button.y, 20, 20), color);
                spriteBatch.DrawString(game.font, button.text, new Vector2(button.x + 25, button.y), color);
            }
        }
    }
}
