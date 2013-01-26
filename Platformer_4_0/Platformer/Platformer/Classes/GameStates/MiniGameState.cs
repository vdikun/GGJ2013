using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Platformer
{
    class MiniGameState : GameState
    {
        void GameState.Update(PlatformerGame game)
        {

        }

        void GameState.Draw(PlatformerGame game, SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(game.font, "Mini Game State", new Vector2(10, 10), Color.White);
        }
    }
}
