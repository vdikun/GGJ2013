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

        SoundEffect music;
        SoundEffectInstance musicLoop;

        public GameState currentState = new MenuState();

        // Resources for drawing.
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;

        // Global content.
        public SpriteFont font;

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

            // Load textures
            placeholder = Content.Load<Texture2D>("Sprites/whitepixel");

            FreePlatformState.LoadContent(Content);
            PlatformState.LoadContent(Content);
            MenuState.LoadContent(Content);
            MiniGame1State.LoadContent(Content);
            MiniGame2State.LoadContent(Content);
            MiniGame3State.LoadContent(Content);

            music = Content.Load<SoundEffect>("Sounds/DrDozer_7");

            /*winOverlay = Content.Load<Texture2D>("Overlays/you_win");
            loseOverlay = Content.Load<Texture2D>("Overlays/you_lose");
            diedOverlay = Content.Load<Texture2D>("Overlays/you_died");*/

            //Known issue that you get exceptions if you use Media PLayer while connected to your PC
            //See http://social.msdn.microsoft.com/Forums/en/windowsphone7series/thread/c8a243d2-d360-46b1-96bd-62b1ef268c66
            //Which means its impossible to test this from VS.
            //So we have to catch the exception and throw it away
            /*try
            {
                MediaPlayer.IsRepeating = true;
                MediaPlayer.Play(Content.Load<Song>("Sounds/Music"));
            }
            catch { }*/

            //LoadNextLevel();
        }

        protected override void Update(GameTime gameTime)
        {
            HandleInput();
            currentState.Update(this, gameTime);

            if (musicLoop == null) musicLoop = music.CreateInstance();
            if (musicLoop.State == SoundState.Stopped) musicLoop.Play();
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
            spriteBatch.Begin();
            currentState.Draw(this, spriteBatch);
            spriteBatch.End();
        }
    }
}
