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
        //Controls
        static Keys[] acceptKeys = new Keys[] { Keys.Enter, Keys.Space, Keys.D, Keys.Right };
        static Keys[] cancelKeys = new Keys[] { Keys.Escape };
        static Keys[] upKeys     = new Keys[] { Keys.W, Keys.Up };
        static Keys[] downKeys   = new Keys[] { Keys.S, Keys.Down };
        
        //Buttons
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
            new Button("Exit", 60, 120, delegate(PlatformerGame game) {
                game.Exit();
            }),
        };

        private delegate void ButtonAction(PlatformerGame game);

        void GameState.Update(PlatformerGame game)
        {
            if (IsAnyKeyDown(game.keyboard, cancelKeys))
            {
                game.Exit();
            }

            if (IsAnyKeyDown(game.keyboard, acceptKeys))
            {
                buttons[selectedButton].action.BeginInvoke(game, null, null);
            }

            if (IsAnyKeyPressed(game.keyboard, game.prevKeyboard, downKeys))
            {
                selectedButton++;
                if (selectedButton >= totalButtons) selectedButton = 0;
            }

            if (IsAnyKeyPressed(game.keyboard, game.prevKeyboard, upKeys))
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

        bool IsAnyKeyDown(KeyboardState keyboard, Keys[] keys)
        {
            foreach (Keys key in keys) {
                if (keyboard.IsKeyDown(key))
                    return true;
            }
            return false;
        }

        bool IsAnyKeyUp(KeyboardState keyboard, Keys[] keys)
        {
            foreach (Keys key in keys) {
                if (keyboard.IsKeyUp(key))
                    return true;
            }
            return false;
        }

        bool IsAnyKeyPressed(KeyboardState keyboard, KeyboardState prevKeyboard, Keys[] keys)
        {
            foreach (Keys key in keys) {
                if (keyboard.IsKeyDown(key) && prevKeyboard.IsKeyUp(key))
                    return true;
            }
            return false;
        }
    }
}
