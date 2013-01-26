using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using IAT236P.Screens;
using IAT236P.Services;
using Microsoft.Xna.Framework.Input;

namespace IAT236P.Managers
{
    public class ScreenManager
    {

        private List<Screen> mScreens;
        private KeyboardState mPreviousKeyboardState;
        private Screen mCurrentScreen;
        private ScreenState mScreenState;

        public ScreenManager()
        {
            mScreens = new List<Screen>();
            GameScreen gameScreen = new GameScreen();
            MenuScreen menuScreen = new MenuScreen();
            mCurrentScreen = gameScreen;
            AddScreen(menuScreen);
            AddScreen(gameScreen);
            mScreenState = ScreenState.Playing;
        }

        public void AddScreen(Screen screen)
        {
            mScreens.Add(screen);
        }

        public void RemoveScreen(Screen screen)
        {
            mScreens.Remove(screen);
        }

        public Screen GetScreen(Type screenType)
        {
            return mScreens.Where(s => s.GetType() == screenType).FirstOrDefault();
        }

        public void Update(GameTime gameTime)
        {
            mCurrentScreen.Update(gameTime);
            ListenForScreenEvents();
        }

        public void Draw(GameTime gameTime)
        {
            mCurrentScreen.Draw(gameTime);
        }


        private void ListenForScreenEvents()
        {
            if(Keyboard.GetState().IsKeyDown(Keys.Escape) && mPreviousKeyboardState.IsKeyUp(Keys.Escape))
            {
                if(mScreenState == ScreenState.Playing)
                {
                    mScreenState = ScreenState.Paused;
                    mCurrentScreen = mScreens.Where(s => s.GetType() == typeof(MenuScreen)).Select(s => s as MenuScreen).FirstOrDefault();
                }
                else if(mScreenState == ScreenState.Paused)
                {
                    mScreenState = ScreenState.Playing;
                    mCurrentScreen = mScreens.Where(s => s.GetType() == typeof(GameScreen)).Select(s => s as GameScreen).FirstOrDefault();
                }
            }
            mPreviousKeyboardState = Keyboard.GetState();
        }

    }
        

}
