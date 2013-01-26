using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;


namespace gamejam_isha_1
{
    enum Click
    {
        Left = 0,
        Right = 1
    }

    class Heart
    {
        
        private MouseState previousMouseState;
        
        // constants
        private const int BEAT_PATTERN_LENGTH = 3;
        private const int HEART_METER_UP_BOUND = 100;
        private const int HEART_METER_LOW_BOUND = 0;
        private const int HEART_METER_MATCH_VALUE = 10;
        private const int HEART_METER_DECREASE_VALUE = -1;
        private const float TIMER = 1.0F;

        // variables
        private int heartMeter;
        private Click[] beatPattern, clickPattern;
        private int beatPatternLength;
        private int level;
        private float timer;

        
        public Heart(int heartLevel)
        {
            level = heartLevel;
            heartMeter = 40;
            beatPatternLength = BEAT_PATTERN_LENGTH;
            beatPattern = new Click[beatPatternLength];
            assignBeatPattern();
            clickPattern = new Click[beatPatternLength];

        }

        public int getHeartMeter() { return heartMeter; }

        void changeHeartMeter(int change) 
        {
            if ((heartMeter + change) >= HEART_METER_LOW_BOUND || (heartMeter + change) <= HEART_METER_UP_BOUND) 
            { heartMeter = heartMeter + change; } 
        }

        void assignBeatPattern()
        {
            Random random = new Random();
            for (int i = 0; i < beatPattern.Length; i++)
            {
                int rnd = random.Next(0, 2);
                beatPattern[i] = (Click) rnd;
                Console.WriteLine(rnd);
            }
                
        }

        bool matchBeatPattern()
        {
            if (clickPattern.Length != beatPattern.Length)
                return false;
            else
                for (int i = 0; i < clickPattern.Length; i++)
                {
                    if (clickPattern[i] != beatPattern[i])
                        return false;
                }
            return true;
        }


        void addToClickPattern(Click click)
        {
            for (int i = 0; i < beatPattern.Length-1; i++)
            {
                clickPattern[i] = clickPattern[i + 1];
            }
            clickPattern[beatPattern.Length - 1] = click;

            if (matchBeatPattern())
            {
                changeHeartMeter(HEART_METER_MATCH_VALUE);
            }
        }

        
        // Initialize, Draw and Update
        /// <summary>
        /// 
        /// </summary>

        public void Draw(SpriteBatch spriteBatch, UI.ProgressBar progressBar)
        {

            progressBar.Draw(spriteBatch);
        }

        public virtual void Update(GameTime gameTime, UI.ProgressBar progressBar)
        {

            timer += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (timer >= TIMER)
            {
                timer = 0F;
                changeHeartMeter(HEART_METER_DECREASE_VALUE);
            }

            if (previousMouseState.LeftButton == ButtonState.Released && Mouse.GetState().LeftButton == ButtonState.Pressed)
            {
                addToClickPattern(Click.Left);
            }

            if (previousMouseState.RightButton == ButtonState.Released && Mouse.GetState().RightButton == ButtonState.Pressed)
            {
                addToClickPattern(Click.Right);
            }


            progressBar.value = getHeartMeter();
            progressBar.Update(gameTime);

            //save the current mouse state for the next frame
            // the current 
            previousMouseState = Mouse.GetState();

        }

        public void Initialize()
        {
            previousMouseState = Mouse.GetState();
        }

       
    }
}
