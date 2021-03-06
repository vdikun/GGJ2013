﻿using System;
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

namespace Dozer
{
    enum Click
    {
        Left = 0,
        Right = 1
    }

    enum State
    {
        Default = 0,
        Left = 1,
        Right = 2,
        Dead = 3
    }

    enum Performance
    {
        VerySlow = 0,
        Slow = 1,
        Perfect = 2,
        Fast = 3,
        VeryFast = 4
    }

    class Heart
    {
        public bool beat = false;
        
        private MouseState previousMouseState;
        
        // constants
        private const int BEAT_PATTERN_LENGTH = 2;
        public const int HEART_METER_INIT = 20;
        public const int HEART_METER_UP_BOUND = 50;
        public const int HEART_METER_LOW_BOUND = 0;
        private const int HEART_METER_MATCH_VALUE = 10;
        private const int HEART_METER_DECREASE_VALUE = -5;
        private const float TIMER = 0.6F;
        private const float BEAT_TIMER = 0.5F;
        private const int MATCH_COUNTER = 1;
        private const int TIME_LAG = 1;

        // variables
        public int heartMeter;
        private Click[] beatPattern, clickPattern;
        private int beatPatternLength;
        private float beatTimer;
        private float autoTimer;
        private State heartState;
        public Performance performance;
        public bool justPressed;

        //other variables
        private float oldTime;
        private float time;
        private float[] timeDiff;
        private int matchCounter;

        // heart anmation frames
        private static Texture2D HEART_BASE;
        private static Texture2D HEART_RED;
        private static Texture2D HEART_BLUE;
        private static Texture2D HEART_DEAD;

        private static SoundEffect pump;
        private static SoundEffectInstance pumpEffect;

        public Heart()
        {
            heartMeter = HEART_METER_INIT;    // Initial value of the Heart meter
            beatPatternLength = BEAT_PATTERN_LENGTH;
            beatPattern = new Click[] { Click.Left, Click.Right };
            beatTimer = BEAT_TIMER;
            // assignBeatPattern();    // assign random click pattern + time width
            clickPattern = new Click[beatPatternLength];    // to store clicks by user
            timeDiff = new float[beatPatternLength];    // to store time diff between clicks
            oldTime = 0F;
            heartState = State.Default;
            performance = Performance.Perfect;
            justPressed = false;
        }

        public static void LoadContent(ContentManager contentManager)
        {

            HEART_BASE = contentManager.Load<Texture2D>("Sprites/Heart_Base");
            HEART_RED = contentManager.Load<Texture2D>("Sprites/Heart_Red");
            HEART_BLUE  = contentManager.Load<Texture2D>("Sprites/Heart_Blue");
            HEART_DEAD = contentManager.Load<Texture2D>("Sprites/Heart_Dead");

            pump = contentManager.Load<SoundEffect>("Sounds/Heart_Pump_A1");
        }

        public void hit()
        {
            changeHeartMeter(-10);
        }

        void changeHeartMeter(int change) 
        {
            if ((heartMeter + change) >= HEART_METER_LOW_BOUND && (heartMeter + change) <= HEART_METER_UP_BOUND) 
            { heartMeter = heartMeter + change; }
            else if ((heartMeter + change) < HEART_METER_LOW_BOUND)
            { heartMeter = HEART_METER_LOW_BOUND; }
            else if ((heartMeter + change) > HEART_METER_UP_BOUND)
            { heartMeter = HEART_METER_UP_BOUND; }
            //Console.WriteLine("Heartmeter " + heartMeter);
        }

        void assignBeatPattern()
        {
            // Assign pattern for mouse click
            Random random = new Random();
            for (int i = 0; i < beatPattern.Length; i++)
            {
                int rnd = random.Next(0, 2);
                beatPattern[i] = (Click) rnd;
                //Console.WriteLine(rnd);
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
            if (avgTime < (beatTimer - 0.1F))
            {
                Console.WriteLine("Take it slow!");
                performance = Performance.Fast;
                return false;
            }
            else if (avgTime < (beatTimer - 0.2F))
            {
                Console.WriteLine("Way too fast....");
                performance = Performance.VeryFast;
                return false;

            }
            else if (avgTime > (beatTimer + 0.3F))
            {
                Console.WriteLine(".......");
                performance = Performance.VerySlow;
                return false;

            }
            else if (avgTime > (beatTimer + 0.15F))
            {
                Console.WriteLine("Come on Grandma....");
                performance = Performance.Slow;
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
            Console.WriteLine("Excellent!");
            performance = Performance.Perfect;
            return true;             // user gets points
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

        public void Draw(SpriteBatch spriteBatch, int xPos, int yPos)
        {
            Texture2D image;
            if (heartState == State.Default)
            {
                image = HEART_BASE;
            }
            else if (heartState == State.Left)
            {
                image = HEART_BLUE;
            }
            else if (heartState == State.Dead)
            {
                image = HEART_DEAD;
            }
            else
            {
                image = HEART_RED;
            }
                spriteBatch.Draw(image, new Vector2(xPos, yPos), Color.White);
        }

        public virtual void Update(GameTime gameTime)
        {
            beat = false;
            // Automatic decrease of HeartMeter with time
            if (heartMeter > 0)
            {
                autoTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (autoTimer >= TIMER)
                {
                    autoTimer = 0F;
                    changeHeartMeter(HEART_METER_DECREASE_VALUE);
                }

                // Mouse Click Input
                // click patterns
                if (previousMouseState.LeftButton == ButtonState.Released && Mouse.GetState().LeftButton == ButtonState.Pressed)
                {
                    pumpEffect = pump.CreateInstance();
                    pumpEffect.Play();
                    beat = true;
                    time = (float)gameTime.TotalGameTime.TotalSeconds - oldTime;
                    oldTime = (float)gameTime.TotalGameTime.TotalSeconds;
                    addToClickPattern(Click.Left, time);
                    justPressed = true;
                }

                else if (previousMouseState.RightButton == ButtonState.Released && Mouse.GetState().RightButton == ButtonState.Pressed)
                {
                    pumpEffect = pump.CreateInstance();
                    pumpEffect.Pitch = 0.2f;
                    pumpEffect.Play();
                    beat = true;
                    time = (float)gameTime.TotalGameTime.TotalSeconds - oldTime;
                    oldTime = (float)gameTime.TotalGameTime.TotalSeconds;
                    addToClickPattern(Click.Right, time);
                    justPressed = true;
                }

                else
                {
                    // check if heart has just been pressed
                    float currentTime = (float)gameTime.TotalGameTime.TotalSeconds;
                    if ((currentTime - oldTime) < 1)
                    {
                        justPressed = true;
                    }
                    else
                    {
                        justPressed = false;
                    }
                }
            }

            // heart state
            if (heartState != State.Dead)
            {
                if (heartMeter <= 0)
                {
                    heartState = State.Dead;
                }
                else
                {
                    if (Mouse.GetState().LeftButton == ButtonState.Pressed)
                    {
                        heartState = State.Left;
                    }
                    else if (Mouse.GetState().RightButton == ButtonState.Pressed)
                    {
                        heartState = State.Right;
                    }
                    else
                    {
                        heartState = State.Default;
                    }
                }
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
