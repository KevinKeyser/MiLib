using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MiLib.CoreTypes
{
    public static class AudioManager
    {
        static List<SoundEffectInstance> sounds = new List<SoundEffectInstance>();

        private static bool soundon = true;

        public static bool SoundOn
        {
            get
            {
                return soundon;
            }
            set
            {
                soundon = value;
                foreach(SoundEffectInstance sound in sounds)
                {
                    sound.Volume = soundon ? 1 : 0;
                }
            }
        }

        public static bool MusicMuted
        {
            get
            {
                return MediaPlayer.IsMuted;
            }
            set
            {
                MediaPlayer.IsMuted = value;
            }
        }

        public static bool MusicLooping
        {
            get
            {
                return MediaPlayer.IsRepeating;
            }
            set
            {
                MediaPlayer.IsRepeating = value;
            }
        }

        public static void PlayMusic(Song song)
        {
            MediaPlayer.Play(song);
        }

        public static void StartSound(SoundEffect sound)
        {
            sounds.Add(sound.CreateInstance());
            sounds[sounds.Count - 1].Play();
        }

        public static void Update()
        {
            for (int i = 0; i < sounds.Count; i++)
            {
                if(sounds[i].State == SoundState.Stopped)
                {
                    sounds[i].Dispose();
                    sounds.RemoveAt(i);
                    i--;
                }
            }
        }
    }
}
