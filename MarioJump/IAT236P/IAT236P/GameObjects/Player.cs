using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using IAT236P.Services;
using IAT236P.Screens;

namespace IAT236P.GameObjects
{
    [TextureAttribute("th_Mario")]
    public class Player : GameObject
    {
        private GameObjectState mState;
        private const int MAX_SPEED = 15;
        private const int JUMP_HEIGHT = 100;

        public Player() : base() 
        {
            ContentManager contentManager = ServiceLocator.GetService<ContentManager>();
            Texture = contentManager.Load<Texture2D>(TextureAttribute.GetCustomAttributes(this.GetType()).Where(at => at.GetType() == typeof(TextureAttribute)).Select(at => at as TextureAttribute).FirstOrDefault().TextureName);
            Position = new Vector2(GameScreenStaticProperties.mStartingPosition.X, GameScreenStaticProperties.mStartingPosition.Y);
            Velocity = new Vector2(0, 0);
            Acceleration = new Vector2(0, 0);
            this.State = GameObjectState.Idle;
        }
        
        public GameObjectState State
        {
            get { return this.mState; }
            set { this.mState = value; }
        }

        public override void Update(GameTime gameTime)
        {
            UpdateMotion(gameTime);

            base.Update(gameTime);
        }

        private void UpdateMotion(GameTime gameTime)
        {
            GamePadState padState = GamePad.GetState(PlayerIndex.One);
            KeyboardState keyState = Keyboard.GetState();

            Acceleration = new Vector2(0f, 0f);
            if (IsGrounded)
            {
                if (padState.DPad.Right == ButtonState.Pressed || keyState.IsKeyDown(Keys.Right)) { Acceleration = new Vector2(1, 0); }
                if (padState.DPad.Left == ButtonState.Pressed || keyState.IsKeyDown(Keys.Left)) { Acceleration = new Vector2(-1, 0); }
            }
            else
            {
                if (padState.DPad.Right == ButtonState.Pressed || keyState.IsKeyDown(Keys.Right)) { Acceleration = new Vector2(0.5f, 0); }
                if (padState.DPad.Left == ButtonState.Pressed || keyState.IsKeyDown(Keys.Left)) { Acceleration = new Vector2(-0.5f, 0); }
            }
            if (padState.DPad.Right == ButtonState.Released && padState.DPad.Left == ButtonState.Released && keyState.IsKeyUp(Keys.Right) && keyState.IsKeyUp(Keys.Left) && IsGrounded) { Velocity = new Vector2(Velocity.X * 0.8f, Velocity.Y); }

            if ((padState.Buttons.A == ButtonState.Pressed || keyState.IsKeyDown(Keys.Space)) && IsGrounded) { Acceleration = new Vector2(Acceleration.X, -20f); }

            if (!IsGrounded) { Acceleration = new Vector2(Acceleration.X, 0.75f); }
         
            Velocity += Acceleration;
            if (Math.Abs(Velocity.X) > MAX_SPEED) Velocity = new Vector2(MAX_SPEED*(Math.Abs(Velocity.X)/Velocity.X), Velocity.Y);
            if (Math.Abs(Velocity.X) < 0.05) Velocity = new Vector2(0f, Velocity.Y);
            Position += Velocity;
            GroundCheck();
        }

        private void GroundCheck()
        {
            if (Position.Y >= 720-Texture.Height)
            {
                Position = new Vector2(Position.X, 720 - Texture.Height);
                Velocity = new Vector2(Velocity.X, 0);
                IsGrounded = true;
            }
            else
            {
                IsGrounded = false;
            }
        }

    }
}
