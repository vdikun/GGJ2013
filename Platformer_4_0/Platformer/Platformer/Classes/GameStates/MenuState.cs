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
using Microsoft.Xna.Framework.Content;

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

        //Art
        static Texture2D background;

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

        static Button[] buttons = new Button[] {
            new Button("Platforming", 60, 60, delegate(PlatformerGame game) {
                game.currentState = new PlatformState();
            }),
            new Button("Free Platforming", 60, 90, delegate(PlatformerGame game) {
                game.currentState = new FreePlatformState();
            }),
            new Button("Mini Games", 60, 120, delegate(PlatformerGame game) {
                game.currentState = new MiniGameState();
            }),
            new Button("Exit", 60, 150, delegate(PlatformerGame game) {
                game.Exit();
            }),
        };

        private delegate void ButtonAction(PlatformerGame game);

        public static void LoadContent(ContentManager manager)
        {
            background = manager.Load<Texture2D>("Backgrounds/Splash");
        }

        void GameState.Update(PlatformerGame game, GameTime gameTime)
        {
            if (Util.IsAnyKeyPressed(game.keyboard, game.prevKeyboard, cancelKeys))
            {
                game.Exit();
            }

            if (Util.IsAnyKeyDown(game.keyboard, acceptKeys))
            {
                buttons[selectedButton].action.BeginInvoke(game, null, null);
            }

            if (Util.IsAnyKeyPressed(game.keyboard, game.prevKeyboard, downKeys))
            {
                selectedButton++;
                if (selectedButton >= totalButtons) selectedButton = 0;
            }

            if (Util.IsAnyKeyPressed(game.keyboard, game.prevKeyboard, upKeys))
            {
                selectedButton--;
                if (selectedButton < 0) selectedButton = totalButtons-1;
            }
        }

        void GameState.Draw(PlatformerGame game, SpriteBatch spriteBatch)
        {
            float scale = 1000 / PlatformerGame.SCREEN_WIDTH;
            spriteBatch.Draw(background, new Rectangle(0, 0, (int) PlatformerGame.SCREEN_WIDTH, (int) (PlatformerGame.SCREEN_HEIGHT * scale)), Color.White);
            spriteBatch.DrawString(game.font, "Menu State", new Vector2(10, 10), Color.White);

            foreach (Button button in buttons) {
                Color color = selectedButton == button.index ? Color.Red : Color.White;
                spriteBatch.Draw(game.placeholderTexture, new Rectangle(button.x, button.y, 20, 20), color);
                spriteBatch.DrawString(game.font, button.text, new Vector2(button.x + 25, button.y), color);
            }
        }
    }
}
