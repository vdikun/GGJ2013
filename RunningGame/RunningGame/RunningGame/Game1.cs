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

namespace RunningGame
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        Texture2D hitTexture;
        Texture2D jumpTexture;
        Texture2D punchTexture;
        Texture2D runTexture;
        Texture2D slideTexture;
        Texture2D standTexture;
        Texture2D currentSprite;
        Texture2D bgTexture;
        Texture2D jumpObstacleTexture;
        Texture2D slideObstacleTexture;
        Texture2D punchObstacleTexture;
        Texture2D currentObstacle;

        Vector2 playerPosition;
        Vector2 bgPosition;
        Vector2 obstaclePosition;

        readonly int RUN_SPEED = 20;

        int jumpTimer;
        int punchTimer;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferHeight = 720;
            graphics.PreferredBackBufferWidth = 1280;

            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            playerPosition = new Vector2(100, 300);
            bgPosition = new Vector2(0, 0);
            obstaclePosition = new Vector2(-500, 0);

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

            hitTexture = Content.Load<Texture2D>("Hit");
            jumpTexture = Content.Load<Texture2D>("Jumping");
            punchTexture = Content.Load<Texture2D>("Punching");
            runTexture = Content.Load<Texture2D>("Running");
            slideTexture = Content.Load<Texture2D>("Sliding");
            standTexture = Content.Load<Texture2D>("Standing");
            bgTexture = Content.Load<Texture2D>("Background");
            jumpObstacleTexture = Content.Load<Texture2D>("JumpObstacle");
            slideObstacleTexture = Content.Load<Texture2D>("SlideObstacle");
            punchObstacleTexture = Content.Load<Texture2D>("punchObstacle");

            // TODO: use this.Content to load your game content here
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

            // TODO: Add your update logic here
            
            currentSprite = runTexture;
            playerPosition.Y = 300;

            KeyboardState keyState = Keyboard.GetState();
            if (keyState.IsKeyDown(Keys.S)) {
                currentSprite = slideTexture;
            }
            if (keyState.IsKeyDown(Keys.D) && punchTimer < -5) {
                punchTimer = 10;
            }
            if ((keyState.IsKeyDown(Keys.Space) || keyState.IsKeyDown(Keys.W)) && jumpTimer < -10)
            {
                jumpTimer = 40;
            }

            if (punchTimer > 0)
            {
                currentSprite = punchTexture;
            }
            if (jumpTimer > 0)
            {
                currentSprite = jumpTexture;
                playerPosition.Y = 150;
            }
            jumpTimer--;
            punchTimer--;

            bgPosition.X-=RUN_SPEED;
            if (bgPosition.X <= -1280) bgPosition.X = 0;

            if (obstaclePosition.X < -400)
            {
                Random random = new Random();
                int randomNumber = random.Next(0, 3);
                if (randomNumber == 0)
                {
                    currentObstacle = jumpObstacleTexture;
                    obstaclePosition = new Vector2(1280, 550);
                }
                if (randomNumber == 1)
                {
                    currentObstacle = slideObstacleTexture;
                    obstaclePosition = new Vector2(1280, -150);
                }
                if (randomNumber == 2)
                {
                    currentObstacle = punchObstacleTexture;
                    obstaclePosition = new Vector2(1280, 50);
                }
            }
            if (obstaclePosition.X < 300 && obstaclePosition.X > -200)
            {
                if (currentObstacle == jumpObstacleTexture && currentSprite != jumpTexture)
                {
                    currentSprite = hitTexture;
                }
                if (currentObstacle == slideObstacleTexture && currentSprite != slideTexture)
                {
                    currentSprite = hitTexture;
                }
                if (currentObstacle == punchObstacleTexture && currentSprite != punchTexture)
                {
                    currentSprite = hitTexture;
                }
            }
            if (obstaclePosition.X < 500)
            {
                if (currentObstacle == punchObstacleTexture && currentSprite == punchTexture)
                {
                    obstaclePosition.X = -500;
                }
            }
            obstaclePosition.X -= RUN_SPEED;
            Console.WriteLine(obstaclePosition.X);
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here

            spriteBatch.Begin();

            spriteBatch.Draw(bgTexture, bgPosition, Color.White);
            spriteBatch.Draw(bgTexture, new Vector2(bgPosition.X+bgTexture.Width, 0), Color.White);
            spriteBatch.Draw(currentObstacle, obstaclePosition, Color.White);
            spriteBatch.Draw(currentSprite, playerPosition, Color.White);

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
