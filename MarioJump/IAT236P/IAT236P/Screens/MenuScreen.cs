using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using IAT236P.UI;
using Microsoft.Xna.Framework;
using IAT236P.Services;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;

namespace IAT236P.Screens
{
    public class MenuScreen : Screen
    {
        private List<UIElement> mUIElements;
        private SpriteBatch mSpriteBatch;
        private SpriteFont mSpriteFont;
        private KeyboardState mPrevState;
        private int mSelectedIndex;


        public MenuScreen() 
        {
            mUIElements = new List<UIElement>();
            UIElement quitElement = new UIElement(null, "Quit");
            UIElement settingsElement = new UIElement(null, "Settings");
            UIElement newGameElement = new UIElement(null, "New Game");
            UIElement testElement = new UIElement(null, "Test");
            mUIElements.Add(settingsElement);
            mUIElements.Add(quitElement);
            mUIElements.Add(newGameElement);
            mUIElements.Add(testElement);
            mSpriteBatch = ServiceLocator.GetService<SpriteBatch>();
            ContentManager contentManager = ServiceLocator.GetService<ContentManager>();
            mSpriteFont = contentManager.Load<SpriteFont>("MenuFont");
            ExitGameChanged += OnExitGameChanged;

        }


        public event EventHandler<EventArgs> ExitGameChanged;

        protected void OnExitGameChanged(object sender, EventArgs e) { }

        public override void Update(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Down) && mPrevState.IsKeyUp(Keys.Down))
            {
                if(mSelectedIndex < mUIElements.Count - 1)
                {
                    mSelectedIndex++;
                }
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Up) && mPrevState.IsKeyUp(Keys.Up))
            {
                if(mSelectedIndex > 0)
                {
                    mSelectedIndex--;
                }
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Enter) && mPrevState.IsKeyUp(Keys.Enter))
            {
                //do whatever is selected
                if(mUIElements[mSelectedIndex].Text == "Quit")
                {
                    ExitGameChanged(this, EventArgs.Empty);
                }
                if(mUIElements[mSelectedIndex].Text == "Settings")
                {
                    
                }
            }
            mPrevState = Keyboard.GetState();
            base.Update(gameTime);


        }

        public override void Draw(GameTime gameTime)
        {
            mSpriteBatch.Begin();

            int elementCount = mUIElements.Count;
            int windowHeight = ServiceLocator.GetService<GraphicsDevice>().PresentationParameters.BackBufferHeight;
            int windowWidth = ServiceLocator.GetService<GraphicsDevice>().PresentationParameters.BackBufferWidth;
            int startingWidth = windowWidth / 3;

            mSpriteBatch.DrawString(mSpriteFont, mUIElements[mSelectedIndex].Text + " is Selected!", new Vector2(0, 0), Color.Black);

            for(int i = 1; i < elementCount+1; i++)
            {
                UIElement element = mUIElements[i-1];

                Vector2 pos = new Vector2(startingWidth, 100 +  i*50);

                
                mSpriteBatch.DrawString(mSpriteFont, element.Text, pos, Color.Black);
            }

            mSpriteBatch.End();

            base.Draw(gameTime);
        }
    }



}
