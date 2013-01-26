using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace IAT236P.GameObjects
{
    public class GameObject
    {
        private Vector2 mPosition;
        private Vector2 mVelocity;
        private Vector2 mAcceleration;
        private int mGravityForce;
        private bool mIsGrounded;
        private bool mIsDrawable;
        private bool mHasHealth;
        private int mHealth;
        private int mFrameIndex;
        private List<Rectangle> mHitBoxes;
        private GameTime mGameTime;

        private event EventHandler<EventArgs> DrawableStateChanged;
        private event EventHandler<EventArgs> HealthStateChanged;

        private Texture2D mTexture;

        public GameObject()
        {
        }

        public Vector2 Position { get { return this.mPosition; } set { this.mPosition = value; } }
        public Vector2 Velocity { get { return this.mVelocity; } set { this.mVelocity = value; } }
        public Vector2 Acceleration { get { return this.mAcceleration; } set { this.mAcceleration = value; } }
        public int GravityForce { get { return this.mGravityForce; } set { this.mGravityForce = value; } }
        public bool IsGrounded { get { return this.mIsGrounded; } set { this.mIsGrounded = value; } }
        public bool IsDrawable { get { return this.mIsDrawable; } set { this.mIsDrawable = value; } }
        public bool HasHealth { get { return this.mHasHealth; } set { this.mHasHealth = value; } }
        public int Health { get { return this.mHealth; } set { this.mHealth = value; } }
        public int FrameIndex { get { return this.mFrameIndex; } set { this.mFrameIndex = value; } }
        public List<Rectangle> HitBoxes { get { return this.mHitBoxes; } set { this.mHitBoxes = value; } }
        public Texture2D Texture { get { return this.mTexture; } set { this.mTexture = value; } }
        //public GameTime GameTime { get { return this.mGameTime; } set { this.mGameTime = value; } }

       public virtual void Update(GameTime gameTime)
       {
        
       }

    }

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, AllowMultiple = true)]
    public class TextureAttribute : Attribute
    {
        private string mTextureName;

        public TextureAttribute(string textureName)
        {
            this.mTextureName = textureName;
        }

        public string TextureName
        {
            get { return this.mTextureName; }   
        }
    }


    public enum GameObjectState
    {
        Walking,
        Running,
        Jumping,
        Idle
    }

}
