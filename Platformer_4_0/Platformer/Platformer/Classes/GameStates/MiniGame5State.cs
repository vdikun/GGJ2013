﻿using System;
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


namespace Platformer
{
    class MiniGame5State : GameState
    {
        static Texture2D heartTexture;
        static Texture2D patientTexture;

        Vector2 heartPosition = new Vector2(240, 240);
        Vector2 patientPosition = new Vector2(0, 0);
        Vector2 patientTargetPosition = new Vector2(0, 0);
        Vector2 woundPosition;

        MouseState currentMouseState, previousMouseState;
        bool operationSuccessful;

        // constants
        private const int PATIENT_SPEED = 10;
        private const int POSITION_MATCH_ERROR = 80;

        public MiniGame5State()
        {
            currentMouseState = new MouseState();
            operationSuccessful = false;
        }

        public static void LoadContent(ContentManager manager)
        {
            patientTexture = manager.Load<Texture2D>("Minigames/OpenWoundGuy");
            heartTexture = manager.Load<Texture2D>("Minigames/heart");

        }

        void GameState.Update(PlatformerGame game, GameTime gameTime)
        {
            if (game.keyboard.IsKeyDown(Keys.Escape))
            {
                game.currentState = new MenuState();
            }

            // Update heart position according to mouse position
            currentMouseState = Mouse.GetState();
            float posDiff = currentMouseState.X - heartPosition.X;
            heartPosition.X += posDiff * 0.1f;
            if (heartPosition.X < 0) heartPosition.X = 0;
            if (heartPosition.X > 1280 - heartTexture.Width) heartPosition.X = 1280 - heartTexture.Width;
            posDiff = currentMouseState.Y - heartPosition.Y;
            heartPosition.Y += posDiff * 0.1f;
            if (heartPosition.Y < 0) heartPosition.Y = 0;
            if (heartPosition.Y > 720 - heartTexture.Height) heartPosition.Y = 720 - heartTexture.Height;

            // Randomize movement of patient
            Random random = new Random();
            if (patientPosition.X < (patientTargetPosition.X + PATIENT_SPEED) && patientPosition.X > (patientTargetPosition.X - PATIENT_SPEED)) 
                patientTargetPosition.X = random.Next(0, (1280 - patientTexture.Width));
            if (patientPosition.Y < (patientTargetPosition.Y + PATIENT_SPEED) && patientPosition.Y > (patientTargetPosition.Y - PATIENT_SPEED))
                patientTargetPosition.Y = random.Next(-200, (720 - patientTexture.Width));
            if (patientPosition.X < patientTargetPosition.X) patientPosition.X += PATIENT_SPEED;
            if (patientPosition.X > patientTargetPosition.X) patientPosition.X -= PATIENT_SPEED;
            if (patientPosition.Y < patientTargetPosition.Y) patientPosition.Y += PATIENT_SPEED;
            if (patientPosition.Y > patientTargetPosition.Y) patientPosition.Y -= PATIENT_SPEED;


            // check if heart is clicked around patient
            woundPosition.X = patientPosition.X + 300;
            woundPosition.Y = patientPosition.Y + 320;

            if (previousMouseState.LeftButton == ButtonState.Released && Mouse.GetState().LeftButton == ButtonState.Pressed)
            {

                if ((Math.Abs(woundPosition.X - heartPosition.X) < POSITION_MATCH_ERROR) && (Math.Abs(woundPosition.Y - heartPosition.Y) < POSITION_MATCH_ERROR))
                    operationSuccessful = true;
            }

            previousMouseState = Mouse.GetState();
        }

        void GameState.Draw(PlatformerGame game, SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(game.font, "Mini Game 5 State", new Vector2(10, 10), Color.White);

            if (operationSuccessful)
            {
                spriteBatch.DrawString(game.font, "GOOD!", new Vector2(450, 100), Color.White, 0f, Vector2.Zero, 5f, SpriteEffects.None, 0f);
            }
            else
            {
                spriteBatch.Draw(patientTexture, patientPosition, Color.White);
                spriteBatch.Draw(heartTexture, heartPosition, Color.White);
            }
        }
    }
}
