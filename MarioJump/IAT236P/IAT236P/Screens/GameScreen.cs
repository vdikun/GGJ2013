using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using IAT236P.Services;
using IAT236P.GameObjects;
using Microsoft.Xna.Framework.Input;

namespace IAT236P.Screens
{
    public class GameScreen : Screen
    {
        private SpriteBatch mSpriteBatch;
        private Player mPlayer;

        public GameScreen() 
        {
            //IsActive = true;
            mSpriteBatch = ServiceLocator.GetService<SpriteBatch>();
            mPlayer = new Player();
        }

        public override void Update(GameTime gameTime)
        {
            mPlayer.Update(gameTime);
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            mSpriteBatch.Begin();
            mSpriteBatch.Draw(mPlayer.Texture, mPlayer.Position, null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
            mSpriteBatch.End();
        }
    }

    public static class GameScreenStaticProperties
    {
        public static Vector2 mStartingPosition = new Vector2(100, 200);
    }
}
