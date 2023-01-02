using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace PewPew {
    public static class Settings {
        public static string Nick = "GrzesiuKOXpl";
        public static float Volume_Sfx = 0.6f;
        public static float Volume_Music = 1f;
        public static Color Color_Player = Consts.Values.Player.Color;
        public static void Load(SaveGameDataSettings data) {
            Settings.Volume_Sfx = data.Vol_Sfx;
            Settings.Volume_Music = data.Vol_Music;
            Settings.Color_Player = data.PlayerColor;
            Settings.Nick = data.Nick;
        }
    }
}
