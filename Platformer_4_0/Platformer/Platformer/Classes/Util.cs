using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;

namespace Platformer
{
    static class Util
    {
        public static float SCALE = 0.65f;
        public static float OFFSET = 252.0f;
        public static float MUSIC_VOLUME = 0.35f;
        public static float SFX_VOLUME = 1f;

        public static int gamesWon = 0;
        public static readonly int GAMES_TO_WIN = 3;

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

        public static void GotoRandomMinigame(PlatformerGame game)
        {
            GameState nextGame = game.currentState;

            switch (new Random().Next(0, 5))
            {
                case 0:
                    nextGame = new MiniGame1State();
                    break;
                case 1:
                    nextGame = new MiniGame2State();
                    break;
                case 2:
                    nextGame = new MiniGame3State();
                    break;
                case 3:
                    nextGame = new MiniGame4State();
                    break;
                case 4:
                    nextGame = new MiniGame5State();
                    break;
                default:
                    break;
            }
            if (nextGame == game.currentState)
            {
                GotoRandomMinigame(game);
            }
            else
            {
                game.currentState = nextGame;
            }
        }
    }
}
