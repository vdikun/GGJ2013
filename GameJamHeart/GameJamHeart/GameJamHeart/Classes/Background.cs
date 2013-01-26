using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace GameJamHeart
{
    class Background
    {

        private int slidePosition;
        private int slideSpeed;

        private float bgSpeed;
        public float BGSpeed
        {
            get { return bgSpeed; }
            set { bgSpeed = value; }
        }


        private List<Texture2D> bgList;
        public List<Texture2D> BGList
        {
            get { return bgList; }
            set { bgList = value; }
        }


        public Background()
        {
            BGList = new List<Texture2D>();
            bgSpeed = 8;
            slidePosition = 0;
            slideSpeed = 1;
        }


        public void Draw(SpriteBatch spriteBatch, int screenWidth, int screenHeight, GameTime gameTime)
        {
            int cycleTime = (int)(1000 / (BGSpeed / 4));

            int frameTime = (cycleTime / bgList.Count());

            int BGIndex = ((int)gameTime.TotalGameTime.TotalMilliseconds % cycleTime) / frameTime;

            spriteBatch.Draw(BGList[BGIndex], new Rectangle(0, 0, screenWidth, screenHeight), Color.White);
        }

        public void DrawSliding(SpriteBatch spriteBatch, int screenWidth, int screenHeight, GameTime gameTime)
        {
            int cycleTime = (int)(1000 / (BGSpeed / 4));

            int frameTime = (cycleTime / bgList.Count());

            int BGIndex = ((int)gameTime.TotalGameTime.TotalMilliseconds % cycleTime) / frameTime;

            spriteBatch.Draw(BGList[BGIndex], new Rectangle(slidePosition, 0, screenWidth, screenHeight), Color.White);
            spriteBatch.Draw(BGList[BGIndex], new Rectangle(slidePosition + screenWidth, 0, screenWidth, screenHeight), Color.White);

            slidePosition -= slideSpeed;

            if (slidePosition == -screenWidth)
                slidePosition = 0;
        }
    }
}
