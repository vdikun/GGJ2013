﻿using System;
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
using Microsoft.Xna.Framework.Audio;

namespace Dozer
{
    class MenuState : GameState
    {
        //Controls
        static Keys[] acceptKeys = new Keys[] { Keys.Enter, Keys.Space, Keys.D, Keys.Right };
        static Keys[] cancelKeys = new Keys[] { Keys.Escape };
        static Keys[] upKeys = new Keys[] { Keys.W, Keys.Up };
        static Keys[] downKeys = new Keys[] { Keys.S, Keys.Down };

        //Buttons
        static int selectedButton = 0;
        static int totalButtons = 0;

        //Art
        static Texture2D background;

        //Constants
        static Vector2 POSITION_TITLE = new Vector2(20, 20);
        static String TITLE_TEXT = "Dr. Dozer, Emergency Transplant Surgeon";
        static Vector2 ORIGIN = new Vector2(0, 0);
        static String PROMPT_TEXT = "Press Space to Begin";

        //Music
        static SoundEffect music;

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
                this.y = 70 + (30 * totalButtons);
                this.action = action;
            }
        }

        static Button[] buttons = new Button[] {
            /*
            new Button("Platforming", delegate(PlatformerGame game) {
                game.currentState = new PlatformState();
            }),
             
            new Button("The Game", delegate(PlatformerGame game) {
                game.currentState = new FreePlatformState();
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
            new Button("Mini Game 5", delegate(PlatformerGame game) {
                game.currentState = new MiniGame5State();
            }),
            new Button("Exit", delegate(PlatformerGame game) {
                game.Exit();
            }),
             * */
        };

        private delegate void ButtonAction(Main game);

        public MenuState()
        {
            Main.music = music;
            if (Main.musicLoop != null) Main.musicLoop.Stop();
            if (Main.musicIntro != null) Main.musicIntro.Stop();
        }

        public static void LoadContent(ContentManager manager)
        {
            background = manager.Load<Texture2D>("Backgrounds/Splash");
            music = manager.Load<SoundEffect>("Sounds/titlescreen");
        }

        void GameState.Update(Main game, GameTime gameTime)
        {
            if (Util.IsAnyKeyPressed(game.keyboard, game.prevKeyboard, cancelKeys))
            {
                game.Exit();
            }

            if (Util.IsAnyKeyDown(game.keyboard, acceptKeys))
            {
                //buttons[selectedButton].action.BeginInvoke(game, null, null);
                game.currentState = new PlatformingState();
            }

            /*
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
             * */
        }

        void GameState.Draw(Main game, SpriteBatch spriteBatch)
        {
            float scale = 1000 / Main.SCREEN_WIDTH;
            spriteBatch.Draw(background, new Rectangle(0, 50, (int)Main.SCREEN_WIDTH, (int)(Main.SCREEN_HEIGHT * scale)), Color.White);

            // Draw title with white outline
            spriteBatch.DrawString(game.headerFont, TITLE_TEXT, POSITION_TITLE + new Vector2(-2, -2), Color.White, 0, ORIGIN, 1, SpriteEffects.None, 1f);
            spriteBatch.DrawString(game.headerFont, TITLE_TEXT, POSITION_TITLE + new Vector2(-2, +2), Color.White, 0, ORIGIN, 1, SpriteEffects.None, 1f);
            spriteBatch.DrawString(game.headerFont, TITLE_TEXT, POSITION_TITLE + new Vector2(+2, +2), Color.White, 0, ORIGIN, 1, SpriteEffects.None, 1f);
            spriteBatch.DrawString(game.headerFont, TITLE_TEXT, POSITION_TITLE + new Vector2(+2, -2), Color.White, 0, ORIGIN, 1, SpriteEffects.None, 1f);
            spriteBatch.DrawString(game.headerFont, TITLE_TEXT, POSITION_TITLE, Color.Black);
            // 
            Vector2 offset = game.font.MeasureString(PROMPT_TEXT);
            spriteBatch.DrawString(game.font, PROMPT_TEXT, new Vector2(505, 680), Color.Black);

            foreach (Button button in buttons)
            {
                Color color = selectedButton == button.index ? Color.Red : Color.White;
                spriteBatch.Draw(game.placeholderTexture, new Rectangle(button.x, button.y, 20, 20), color);
                spriteBatch.DrawString(game.font, button.text, new Vector2(button.x + 25, button.y), color);
            }
        }
    }
}

