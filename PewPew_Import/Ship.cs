using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;

namespace PewPew {

    public class Ship {
        public Texture2D Texture;
        public int Experience, LevelCost, Cost;
        public int LevelSpeed, LevelAgility, LevelHealth;
        public string Name;

        public int[] tableHealth,
                     needLevelSpeed,needLevelAgility,needLevelHealth,
                     costSpeed,costAgility,costHealth;
        public float[] tableSpeed,
                       tableAccelerate,
                       tableDrag;
        public float Speed { get { return this.tableSpeed[this.LevelSpeed]; } }
        public float Agility { get { return this.tableAccelerate[this.LevelAgility]; } }
        public float Drag { get { return this.tableDrag[this.LevelAgility]; } }
        public int Health { get { return this.tableHealth[this.LevelHealth]; } }
        public float NextSpeed { get { if( this.LevelSpeed + 1 < this.tableSpeed.Count() )return this.tableSpeed[this.LevelSpeed + 1]; else return 0; } }
        public float NextAgility { get { if( this.LevelAgility + 1 < this.tableAccelerate.Count() )return this.tableAccelerate[this.LevelAgility + 1]; else return 0; } }
        public int NextHealth { get { if( this.LevelHealth + 1 < this.tableHealth.Count() )return this.tableHealth[this.LevelHealth + 1]; else return 0; } }
        public int NextCostSpeed { get { return this.costSpeed[this.LevelSpeed]; } }
        public int NextCostAgility { get { return this.costAgility[this.LevelAgility]; } }
        public int NextCostHealth { get { return this.costHealth[this.LevelHealth]; } }
        public int NextLevelSpeed { get { return this.needLevelSpeed[this.LevelSpeed]; } }
        public int NextLevelAgility { get { return this.needLevelAgility[this.LevelAgility]; } }
        public int NextLevelHealth { get { return this.needLevelHealth[this.LevelHealth]; } }
        public int Level {
            get {
                int i = 0;
                while( i < Consts.Values.Ships.tableExperience.Count() && this.Experience >= Consts.Values.Ships.tableExperience[i] ) {
                    ++i;
                }
                return ++i;
            }
        }
        public int LevelPercent {
            get {
                float diff = this.Experience - ( this.Level >= 2 ? Consts.Values.Ships.tableExperience[this.Level - 2] : 0 );
                float interval = Consts.Values.Ships.tableExperience[this.Level - 1] - ( this.Level >= 2 ? Consts.Values.Ships.tableExperience[this.Level - 2] : 0 );
                return ( int ) ( diff / interval * 100 );
            }
        }

        public bool CanUpgradeSpeed() {
            if( this.Level >= this.needLevelSpeed[this.LevelSpeed] )
                return true;
            return false;
        }
        public bool CanUpgradeAgility() {
            if( this.Level >= this.needLevelAgility[this.LevelAgility] )
                return true;
            return false;
        }
        public bool CanUpgradeHealth() {
            if( this.Level >= this.needLevelHealth[this.LevelHealth] )
                return true;
            return false;
        }

        public void UpgradeSpeed() {
            if( User.Money < this.NextCostSpeed ||
                this.Level < this.NextLevelSpeed )
                return;
            if( this.LevelSpeed - 1 == this.tableSpeed.Count() )
                return;
            User.Money -= this.NextCostSpeed;
            ++this.LevelSpeed;
        }
        public void UpgradeAgility() {
            if( User.Money < this.NextCostAgility ||
                this.Level < this.NextLevelAgility )
                return;
            if( this.LevelAgility - 1 == this.tableAccelerate.Count() )
                return;
            User.Money -= this.NextCostAgility;
            ++this.LevelAgility;
        }
        public void UpgradeHealth() {
            if( User.Money < this.NextCostHealth ||
                this.Level < this.NextLevelHealth )
                return;
            if( this.LevelHealth - 1 == this.tableHealth.Count() )
                return;
            User.Money -= this.NextCostHealth;
            ++this.LevelHealth;
        }

        public void UpdateData(SaveGameDataShip ship) {
            this.Experience = ship.Experience;
            this.LevelSpeed = ship.LevelSpeed;
            this.LevelAgility = ship.LevelAgility;
            this.LevelHealth = ship.LevelHealth;
        }
    }
}
