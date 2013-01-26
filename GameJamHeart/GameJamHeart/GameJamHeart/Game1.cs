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

/*
 * 
 * 
 * */


namespace GameJamHeart
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        const Keys P_UP = Keys.W;
        const Keys P_DOWN = Keys.S;
        const Keys P_RIGHT = Keys.D;
        const Keys P_LEFT = Keys.A;

        //const Buttons GAMEPAD_BUTTON1 = Buttons.A;
        //const Buttons GAMEPAD_BUTTON2 = Buttons.X;

        Song BGMusic;

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        SpriteFont font;
        SpriteFont nameFont;
        SpriteFont menuTitleFont;
        SpriteFont winnerScreenFont;
        SpriteFont smallFont;

        Player player;

        Background background;


        Color fontColor = Color.White;
        Color menuFontColor = Color.White;
        Color menuSelectColor = Color.Yellow;
        Color menuTitleColor = Color.LightGoldenrodYellow;

        int SCREEN_WIDTH;
        int SCREEN_HEIGHT;

        Keys[] lastKeys;
        KeyboardState keyboardState;
        //GamePadState gamepadState1;
        //GamePadState gamepadState2;

        Color screenColor = Color.Black;
        Color paddleColor = Color.White;
        Color ballColor = Color.White;

        Random rand = new Random();

        GameItem title;

        TransitionScreen transScreen;
        Texture2D whitepixel;
        Texture2D helpMenu;

        Vector2 baseScreenSize = new Vector2(1600, 900);

        bool STRETCH = false;

        Matrix globalTransformation;


        Color menuColor = Color.Black;
        SpriteFont menuFont;

        float MusicVolumeDefault = .3f;
        float MusicVolumeQuiet = .05f;


        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
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
            SCREEN_WIDTH = (int)baseScreenSize.X;
            SCREEN_HEIGHT = (int)baseScreenSize.Y;

       
            background = new Background();
            title = new GameItem();
            transScreen = new TransitionScreen(SCREEN_WIDTH, SCREEN_HEIGHT);


            GM.GameIsRunning = false;
            GM.ShowTitleScreen = true;
            lastKeys = Keyboard.GetState().GetPressedKeys();
            keyboardState = Keyboard.GetState();
            //gamepadState1 = GamePad.GetState(PlayerIndex.One);
            //gamepadState2 = GamePad.GetState(PlayerIndex.Two);
            GM.SetLastGamePadStates();

            SoundHelper.Initialize();

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            globalTransformation = Resolution.GetTransformMatrix(graphics, baseScreenSize, STRETCH);
            //graphics.ToggleFullScreen();

            spriteBatch = new SpriteBatch(GraphicsDevice);  
            /*
            font = Content.Load<SpriteFont>(@"Fonts\Score");
            nameFont = Content.Load<SpriteFont>(@"Fonts\TitleCredits");
            menuFont = Content.Load<SpriteFont>(@"Fonts\Menu");
            menuTitleFont = Content.Load<SpriteFont>(@"Fonts\MenuTitle");
            winnerScreenFont = Content.Load<SpriteFont>(@"Fonts\WinnerScreen");
            smallFont = Content.Load<SpriteFont>(@"Fonts\Small");
             * 
             * */


            /*
            //ball.Sprite = Content.Load<Texture2D>(@"Textures\mario1");

            ball.MarioList.Add(Content.Load<Texture2D>(@"Textures\mario1"));
            ball.MarioList.Add(Content.Load<Texture2D>(@"Textures\mario2"));
            ball.MarioList.Add(Content.Load<Texture2D>(@"Textures\mario3"));
            ball.MarioList.Add(Content.Load<Texture2D>(@"Textures\mario4"));

            playerLeft.Sprite = Content.Load<Texture2D>(@"Textures\blockpaddle");
            playerLeft.FollowSprite = Content.Load<Texture2D>(@"Textures\bigbooleft256");
            playerLeft.PassiveSprite = Content.Load<Texture2D>(@"Textures\bigbooleft256");
            playerLeft.SmackSprite = Content.Load<Texture2D>(@"Textures\bigbooleftattack268");

            playerRight.Sprite = Content.Load<Texture2D>(@"Textures\blockpaddle");
            playerRight.FollowSprite = Content.Load<Texture2D>(@"Textures\bigbooright256");
            playerRight.PassiveSprite = Content.Load<Texture2D>(@"Textures\bigbooright256");
            playerRight.SmackSprite = Content.Load<Texture2D>(@"Textures\bigboorightattack268");

            aiLeft.Sprite = Content.Load<Texture2D>(@"Textures\blockpaddle");
            aiLeft.FollowSprite = Content.Load<Texture2D>(@"Textures\bigbooleft256");
            aiLeft.PassiveSprite = Content.Load<Texture2D>(@"Textures\bigbooleft256");
            aiLeft.SmackSprite = Content.Load<Texture2D>(@"Textures\bigbooleftattack268");

            aiRight.Sprite = Content.Load<Texture2D>(@"Textures\blockpaddle");
            aiRight.FollowSprite = Content.Load<Texture2D>(@"Textures\bigbooright256");
            aiRight.PassiveSprite = Content.Load<Texture2D>(@"Textures\bigbooright256");
            aiRight.SmackSprite = Content.Load<Texture2D>(@"Textures\bigboorightattack268");

            background.BGList.Add(Content.Load<Texture2D>(@"Textures\gh1-1600x900"));
            background.BGList.Add(Content.Load<Texture2D>(@"Textures\gh2-1600x900"));
            background.BGList.Add(Content.Load<Texture2D>(@"Textures\gh3-1600x900"));
            background.BGList.Add(Content.Load<Texture2D>(@"Textures\gh2-1600x900"));

            specialLeft.SpriteList.Add(Content.Load<Texture2D>(@"Textures\hadouken1"));
            specialLeft.SpriteList.Add(Content.Load<Texture2D>(@"Textures\hadouken2"));
            specialLeft.SpriteList.Add(Content.Load<Texture2D>(@"Textures\hadouken3"));


            specialRight.SpriteList.Add(Content.Load<Texture2D>(@"Textures\hadouken1r"));
            specialRight.SpriteList.Add(Content.Load<Texture2D>(@"Textures\hadouken2r"));
            specialRight.SpriteList.Add(Content.Load<Texture2D>(@"Textures\hadouken3r"));

            helpMenu = Content.Load<Texture2D>(@"Textures\helpmenudotted");

            ChargeBar.BarSprite = Content.Load<Texture2D>(@"Textures\bar");
           
            */

            //title.Sprite = Content.Load<Texture2D>(@"Textures\title");
            //title.CenterSprite(SCREEN_WIDTH, SCREEN_HEIGHT);

            //BGMusic = Content.Load<Song>(@"Audio\musicloop");

            //whitepixel = Content.Load<Texture2D>(@"Textures\whitepixel");

        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {

        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            SoundHelper.Update();

            keyboardState = Keyboard.GetState();
            //gamepadState1 = GamePad.GetState(PlayerIndex.One);
            //gamepadState2 = GamePad.GetState(PlayerIndex.Two);


            if (GM.GameIsRunning)
            {
                if (!GM.Pause)
                {
                    SoundHelper.MusicVolume(MusicVolumeDefault);

                    //player.Update(SCREEN_WIDTH, SCREEN_HEIGHT, keyboardState, gamepadState1, ball, gameTime);
                }
            }

            else if (GM.ShowTitleScreen)
            {
                Keys[] keys = keyboardState.GetPressedKeys();
                if (keys.Length != lastKeys.Length && gameTime.TotalGameTime.Milliseconds > 50)
                {
                    transScreen.CueFade();
                    GM.TitleScreenFade = true;
                }
                lastKeys = Keyboard.GetState().GetPressedKeys();

                if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
                    Keyboard.GetState().IsKeyDown(Keys.Escape))
                {
                    this.Exit();
                }
            }




            GM.SetLastKeyboardState();
            GM.SetLastGamePadStates();


            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {

            GraphicsDevice.Clear(screenColor);

            //draw spritebatch but scale everything based on current resolution
            spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, globalTransformation);


            if (GM.GameIsRunning)
            {
                background.Draw(spriteBatch, SCREEN_WIDTH, SCREEN_HEIGHT, gameTime);
                DrawScreenTips(spriteBatch);
                
                /*
                referee.DrawScores(spriteBatch, font, SCREEN_WIDTH, SCREEN_HEIGHT);
                ChargeBar.DrawBars(spriteBatch, SCREEN_WIDTH, SCREEN_HEIGHT, whitepixel);
                ball.Draw(spriteBatch, gameTime);
                playerLeft.Draw(spriteBatch);

                if (GM.NumPlayers == 1)
                    aiRight.Draw(spriteBatch, SCREEN_WIDTH, SCREEN_HEIGHT, whitepixel);
                else if (GM.NumPlayers == 2)
                    playerRight.Draw(spriteBatch);

                specialRight.Draw(spriteBatch, gameTime);
                specialLeft.Draw(spriteBatch, gameTime);
                */

                if (GM.ShowMainMenu)
                {
                    //shade game
                    spriteBatch.Draw(whitepixel, new Rectangle(0, 0, SCREEN_WIDTH, SCREEN_HEIGHT), new Color(0, 0, 0, 100));

                }
            }
            /*
            else if (GM.ShowGameLengthMenu)
            {
                Menu.Draw(spriteBatch, menuFont, menuTitleFont, whitepixel, menuColor, 0f);
            }
            else if (GM.ShowPlayersMenu)
            {
                Menu.Draw(spriteBatch, menuFont, menuTitleFont, whitepixel, menuColor, 0f);
            }

            else if (GM.ShowWinnerScreen)
            {
                referee.DrawWinnerScreen(spriteBatch, winnerScreenFont, SCREEN_WIDTH, SCREEN_HEIGHT);

            }
            else if (GM.ShowTitleScreen)
            {
                string versionString = "ver 1.05 Beta";
                string nameString = "Jeremy Krentz 2011";
                background.DrawSliding(spriteBatch, SCREEN_WIDTH, SCREEN_HEIGHT, gameTime);
                title.Draw(spriteBatch);
                spriteBatch.DrawString(nameFont, nameString,
                                       new Vector2(10, SCREEN_HEIGHT - nameFont.MeasureString(nameString).Y), Color.White);
                spriteBatch.DrawString(nameFont, versionString,
                                       new Vector2(SCREEN_WIDTH - nameFont.MeasureString(versionString).X - 10,
                                                   SCREEN_HEIGHT - nameFont.MeasureString(versionString).Y), Color.White);


            }
             * */

            if (GM.ShowHelpScreen)
            {
                spriteBatch.Draw(helpMenu, new Vector2(SCREEN_WIDTH / 2 - helpMenu.Width / 2, SCREEN_HEIGHT / 2 - helpMenu.Height / 2), Color.White);
            }

            if (transScreen.FadeCued())
                transScreen.DrawFadeBlackAndBack(spriteBatch, gameTime, .5f, .5f, whitepixel);

            spriteBatch.End();

            base.Draw(gameTime);
        }

        private void BeginGame()
        {

            GM.GameIsRunning = true;

            /*
            playerLeft.DefaultPosition = new Vector2(PLAYER_WALL_DISTANCE, SCREEN_HEIGHT / 2 - playerLeft.Sprite.Height / 2);
            playerLeft.Position = playerLeft.DefaultPosition;
            playerLeft.Velocity = 0f;
            playerLeft.SpriteColor = paddleColor;
            playerLeft.DefaultMaxVelocity = MAX_PLAYER_VEL;
            playerLeft.MaxVelocity = MAX_PLAYER_VEL;
            playerLeft.BumperSize = BUMPER_SIZE;
            playerLeft.BumperStrength = BUMPER_STRENGTH;
            playerLeft.PlayerSize = PLAYER_SIZE;
            playerLeft.AdjustFollowPosition();
             * 
             * */

            StartMusic();

            SoundHelper.PlaySound("grow");

        }

        private void DrawScreenTips(SpriteBatch spriteBatch)
        {
            string menuTip = "Main Menu[Esc]";
            spriteBatch.DrawString(smallFont, menuTip, new Vector2(SCREEN_WIDTH / 2 - smallFont.MeasureString(menuTip).X / 2, 10), Color.White);
        }


        private void StartMusic()
        {
            SoundHelper.MusicVolume(MusicVolumeDefault);
            SoundHelper.PlayMusic();
        }

        private void SimpleMusicStart()
        {
            //MediaPlayer.Volume = 0.5f;
            //MediaPlayer.Play(BGMusic);
            //MediaPlayer.IsRepeating = true;
        }
    }
}
