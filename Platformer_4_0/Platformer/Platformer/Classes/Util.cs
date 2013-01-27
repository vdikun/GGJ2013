using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;

namespace Platformer
{
    static class Util
    {
        public static float SCALE = 0.5f;
        public static float OFFSET = 300.0f;
        public static float MUSIC_VOLUME = 0.5f;
        public static float SFX_VOLUME = 1f;

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

        public static float scale(float input)
        {
            return SCALE * input;
        }

        public static float offsetY(float input)
        {
            return OFFSET + input;
        }
    }
}
