using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using IAT236P.Managers;
using IAT236P.GameObjects;
using IAT236P.Services;
using IAT236P.Screens;

namespace IAT236P
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        private ScreenManager mScreenManager;
        private SoundManager mSoundManager;

        public static readonly int SCREENWIDTH = 1280;
        public static readonly int SCREENHEIGHT = 720;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferHeight = SCREENHEIGHT;
            graphics.PreferredBackBufferWidth = SCREENWIDTH;

            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            base.Initialize();
        }

        
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            ServiceLocator.AddService<ContentManager>(this.Content);
            ServiceLocator.AddService<SpriteBatch>(spriteBatch);
            ServiceLocator.AddService<GraphicsDevice>(GraphicsDevice);
            mScreenManager = new ScreenManager();
            MenuScreen menuScreen = (MenuScreen)mScreenManager.GetScreen(typeof(MenuScreen));
            menuScreen.ExitGameChanged += ExitGame;
        }

        protected override void UnloadContent()
        {
            
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            mScreenManager.Update(gameTime);

            base.Update(gameTime);
        }

      
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            mScreenManager.Draw(gameTime);

            base.Draw(gameTime);
        }

        public void ExitGame(object sender, EventArgs e)
        {
            Exit();
        }
    }
}
