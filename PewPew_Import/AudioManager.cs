using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;

namespace PewPew {
    public enum SoundType {
        Sfx,
        Music,
        Ambient,
        Movie
    }
    public class AudioManager {
        public AudioManager() {
            MediaPlayer.Volume = Settings.Volume_Music;
            MediaPlayer.IsRepeating = true;
        }
        public void Update() {
            MediaPlayer.Volume = Settings.Volume_Music;
        }
        public void Play(SoundEffect sound) {
            sound.Play( Settings.Volume_Sfx, 0f, 0f );
        }
        public void Play(Song music) {
            MediaPlayer.Play( music );
        }
        public void Stop() {
            MediaPlayer.Stop();
        }
    }
}
