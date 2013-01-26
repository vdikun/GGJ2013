using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace GameJamHeart
{
    class Player : GameItem
    {
        private Texture2D smackSprite;
        public Texture2D SmackSprite
        {
            get { return smackSprite; }
            set { smackSprite = value; }
        }

        private Texture2D passiveSprite;
        public Texture2D PassiveSprite
        {
            get { return passiveSprite; }
            set { passiveSprite = value; }
        }

        private Texture2D followSprite;
        public Texture2D FollowSprite
        {
            get { return followSprite; }
            set { followSprite = value; }
        }

        private float maxVelocity;
        public float MaxVelocity
        {
            get { return maxVelocity; }
            set { maxVelocity = value; }
        }

        private float velocity;
        public float Velocity
        {
            get { return velocity; }
            set { velocity = value; }
        }

        private int bumperSize;
        public int BumperSize
        {
            get { return bumperSize; }
            set { bumperSize = value; }
        }

        private Vector2 defaultPosition;
        public Vector2 DefaultPosition
        {
            get { return defaultPosition; }
            set { defaultPosition = value; }
        }

        private float bumperStrength;
        public float BumperStrength
        {
            get { return bumperStrength; }
            set { bumperStrength = value; }
        }

        private Vector2 followPosition;
        public Vector2 FollowPosition
        {
            get { return followPosition; }
            set { followPosition = value; }
        }

        private int playerSize;
        public int PlayerSize
        {
            get { return playerSize; }
            set { playerSize = value; }
        }

        int GhostAlpha = 150;

        private Color ghostColor;
        public Color GhostColor
        {
            get { return ghostColor; }
            set { ghostColor = value; }
        }

        protected KeyboardState lastKeyboardState;
        protected GamePadState lastGamepadState;

        protected const int GHOST_ARM_OFFSET = 12;

        private Keys up;
        public Keys Up
        {
            get { return up; }
            set { up = value; }
        }

        private Keys down;
        public Keys Down
        {
            get { return down; }
            set { down = value; }
        }

        private Keys smack;
        public Keys Smack
        {
            get { return smack; }
            set { smack = value; }
        }

        private Keys special;
        public Keys Special
        {
            get { return special; }
            set { special = value; }
        }

        private Buttons gamepadSmack;
        public Buttons GamepadSmack
        {
            get { return gamepadSmack; }
            set { gamepadSmack = value; }
        }

        private Buttons gamepadSpecial;
        public Buttons GamepadSpecial
        {
            get { return gamepadSpecial; }
            set { gamepadSpecial = value; }
        }

        protected PlayerIndex playerIndex;

        private bool frozen = false;

        private double freezeStartTime;

        private float meltTime;
        public float MeltTime
        {
            get { return meltTime; }
            set { meltTime = value; }
        }

        private Color defaultGhostColor;
        public Color DefaultGhostColor
        {
            get { return defaultGhostColor; }
            set { defaultGhostColor = value; }
        }

        private Color frozenGhostColor;
        public Color FrozenGhostColor
        {
            get { return frozenGhostColor; }
            set { frozenGhostColor = value; }
        }

        private float defaultMaxVelocity;
        public float DefaultMaxVelocity
        {
            get { return defaultMaxVelocity; }
            set { defaultMaxVelocity = value; }
        }





        public Player(Keys up, Keys down, Keys smack, Keys special, Buttons GamepadSmack, Buttons GamepadSpecial, PlayerIndex playerIndex)
        {
            velocity = 0;
            maxVelocity = 0;
            bumperStrength = 2;
            defaultGhostColor = new Color(255, 255, 255, GhostAlpha);
            ghostColor = defaultGhostColor;
            frozenGhostColor = Color.LimeGreen;
            meltTime = 1.5f;

            this.up = up;
            this.down = down;
            this.smack = smack;
            this.special = special;
            this.gamepadSmack = GamepadSmack;
            this.gamepadSpecial = GamepadSpecial;

            this.playerIndex = playerIndex;
        }

        protected void LoadPassiveSprite()
        {
            followSprite = passiveSprite;
        }

        protected void LoadSmackSprite()
        {
            followSprite = smackSprite;
        }

        protected void ResetBumper()
        {
            Position = new Vector2(DefaultPosition.X, Position.Y);
        }

        public virtual void Update(int screenWidth, int screenHeight, KeyboardState keyboardState, GamePadState gamepadState, GameTime gameTime)
        {

            if (!frozen)
            {
                if ((Keyboard.GetState().IsKeyDown(Up) && Keyboard.GetState().IsKeyDown(Down)) || (gamepadState.ThumbSticks.Left.Y > 0 && gamepadState.ThumbSticks.Left.Y < 0))
                    SlowDown();
                else if (Keyboard.GetState().IsKeyDown(Up))
                    Velocity = -MaxVelocity;
                else if (gamepadState.ThumbSticks.Left.Y > 0)
                    Velocity = -MaxVelocity * (.25f + gamepadState.ThumbSticks.Left.Y * (3f / 4));
                else if (Keyboard.GetState().IsKeyDown(Down))
                    Velocity = MaxVelocity;
                else if (gamepadState.ThumbSticks.Left.Y < 0)
                    Velocity = MaxVelocity * (.25f + -gamepadState.ThumbSticks.Left.Y * (3f / 4));
                else
                    SlowDown();

                Position = new Vector2(Position.X, Position.Y + Velocity);

                CheckWallCollision(screenHeight);

                if ((Keyboard.GetState().IsKeyDown(Smack) || gamepadState.IsButtonDown(GamepadSmack)) && (lastKeyboardState.IsKeyUp(Smack) && lastGamepadState.IsButtonUp(GamepadSmack)))
                {
                    FireBumper();
                    LoadSmackSprite();
                    AdjustFollowPosition();
                }
                else if (Keyboard.GetState().IsKeyUp(Smack) && gamepadState.IsButtonUp(GamepadSmack))
                {
                    ResetBumper();
                    LoadPassiveSprite();
                    AdjustFollowPosition();
                }
                else
                {
                    AdjustFollowPosition();
                }
            }


            lastKeyboardState = Keyboard.GetState();
            lastGamepadState = GamePad.GetState(playerIndex);

        }

        public virtual bool FireSpecial(GamePadState lastPadState)
        {
            /*
            GamePadState lastPadState;
            if(playerIndex.Equals(PlayerIndex.One));
                lastPadState = GM.LastP1GamePadState;
            if(playerIndex.Equals(PlayerIndex.Two));
                lastPadState = GM.LastP2GamePadState;
            */

            if (!frozen && (Keyboard.GetState().IsKeyDown(special) && GM.LastKeyboardState.IsKeyUp(special)) || (GamePad.GetState(playerIndex).IsButtonDown(gamepadSpecial) && lastPadState.IsButtonUp(gamepadSpecial)))
                return true;
            else
                return false;
        }

        public virtual void SpecialOffset()
        {
            FireBumper();
            AdjustFollowPosition();
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Sprite, Position, SpriteColor);
            spriteBatch.Draw(followSprite, FollowPosition, GhostColor);
        }

        protected virtual void FireBumper()
        {

        }

        public virtual void AdjustFollowPosition()
        {

        }

        protected virtual void CheckWallCollision(int screenHeight)
        {
            if (Position.Y <= 0)
            {
                Position = new Vector2(Position.X, 0);
                Velocity = 0;
            }

            if (Position.Y + Sprite.Height >= screenHeight)
            {
                Position = new Vector2(Position.X, screenHeight - Sprite.Height);
                Velocity = 0;
            }

        }

        protected void SlowDown()
        {
            Velocity *= .9f;
        }

        public virtual Rectangle GetHitBox()
        {
            return new Rectangle(0, 0, 0, 0);
        }



    }
}
