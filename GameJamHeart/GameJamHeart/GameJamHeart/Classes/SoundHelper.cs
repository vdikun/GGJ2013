using System;
using Microsoft.Xna.Framework.Audio;


namespace GameJamHeart
{
    static class SoundHelper
    {
        static AudioEngine audioEngine;
        static WaveBank waveBank;
        static SoundBank soundBank;

        static Cue musicCue;
        static Cue winCue;

        static string[] sounds;

        public static void Initialize()
        {
            //audioEngine = new AudioEngine(@"Content\Audio\XACTproj.xgs");

            //waveBank = new WaveBank(audioEngine, @"Content\Audio\Wave Bank.xwb");

            //soundBank = new SoundBank(audioEngine, @"Content\Audio\Sound Bank.xsb");

            //sounds = new string[] { "kick2left", "kick2right", "kickleft", "kickright", "stomp", "coinleft", "coinright", "grow", "stompquiet", "musicloop", "levelclear", "bigstomp" };

            //musicCue = soundBank.GetCue("musicloop");
            //winCue = soundBank.GetCue("levelclear");
        }

        public static void Update()
        {
           // audioEngine.Update();

        }

        public static void PlayMusic()
        {
            musicCue = soundBank.GetCue("musicloop");
            musicCue.Play();
        }

        public static void PlayWin()
        {
            winCue = soundBank.GetCue("levelclear");
            winCue.Play();
        }

        public static void StopWin()
        {
            winCue.Stop(AudioStopOptions.Immediate);
        }

        public static void MuteSounds()
        {
            AudioCategory global = audioEngine.GetCategory("Global");

            global.SetVolume(0);
        }

        public static void EnableSound()
        {
            AudioCategory global = audioEngine.GetCategory("Global");

            global.SetVolume(1);
        }

        public static void MusicVolume(float volume)
        {
            AudioCategory music = audioEngine.GetCategory("Music");

            music.SetVolume(volume);
        }

        public static void MusicDefaultVolume()
        {
            AudioCategory music = audioEngine.GetCategory("Music");

            music.SetVolume(.3f);
        }

        public static void MusicQuietVolume()
        {
            AudioCategory music = audioEngine.GetCategory("Music");

            music.SetVolume(.05f);
        }

        public static void StopMusic()
        {
            musicCue.Stop(AudioStopOptions.Immediate);
        }

        public static void PlaySound(int soundIndex)
        {
            soundBank.PlayCue(sounds[soundIndex]);
        }

        public static void PlaySound(string soundName)
        {
            for (int i = 0; i < sounds.Length; i++)
            {
                if (sounds[i] == soundName)
                {
                    PlaySound(i);
                    break;
                }
            }
        }
    }
}
