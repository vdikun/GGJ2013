using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace GameJamHeartSyringe
{
    class Heart : GameItem
    {
        private const int MARGIN = 100;
        private int heartSpeed = 20;
        
        public void Update(int screenWidth, int screenHeight, Syringe syringe)
        {
            if (Position.X > screenWidth - MARGIN - Sprite.Width)
                heartSpeed *= -1;
            else if (Position.X < MARGIN)
                heartSpeed *= -1;
            Position = new Vector2(Position.X + heartSpeed, Position.Y);

            CheckCollision(syringe);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Sprite, Position, SpriteColor);
        }

        public void CheckCollision(Syringe syringe)
        {
            Rectangle heartHitBox = new Rectangle((int)this.Position.X, (int)this.Position.Y, this.Sprite.Width, this.Sprite.Height);
            Rectangle syringeHitBox = new Rectangle((int)syringe.Position.X, (int)syringe.Position.Y, syringe.Sprite.Width, syringe.Sprite.Height);

            if (heartHitBox.Intersects(syringeHitBox))
                this.Position = new Vector2(2000, 0);
        }
    }
}
