using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;

namespace Platformer
{
    static class Util
    {
        public static bool IsAnyKeyDown(KeyboardState keyboard, Keys[] keys)
        {
            foreach (Keys key in keys)
            {
                if (keyboard.IsKeyDown(key))
                    return true;
            }
            return false;
        }

        public static bool IsAnyKeyUp(KeyboardState keyboard, Keys[] keys)
        {
            foreach (Keys key in keys)
            {
                if (keyboard.IsKeyUp(key))
                    return true;
            }
            return false;
        }

        public static bool IsAnyKeyPressed(KeyboardState keyboard, KeyboardState prevKeyboard, Keys[] keys)
        {
            foreach (Keys key in keys)
            {
                if (keyboard.IsKeyDown(key) && prevKeyboard.IsKeyUp(key))
                    return true;
            }
            return false;
        }
    }
}
