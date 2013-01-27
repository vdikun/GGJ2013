#region File Description
//-----------------------------------------------------------------------------
// PlatformerGame.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------
#endregion

using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Platformer
{
    public class PlatformerGame : Microsoft.Xna.Framework.Game
    {
        public readonly static float SCREEN_WIDTH;
        public readonly static float SCREEN_HEIGHT = 720.0f;

        public static SoundEffect music;
        public static SoundEffectInstance musicIntro;
        public static SoundEffectInstance musicLoop;

        public GameState currentState;

        // Resources for drawing.
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;

        // Global content.
        public SpriteFont font;
        public SpriteFont headerFont;

        private Texture2D placeholder;
        public Texture2D placeholderTexture { get { return placeholder; } }
        private Texture2D winOverlay;
        private Texture2D loseOverlay;
        private Texture2D diedOverlay;

        // Meta-level game state.
        private int levelIndex = -1;
        private Level level;
        private bool wasContinuePressed;

        // When the time remaining is less than the warning time, it blinks on the hud
        private static readonly TimeSpan WarningTime = TimeSpan.FromSeconds(30);

        // We store our input states so that we only poll once per frame, 
        // then we use the same input state wherever needed
        private GamePadState gamePadState;
        public GamePadState gamePad { get { return gamePadState; } }
        private GamePadState prevGamePadState;
        public GamePadState prevGamePad { get { return prevGamePadState; } }
        private KeyboardState keyboardState;
        public KeyboardState keyboard { get { return keyboardState; } }
        private KeyboardState prevKeyboardState;
        public KeyboardState prevKeyboard { get { return prevKeyboardState; } }
        private MouseState mouseState;
        public MouseState mouse { get { return mouseState; } }
        private MouseState prevMouseState;
        public MouseState prevMouse { get { return prevMouseState; } }

        static PlatformerGame()
        {
            SCREEN_WIDTH = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
            SCREEN_HEIGHT = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
        }

        public PlatformerGame()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferHeight = 720;
            graphics.PreferredBackBufferWidth = 1280;

            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // Load fonts
            font = Content.Load<SpriteFont>("Fonts/Hud");
            headerFont = Content.Load<SpriteFont>("Fonts/Hud");

            // Load textures
            placeholder = Content.Load<Texture2D>("Sprites/whitepixel");

            FreePlatformState.LoadContent(Content);
            PlatformState.LoadContent(Content);
            MenuState.LoadContent(Content);
            MiniGame1State.LoadContent(Content);
            MiniGame2State.LoadContent(Content);
            MiniGame3State.LoadContent(Content);
            MiniGame4State.LoadContent(Content);
            MiniGame5State.LoadContent(Content);
            Heart.LoadContent(Content);

            currentState = new MenuState();
        }

        protected override void Update(GameTime gameTime)
        {
            HandleInput();
            currentState.Update(this, gameTime);

            if (musicLoop == null && music != null)
            {
                musicLoop = music.CreateInstance();
                musicLoop.Volume = Util.MUSIC_VOLUME;
                musicLoop.IsLooped = true;
            }

            if (musicLoop != null && musicLoop.State == SoundState.Stopped && (musicIntro == null || musicIntro.State == SoundState.Stopped))
                musicLoop.Play();
        }

        private void HandleInput()
        {
            // get all of our input states
            prevKeyboardState = keyboardState;
            keyboardState = Keyboard.GetState();
            prevMouseState = mouseState;
            mouseState = Mouse.GetState();
            prevGamePadState = gamePadState;
            gamePadState = GamePad.GetState(PlayerIndex.One);

            // Exit the game when back is pressed.
            if (gamePadState.Buttons.Back == ButtonState.Pressed)
                Exit();
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(new Color(226, 221, 217));
            spriteBatch.Begin();
            currentState.Draw(this, spriteBatch);
            spriteBatch.End();
        }
    }
}

