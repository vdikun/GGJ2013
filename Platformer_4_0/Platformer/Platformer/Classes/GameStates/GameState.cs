using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Dozer
{
    public interface GameState
    {
        void Update(Main game, GameTime gameTime);
        void Draw(Main game, SpriteBatch spriteBatch);
    }
}
