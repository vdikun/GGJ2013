using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace GameJamHeart
{
    class GameItem
    {

        private Texture2D sprite;
        private Vector2 position;
        private Color spriteColor;

        public Texture2D Sprite
        {
            get { return sprite; }
            set { sprite = value; }
        }
        public Vector2 Position
        {
            get { return position; }
            set { position = value; }
        }
        public Color SpriteColor
        {
            get { return spriteColor; }
            set { spriteColor = value; }
        }

        public GameItem()
        {
            SpriteColor = Color.White;
        }

        public virtual void Update(int screenWidth, int screenHeight)
        {

        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(sprite, position, spriteColor);
        }

        public virtual void CenterSprite(int screenWidth, int screenHeight)
        {
            position = new Vector2(screenWidth / 2 - sprite.Width / 2, screenHeight / 2 - sprite.Height / 2);
        }



    }
}
