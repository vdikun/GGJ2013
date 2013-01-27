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
    class MiniGame2State : GameState
    {

        static SoundEffect soundEffect;

        static Texture2D heartTexture;
        static Texture2D syringeTexture;

        Vector2 heartPosition = new Vector2(0, 400);
        Vector2 syringePosition = new Vector2(0, 0);


        public static void LoadContent(ContentManager manager)
        {
            heartTexture = manager.Load<Texture2D>("Minigames/heart");
            syringeTexture = manager.Load<Texture2D>("Minigames/syringe");
            soundEffect = manager.Load<SoundEffect>("Voices/dr_kapow_01");
        }



        void GameState.Update(PlatformerGame game, GameTime gameTime)
        {
            if (game.keyboard.IsKeyDown(Keys.Escape))
            {
                game.currentState = new MenuState();
            }


            if (game.keyboard.IsKeyDown(Keys.R))
            {
                
                soundEffect.Play();
            }
        }

        void GameState.Draw(PlatformerGame game, SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(game.font, "Mini Game 2 State", new Vector2(10, 10), Color.White);
        }
    }
}
