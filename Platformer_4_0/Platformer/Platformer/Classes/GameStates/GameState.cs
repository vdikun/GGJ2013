using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Platformer
{
    public interface GameState
    {
        void Update(PlatformerGame game, GameTime gameTime);
        void Draw(PlatformerGame game, SpriteBatch spriteBatch);
    }
}
