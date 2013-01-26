using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace IAT236P.Screens
{
    public class Screen
    {

        public Screen()
        {
        }


        public virtual void Update(GameTime gameTime)
        {
        }

        public virtual void Draw(GameTime gameTime)
        {
        }


    }

    public enum ScreenState
    {
        Playing,
        Paused
    }
}
