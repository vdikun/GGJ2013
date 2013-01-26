using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace GameJamHeart
{
    static class GM
    {
        //public static int GameLength;
        public static bool Pause;
        public static bool ShowMainMenu;
        public static bool ShowTitleScreen;
        public static bool GameIsRunning;
        public static bool ShowGameLengthMenu;
        public static bool GameRestart;
        public static bool ValueChanged;
        public static bool ShowWinnerScreen;
        public static bool PostGLMenuFade;
        public static bool ShowPlayersMenu;
        public static int NumPlayers;
        public static bool PostPlayersMenuFade;
        public static bool PostGameRestartFade;
        public static bool ShowHelpScreen;
        public static bool TitleScreenFade;
        public static bool HelpScreenFade;



        public static KeyboardState LastKeyboardState;
        public static GamePadState LastP1GamePadState;
        public static GamePadState LastP2GamePadState;


        public static void SetLastKeyboardState()
        {
            LastKeyboardState = Keyboard.GetState();
        }

        public static void SetLastGamePadStates()
        {
            LastP1GamePadState = GamePad.GetState(PlayerIndex.One);
            LastP2GamePadState = GamePad.GetState(PlayerIndex.Two);
        }

        public static GamePadState LastGamePadState(PlayerIndex playerIndex)
        {
            if (playerIndex.Equals(PlayerIndex.One))
                return LastP1GamePadState;
            else
                return LastP2GamePadState;
        }
    }
}
