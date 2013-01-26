using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace Platformer
{
    public interface GameState
    {
        void Update(PlatformerGame game);
        void Draw(PlatformerGame game, SpriteBatch spriteBatch);
    }
}
