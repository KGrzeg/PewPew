using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace PewPew {
    public static class User {
        public static int Experience,
                         Money,
                         Ship,
                         Stage;
        public static Ship[] Ships;
        public static bool[] BuyedShips;
        public static bool[] PassedStages;

        public static void Reset() {
            User.Experience = 0;
            User.Money = 99999999;
            User.Ship = 0;
            User.Stage = 0;
            User.Ships = Consts.Values.Ships.Array;
            User.BuyedShips = new bool[User.Ships.Count()];
            User.PassedStages = new bool[Consts.Values.Stages.Array.Count()];

            User.BuyedShips[0] = true;
            User.PassedStages[0] = User.PassedStages[1] = User.PassedStages[2] = true;
        }
        public static int Level {
            get {
                int i = 0;
                while( i < Consts.Values.Player.LevelsTable.Count() && User.Experience >= Consts.Values.Player.LevelsTable[i] ) {
                    ++i;
                }
                return ++i;
            }
        }
        public static int LevelPercent {
            get {
                float diff = User.Experience - ( User.Level >= 2 ? Consts.Values.Player.LevelsTable[User.Level - 2] : 0 );
                float interval = Consts.Values.Player.LevelsTable[User.Level - 1] - ( User.Level >= 2 ? Consts.Values.Player.LevelsTable[User.Level - 2] : 0 );
                return ( int ) ( diff / interval * 100 );
            }
        }
        public static void Load(SaveGameDataUser data) {
            User.Experience = data.Experience;
            User.Money = data.Money;
            User.BuyedShips = data.BuyedShips;
            int i = 0;
            foreach( SaveGameDataShip ship in data.Ships )
                User.Ships[i].UpdateData( data.Ships[i++] );
        }
        public static void Update(SaveGameDataUser data) {
            User.Experience = data.Experience;
            User.Money = data.Money;
            User.BuyedShips = data.BuyedShips;
            int i = 0;
            foreach( SaveGameDataShip ship in data.Ships )
                User.Ships[i].UpdateData( data.Ships[i++] );
        }
        public static bool Unlocked(int stage) {
            if( stage > 0 ) {
                if( User.PassedStages[stage - 1] == true )
                    return true;
                else
                    return false;
            } else {
                return true;
            }
        }
    }
}
