using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;

namespace PewPew.Enemies {
    public class Impulse : IEnemy {
        protected Stage m_Stage;
        protected Texture2D m_Tex;
        protected Vector2 m_Pos, m_Move;
        protected Color m_Color, m_StandardColor, m_DamagedColor;
        protected float m_Angle, m_Size;
        protected SoundEffect m_HitSound;

        protected int m_Damage, m_Score, m_Health;
        protected float m_MaxSpeed, m_Drag, m_MinimumSpeed, m_Accelerate;
        protected bool m_Alive, m_SpeedUp;
        protected double m_DamagedTimeRemain, m_DamagedTime;

        public float Size { get { return this.m_Size; } }
        public float Angle { get { return this.m_Angle; } }
        public Color Color { get { return this.m_Color; } }
        public Vector2 Position { get { return this.m_Pos; } }
        public Texture2D Texture { get { return this.m_Tex; } }
        public int Score { get { return this.m_Score; } }
        public int Health { get { return this.m_Health; } }
        public int Damage { get { return this.m_Damage; } }
        public bool SpecialDraw { get { return false; } }

        public Impulse(Stage stage, Vector2 where) {
            this.m_Stage = stage;
            this.m_Tex = Consts.Values.Enemies.Impulse.Tex;
            this.m_StandardColor = this.m_Color = Consts.Values.Enemies.Impulse.Color;
            this.m_DamagedColor = Consts.Values.Enemies.DamagedColor;
            this.m_DamagedTime = Consts.Values.Enemies.DamagedColorTime;
            this.m_Health = Consts.Values.Enemies.Impulse.Health;
            this.m_Score = Consts.Values.Enemies.Impulse.Score;
            this.m_MaxSpeed = Consts.Values.Enemies.Impulse.Move_MaxSpeed;
            this.m_Damage = Consts.Values.Enemies.Impulse.Damage;
            this.m_Drag = Consts.Values.Enemies.Impulse.move_Drag;
            this.m_MinimumSpeed = Consts.Values.Enemies.Impulse.move_Minimum;
            this.m_Accelerate = Consts.Values.Enemies.Impulse.move_Accelerate;
            this.m_HitSound = Consts.Values.Enemies.wav_Hit;
            this.m_Size = Math.Max( this.m_Tex.Width / 2, this.m_Tex.Height / 2 );
            this.m_Move = Vector2.Zero;
            this.m_Angle = 0f;
            this.m_DamagedTimeRemain = 0d;
            this.m_Pos = where;
            this.m_Alive = true;
            this.m_SpeedUp = false;
            this.ThinkMove();
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

            if( this.m_Stage.AllowMove( this.m_Pos, this.m_Pos + this.m_Move * time ) )
                this.m_Pos += this.m_Move * time;
            else
                this.ThinkMove();

            Vector2 pos = this.m_Pos;
            int dir = 0;
            if( this.m_Stage.Camera.BackOnScreen( ref pos, ref dir ) ) { this.m_Pos = pos; this.ThinkMove(); }


            if( this.m_SpeedUp ) {
                this.m_Move *= this.m_Accelerate;
                if( this.m_Move.Length() >= this.m_MaxSpeed )
                    this.m_SpeedUp = false;
            } else {
                this.m_Move *= this.m_Drag;
            }
            if( !this.m_SpeedUp && this.m_Move.Length() < this.m_MinimumSpeed )
                this.ThinkMove();
        }
        protected void ThinkMove() {
            int angle = Consts.Random.Next( 359 );
            this.m_Move = Functions.CirclePoint( Vector2.Zero, 0.01f, 360, angle );
            int tAngle = angle + 90;
            tAngle %= 360;
            if( tAngle < 0 )
                tAngle = 360 - tAngle;
            this.m_Angle = tAngle;
            this.m_SpeedUp = true;
        }
    }
}
