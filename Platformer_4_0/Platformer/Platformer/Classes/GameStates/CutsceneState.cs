using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;


namespace Platformer
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
            PlatformerGame.music = music;
        }

        void GameState.Update(PlatformerGame game, GameTime gameTime)
        {
            if (game.keyboard.IsKeyDown(Keys.Space))
            {
                game.currentState = new MenuState();
            }
        }

        void GameState.Draw(PlatformerGame game, SpriteBatch spriteBatch)
        {
            float scale = 1000 / PlatformerGame.SCREEN_WIDTH;
            spriteBatch.Draw(background, new Rectangle(0, 0, (int)PlatformerGame.SCREEN_WIDTH, (int)(PlatformerGame.SCREEN_HEIGHT * scale)), Color.White);

        }
    }
}
