using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameJamHeart
{

    //Jer's resolution fixer (to work on other computers/resolutions)
    static class Resolution
    {
        public static Matrix GetTransformMatrix(GraphicsDeviceManager graphics, Vector2 baseScreenSize, bool stretchScreen)
        {
            // sets form size to size of resolution
            graphics.PreferredBackBufferWidth = graphics.GraphicsDevice.DisplayMode.Width;
            graphics.PreferredBackBufferHeight = graphics.GraphicsDevice.DisplayMode.Height;
            graphics.ApplyChanges();


            //create scale factor based on the ratio of current resolution and jer's 16:9 resolution
            //////////////////////////
            Vector3 screenScalingFactor;

            float horScaling = (float)graphics.GraphicsDevice.DisplayMode.Width / baseScreenSize.X;
            float verScaling = (float)graphics.GraphicsDevice.DisplayMode.Height / baseScreenSize.Y;
            //screenScalingFactor = new Vector3(horScaling, verScaling, 1);

            if (stretchScreen)
                screenScalingFactor = new Vector3(horScaling, verScaling, 1);
            else
                screenScalingFactor = new Vector3(horScaling, horScaling, 1);


            Matrix globalTransformation = Matrix.CreateScale(screenScalingFactor);

            if (!stretchScreen)
            {
                Viewport viewport = new Viewport();

                float scaledWidth = graphics.GraphicsDevice.DisplayMode.Width;
                float scaledHeight = baseScreenSize.Y * horScaling;

                viewport.X = 0;
                viewport.Y = (int)((graphics.GraphicsDevice.DisplayMode.Height - scaledHeight) / 2);
                viewport.Width = graphics.GraphicsDevice.Viewport.Width;
                viewport.Height = (int)scaledHeight;
                viewport.MinDepth = 0;
                viewport.MaxDepth = 1;

                float test = viewport.MinDepth;
                float test2 = viewport.MaxDepth;

                graphics.GraphicsDevice.Viewport = viewport;
            }

            return globalTransformation;
        }

    }
}
