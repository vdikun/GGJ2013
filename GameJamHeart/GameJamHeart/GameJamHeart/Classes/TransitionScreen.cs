using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace GameJamHeart
{
    class TransitionScreen
    {

        private int fadeOutAlpha;
        private int fadeInAlpha;

        private int ScreenWidth;
        private int ScreenHeight;

        private double DrawFadeCallTime;

        private bool cueFade = false;
        private bool cueFadeIn = false;

        public bool hitBlack = false;

        public TransitionScreen(int screenWidth, int screenHeight)
        {
            this.ScreenWidth = screenWidth;
            this.ScreenHeight = screenHeight;
            fadeOutAlpha = 0;
            fadeInAlpha = 255;
            DrawFadeCallTime = 0;
        }

        public void DrawFadeToBlack(SpriteBatch spriteBatch, GameTime gameTime, float fadeTimeSeconds, Texture2D whitePixel)
        {
            if (fadeOutAlpha < 255)
            {
                if (DrawFadeCallTime == 0)
                    DrawFadeCallTime = gameTime.TotalGameTime.TotalMilliseconds;

                double totalElapsedTime = gameTime.TotalGameTime.TotalMilliseconds - DrawFadeCallTime;
                float MSperAlpha = fadeTimeSeconds * 1000 / 255;

                fadeOutAlpha = (int)(totalElapsedTime / MSperAlpha);
            }
            else
            {
                DrawFadeCallTime = 0;
            }

            spriteBatch.Draw(whitePixel, new Rectangle(0, 0, ScreenWidth, ScreenHeight), new Color(0, 0, 0, fadeOutAlpha));

        }



        public void DrawFadeInFromBlack(SpriteBatch spriteBatch, GameTime gameTime, float fadeTimeSeconds, Texture2D whitePixel)
        {
            if (fadeInAlpha > 0)
            {
                if (DrawFadeCallTime == 0)
                    DrawFadeCallTime = gameTime.TotalGameTime.TotalMilliseconds;

                double totalElapsedTime = gameTime.TotalGameTime.TotalMilliseconds - DrawFadeCallTime;
                float MSperAlpha = fadeTimeSeconds * 1000 / 255;

                fadeInAlpha = 255 - (int)(totalElapsedTime / MSperAlpha);
            }
            else
            {
                DrawFadeCallTime = 0;

            }

            spriteBatch.Draw(whitePixel, new Rectangle(0, 0, ScreenWidth, ScreenHeight), new Color(0, 0, 0, fadeInAlpha));
        }

        public void DrawFadeBlackAndBack(SpriteBatch spriteBatch, GameTime gameTime, float fadeOutTimeSeconds, float fadeInTimeSeconds, Texture2D whitePixel)
        {
            if (fadeOutAlpha <= 255 && !hitBlack)
            {
                if (DrawFadeCallTime == 0)
                    DrawFadeCallTime = gameTime.TotalGameTime.TotalMilliseconds;

                double totalElapsedTime = gameTime.TotalGameTime.TotalMilliseconds - DrawFadeCallTime;
                float MSperAlpha = fadeOutTimeSeconds * 1000 / 255;

                fadeOutAlpha = (int)(totalElapsedTime / MSperAlpha);

                //spriteBatch.Draw(whitePixel, new Rectangle(0, 0, ScreenWidth, ScreenHeight), new Color(0, 0, 0, fadeOutAlpha));
            }

            if (fadeOutAlpha >= 255 && !hitBlack)
            {
                hitBlack = true;
                DrawFadeCallTime = 0;
            }

            if (hitBlack == true)
            {
                if (fadeInAlpha > 0)
                {
                    if (DrawFadeCallTime == 0)
                        DrawFadeCallTime = gameTime.TotalGameTime.TotalMilliseconds;

                    double totalElapsedTime = gameTime.TotalGameTime.TotalMilliseconds - DrawFadeCallTime;
                    float MSperAlpha = fadeInTimeSeconds * 1000 / 255;

                    fadeInAlpha = 255 - (int)(totalElapsedTime / MSperAlpha);

                    //spriteBatch.Draw(whitePixel, new Rectangle(0, 0, ScreenWidth, ScreenHeight), new Color(0, 0, 0, fadeInAlpha));
                }
            }
            if (hitBlack && fadeInAlpha <= 0)
            {
                DrawFadeCallTime = 0;
                cueFade = false;
                hitBlack = false;
                fadeInAlpha = 255;
                fadeOutAlpha = 0;
            }
        }

        public bool IsFaded()
        {
            if (fadeOutAlpha >= 255)
            {
                cueFade = false;
                return true;

            }
            else
                return false;
        }

        public bool IsFadedIn()
        {
            if (fadeInAlpha <= 0)
            {
                cueFadeIn = false;
                return true;

            }
            else
                return false;
        }

        public void CueFade()
        {

            cueFade = true;

            fadeOutAlpha = 0;

        }

        public void RemoveFadeCue()
        {
            cueFade = false;
            fadeOutAlpha = 0;
        }

        public void CueFadeIn()
        {
            cueFadeIn = true;
            fadeInAlpha = 255;
        }

        public void RemoveFadeInCue()
        {
            cueFadeIn = false;
            fadeInAlpha = 255;
        }

        public bool FadeCued()
        {
            return (cueFade);
        }

        public bool FadeInCued()
        {
            return (cueFadeIn);
        }
    }
}
