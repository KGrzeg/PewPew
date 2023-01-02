using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;

namespace PewPew.Enemies {
    public class Phantom : IEnemy {
        protected Stage m_Stage;
        protected Texture2D m_Tex;
        protected Vector2 m_Pos, m_Move;
        protected Color m_Color, m_StandardColor, m_DamagedColor;
        protected float m_Angle, m_Size;
        protected SoundEffect m_HitSound;

        protected int m_Damage, m_Score, m_Health;
        protected float m_MaxSpeed, m_MinSpeed, m_Drag, m_Accelerate, m_Distance;
        protected bool m_Alive;
        protected double m_DamagedTimeRemain, m_DamagedTime;

        protected float Speed { get { return this.m_Move.Length(); } }
        public float Size { get { return this.m_Size; } }
        public float Angle { get { return this.m_Angle; } }
        public Color Color { get { return this.m_Color; } }
        public Vector2 Position { get { return this.m_Pos; } }
        public Texture2D Texture { get { return this.m_Tex; } }
        public int Score { get { return this.m_Score; } }
        public int Health { get { return this.m_Health; } }
        public int Damage { get { return this.m_Damage; } }
        public bool SpecialDraw { get { return false; } }

        public Phantom(Stage stage, Vector2 where) {
            this.m_Stage = stage;
            this.m_Tex = Consts.Values.Enemies.Phantom.Tex;
            this.m_StandardColor = this.m_Color = Consts.Values.Enemies.Phantom.Color;
            this.m_DamagedColor = Consts.Values.Enemies.DamagedColor;
            this.m_DamagedTime = Consts.Values.Enemies.DamagedColorTime;
            this.m_Health = Consts.Values.Enemies.Phantom.Health;
            this.m_Score = Consts.Values.Enemies.Phantom.Score;
            this.m_MaxSpeed = Consts.Values.Enemies.Phantom.move_MaxSpeed;
            this.m_MinSpeed = Consts.Values.Enemies.Phantom.move_MinSpeed;
            this.m_Damage = Consts.Values.Enemies.Phantom.Damage;
            this.m_Drag = Consts.Values.Enemies.Phantom.move_Drag;
            this.m_Accelerate = Consts.Values.Enemies.Phantom.move_Accelerate;
            this.m_Distance = Consts.Values.Enemies.Phantom.Distance;
            this.m_Size = Consts.Values.Enemies.Phantom.Size;
            this.m_HitSound = Consts.Values.Enemies.wav_Hit;
            this.m_DamagedTimeRemain = 0d;
            this.m_Pos = where;
            this.m_Alive = true;
            this.m_Angle = Functions.Angle( this.m_Pos, this.m_Stage.Player.Position );
            this.m_Move = Functions.CirclePoint( Vector2.Zero, this.m_MinSpeed, 360, ( int ) Functions.CorrectAngle( this.Angle - 90 ) );
        }
        public void Update(GameTime gameTime) {
            if( this.m_Health <= 0 )
                this.Killed();
            if( this.m_Alive ) {
                this.Move( gameTime );
                if( this.m_DamagedTimeRemain > 0 ) {
                    this.m_DamagedTimeRemain -= gameTime.ElapsedGameTime.TotalMilliseconds;
                } else {
                    if( this.m_Color != this.m_StandardColor )
                        this.m_Color = this.m_StandardColor;
                }
            }
        }
        public bool Draw(SpriteBatch spriteBatch, GameTime gameTime) { return true; }
        public bool Check() { return this.m_Alive; }
        public void Damaged(int value) {
            this.m_Health -= value;
            this.m_DamagedTimeRemain = this.m_DamagedTime;
            this.m_Color = this.m_DamagedColor;
            Consts.AudioManager.Play( this.m_HitSound );
        }

        protected void Killed() { this.m_Stage.EnemyKilled( this ); this.m_Alive = false; }
        protected void Move(GameTime gameTime) {
            float time = ( float ) gameTime.ElapsedGameTime.TotalMilliseconds;
            this.m_Angle = Functions.Angle( this.m_Pos, this.m_Stage.Player.Position );

            this.m_Move = Functions.CirclePoint( Vector2.Zero, this.Speed, 360, Functions.CorrectAngle( ( int ) this.Angle - 90 ) );
            this.m_Pos += this.m_Move * time;
            Vector2 pos = this.m_Pos;
            int dir = 0;
            if( this.m_Stage.Camera.BackOnScreen( ref pos, ref dir ) ) { this.m_Pos = pos; }

            if( Vector2.Distance( this.Position, this.m_Stage.Player.Position ) > this.m_Distance ) {
                if( this.Speed < this.m_MinSpeed )
                    this.m_Move = Functions.CirclePoint( Vector2.Zero, this.m_MinSpeed, 360, Functions.CorrectAngle( ( int ) this.Angle - 90 ) );
                else
                    if( this.Speed < this.m_MaxSpeed )
                        this.m_Move *= this.m_Accelerate;
                    else
                        this.m_Move = Functions.CirclePoint( Vector2.Zero, this.m_MaxSpeed, 360, Functions.CorrectAngle( ( int ) this.Angle - 90 ) );
            } else {
                if( this.Speed > this.m_MinSpeed )
                    this.m_Move *= this.m_Drag;
            }
        }
    }
}
