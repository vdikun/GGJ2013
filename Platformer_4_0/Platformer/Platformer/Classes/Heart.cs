using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

// health of heart stored in (int) heartMeter , range 0 - 100
// pattern in (int[]) beatPattern , values from {0, 1} 0: left click, 1: right click
// time for each click is 0.5s +- 0.1s

namespace Platformer
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
        private const int BEAT_PATTERN_LENGTH = 2;
        public const int HEART_METER_UP_BOUND = 100;
        public const int HEART_METER_LOW_BOUND = 0;
        private const int HEART_METER_MATCH_VALUE = 10;
        private const int HEART_METER_DECREASE_VALUE = -1;
        private const float TIMER = 0.6F;
        private const float BEAT_TIMER = 0.5F;
        private const int MATCH_COUNTER = 2;

        // variables
        private int heartMeter;
        private Click[] beatPattern, clickPattern;
        private int beatPatternLength;
        private float beatTimer;
        private float autoTimer;
        

        //other variables
        private float oldTime;
        private float time;
        private float[] timeDiff;
        private int matchCounter;

        public Heart()
        {
            heartMeter = 80;    // Initial value of the Heart meter
            beatPatternLength = BEAT_PATTERN_LENGTH;
            beatPattern = new Click[] {Click.Left, Click.Right};
            beatTimer = BEAT_TIMER;
            // assignBeatPattern();    // assign random click pattern + time width
            clickPattern = new Click[beatPatternLength];    // to store clicks by user
            timeDiff = new float[beatPatternLength];    // to store time diff between clicks
            oldTime = 0F;
        }

        public int getHeartMeter() { return heartMeter; }

        void changeHeartMeter(int change) 
        {
            if ((heartMeter + change) >= HEART_METER_LOW_BOUND && (heartMeter + change) <= HEART_METER_UP_BOUND) 
            { heartMeter = heartMeter + change; }
            else if ((heartMeter + change) < HEART_METER_LOW_BOUND)
            { heartMeter = HEART_METER_LOW_BOUND; }
            else if ((heartMeter + change) > HEART_METER_UP_BOUND)
            { heartMeter = HEART_METER_UP_BOUND; }
            Console.WriteLine("Heartmeter " + getHeartMeter());
        }

        void assignBeatPattern()
        {
            // Assign pattern for mouse click
            Random random = new Random();
            for (int i = 0; i < beatPattern.Length; i++)
            {
                int rnd = random.Next(0, 2);
                beatPattern[i] = (Click) rnd;
                Console.WriteLine(rnd);
            }

            // Assign time length for each beat
            beatTimer = BEAT_TIMER;

            // Display to console the pattern (beat + time)

        }

        bool matchBeatPattern()
        {
            float avgTime = 0;
            for (int i = 0; i < beatPatternLength; i++)
            {
                avgTime += timeDiff[i];                
            }
            avgTime /= beatPatternLength;

            // check if click pattern wasnt too slow/ too fast/ incorrect number/ wrong pattern
            if (avgTime < (beatTimer - 0.15F))
            {
                Console.WriteLine("Take it slow!");
                return false;
            }
            else if (avgTime > (beatTimer + 0.15F))
            {
                Console.WriteLine("Come on Grandma....");
                return false;

            }
            else if (clickPattern.Length != beatPattern.Length)
                return false;
            else
                for (int i = 0; i < clickPattern.Length; i++)
                {
                    if (clickPattern[i] != beatPattern[i])
                        return false;
                }
            return true;
        }


        void addToClickPattern(Click click, float time)
        {
            // store which click
            for (int i = 0; i < beatPattern.Length - 1; i++)
            {
                clickPattern[i] = clickPattern[i + 1];
            }
            clickPattern[beatPattern.Length - 1] = click;

            // store time since last click
            for (int i = 0; i < beatPattern.Length - 1; i++)
            {
                timeDiff[i] = timeDiff[i + 1];
            }
            timeDiff[beatPattern.Length - 1] = time;

   
            if (matchBeatPattern())
            {
                matchCounter++;
                Console.WriteLine("yes");
                if (matchCounter == MATCH_COUNTER)
                {
                    matchCounter = 0;
                    changeHeartMeter(HEART_METER_MATCH_VALUE);
                }
            }
        }

        
        // Initialize, Draw and Update
        /// <summary>
        /// 
        /// </summary>

        public void Draw(SpriteBatch spriteBatch)
        {
            
        }

        public virtual void Update(GameTime gameTime)
        {
            // Automatic decrease of HeartMeter with time
            autoTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (autoTimer >= TIMER)
            {
                autoTimer = 0F;
                changeHeartMeter(HEART_METER_DECREASE_VALUE);
            }

            // Mouse Click Input
            if (previousMouseState.LeftButton == ButtonState.Released && Mouse.GetState().LeftButton == ButtonState.Pressed)
            {
     
                time = (float)gameTime.TotalGameTime.TotalSeconds - oldTime;
                oldTime = (float)gameTime.TotalGameTime.TotalSeconds;
                addToClickPattern(Click.Left, time);
            }

            else if (previousMouseState.RightButton == ButtonState.Released && Mouse.GetState().RightButton == ButtonState.Pressed)
            {
                time = (float)gameTime.TotalGameTime.TotalSeconds - oldTime;
                oldTime = (float)gameTime.TotalGameTime.TotalSeconds;
                addToClickPattern(Click.Right, time);
            }

            // Update Progress bar with HeartMeter value
            

            //save the current mouse state for the next frame
            previousMouseState = Mouse.GetState();

        }

        public void Initialize()
        {
            previousMouseState = Mouse.GetState();
        }

       
    }
}
