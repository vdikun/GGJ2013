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

            public Button(string text, ButtonAction action)
            {
                this.index = totalButtons;
                totalButtons++;
                this.text = text;
                this.x = 60;
                this.y = 60 + (30*totalButtons);
                this.action = action;
            }
        }

        static Button[] buttons = new Button[] {
            new Button("Platforming", delegate(PlatformerGame game) {
                game.currentState = new PlatformState();
            }),
            new Button("Free Platforming", delegate(PlatformerGame game) {
                game.currentState = new FreePlatformState();
            }),
            new Button("Mini Games", delegate(PlatformerGame game) {
                //game.currentState = new MiniGameState();
                Random random = new Random();
                int randInt = random.Next(0, 4);
                randInt = 0; //REMOVE THIS ONCE ALL MINIGAMES ARE INTEGRATED
                switch (randInt)
                {
                    case 0:
                        game.currentState = new MiniGame1State();
                        break;
                    case 1:
                        game.currentState = new MiniGame2State();
                        break;
                    case 2:
                        game.currentState = new MiniGame3State();
                        break;
                    case 3:
                        game.currentState = new MiniGame4State();
                        break;
                    default:
                        break;
                }
            }),
            new Button("Mini Game 1", delegate(PlatformerGame game) {
                game.currentState = new MiniGame1State();
            }),
            new Button("Mini Game 2", delegate(PlatformerGame game) {
                game.currentState = new MiniGame2State();
            }),
            new Button("Mini Game 3", delegate(PlatformerGame game) {
                game.currentState = new MiniGame3State();
            }),
            new Button("Mini Game 4", delegate(PlatformerGame game) {
                game.currentState = new MiniGame4State();
            }),
            new Button("Exit", delegate(PlatformerGame game) {
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
