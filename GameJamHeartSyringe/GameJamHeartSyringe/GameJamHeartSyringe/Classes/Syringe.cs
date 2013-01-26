using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace GameJamHeartSyringe
{
    class Syringe : GameItem
    {

        public override void Update(int screenWidth, int screenHeight, KeyboardState keyboardState)
        {
            if (keyboardState.IsKeyDown(Keys.D))
                Position = new Vector2(Position.X, Position.Y + 100);

            

        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Sprite, Position, SpriteColor);
        }
    }
}
