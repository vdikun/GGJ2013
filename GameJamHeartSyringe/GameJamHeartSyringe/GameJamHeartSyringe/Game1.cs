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

namespace GameJamHeartSyringe
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        const int SCREEN_HEIGHT = 900;
        const int SCREEN_WIDTH = 1600;

        Color foreColor = new Color(166, 206, 57);
        Color backColor = new Color(98, 127, 21);

        Texture2D syringeTexture;
        Texture2D heartTexture;

        Heart heart;
        Syringe syringe;

        KeyboardState keyboardState;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            graphics.PreferredBackBufferWidth = SCREEN_WIDTH;
            graphics.PreferredBackBufferHeight = SCREEN_HEIGHT;
            graphics.IsFullScreen = false;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            //this.IsMouseVisible = true;

            heart = new Heart();

            syringe = new Syringe();

            keyboardState = Keyboard.GetState();
            

            base.Initialize();

        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            syringeTexture = Content.Load<Texture2D>(@"Textures\syringe");
            heartTexture = Content.Load<Texture2D>(@"Textures\heart");
            
            heart.Sprite = heartTexture;
            heart.Position = new Vector2(100, 500);
            syringe.Sprite = syringeTexture;
            syringe.Position = new Vector2(SCREEN_WIDTH / 2 - syringe.Sprite.Width / 2, 100);
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                this.Exit();

            //MouseState mouseState = Mouse.GetState();

            //mousePosition = new Vector2(mouseState.X, mouseState.Y);

            
            syringe.Update(SCREEN_WIDTH, SCREEN_HEIGHT, Keyboard.GetState());
            heart.Update(SCREEN_WIDTH, SCREEN_HEIGHT, syringe);
            

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            MouseState mouseState = Mouse.GetState();

            spriteBatch.Begin();
            heart.Draw(spriteBatch);
            syringe.Draw(spriteBatch);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
