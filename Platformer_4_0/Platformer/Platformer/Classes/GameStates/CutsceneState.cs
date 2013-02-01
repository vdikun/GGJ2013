using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;


namespace Dozer
{
    class CutsceneState : GameState
    {
        static Texture2D background;
        static SoundEffect music;

        public static void LoadContent(ContentManager manager)
        {
            background = manager.Load<Texture2D>("Backgrounds/Splash");
            music = manager.Load<SoundEffect>("Sounds/victory_music");
        }

        public CutsceneState()
        {
            Main.music = music;
            if (Main.musicLoop != null) Main.musicLoop.Stop();

            Main.SubmitResult("Victory", 1);
        }

        void GameState.Update(Main game, GameTime gameTime)
        {
            if (game.keyboard.IsKeyDown(Keys.Space))
            {
                game.currentState = new MenuState();
            }
        }

        void GameState.Draw(Main game, SpriteBatch spriteBatch)
        {
            float scale = 1000 / Main.SCREEN_WIDTH;
            spriteBatch.Draw(background, new Rectangle(0, 0, (int)Main.SCREEN_WIDTH, (int)(Main.SCREEN_HEIGHT * scale)), Color.White);
        }
    }
}
