using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;

namespace PewPew.Enemies {
    public class Cannon : IEnemy {
        protected Stage m_Stage;
        protected Texture2D m_Tex, m_BulletTex;
        protected Vector2 m_Pos;
        protected Color m_Color, m_StandardColor, m_DamagedColor;
        protected float m_Angle, m_Size, m_BulletSpeed;
        protected SoundEffect m_HitSound;

        protected int m_Damage, m_Score, m_Health, m_Series, m_SeriesRemain;
        protected double m_ThinkTime, m_ThinkTimeRemain, m_DamagedTimeRemain, m_DamagedTime, m_ShootTime, m_ShootTimeRemain;
        protected bool m_Alive, m_BlockRotOnShoot;

        public float Size { get { return this.m_Size; } }
        public float Angle { get { return this.m_Angle; } }
        public Color Color { get { return this.m_Color; } }
        public Vector2 Position { get { return this.m_Pos; } }
        public Texture2D Texture { get { return this.m_Tex; } }
        public int Score { get { return this.m_Score; } }
        public int Health { get { return this.m_Health; } }
        public int Damage { get { return this.m_Damage; } }
        public bool SpecialDraw { get { return false; } }

        public Cannon(Stage stage, Vector2 where) {
            this.m_Stage = stage;
            this.m_Tex = Consts.Values.Enemies.Cannon.Tex;
            this.m_BulletTex = Consts.Values.Enemies.Cannon.Bullet.Tex;
            this.m_StandardColor = this.m_Color = Consts.Values.Enemies.Cannon.Color;
            this.m_DamagedColor = Consts.Values.Enemies.DamagedColor;
            this.m_DamagedTime = Consts.Values.Enemies.DamagedColorTime;
            this.m_Health = Consts.Values.Enemies.Cannon.Health;
            this.m_Score = Consts.Values.Enemies.Cannon.Score;
            this.m_Damage = Consts.Values.Enemies.Cannon.Damage;
            this.m_BulletSpeed = Consts.Values.Enemies.Cannon.Bullet.Speed;
            this.m_ThinkTimeRemain = this.m_ThinkTime = Consts.Values.Enemies.Cannon.ThinkTime;
            this.m_Series = Consts.Values.Enemies.Cannon.Series;
            this.m_ShootTime = Consts.Values.Enemies.Cannon.SeriesInterval;
            this.m_BlockRotOnShoot = Consts.Values.Enemies.Cannon.BlockRotOnShoot;
            this.m_HitSound = Consts.Values.Enemies.wav_Hit;
            this.m_DamagedTimeRemain = this.m_ShootTimeRemain = 0d;
            this.m_Angle = 0f;
            this.m_SeriesRemain = 0;
            this.m_Pos = where;
            this.m_Size = Math.Max( this.m_Tex.Width / 2, this.m_Tex.Height / 2 );
            this.m_Alive = true;
        }
        public void Update(GameTime gameTime) {
            if( this.m_Health <= 0 )
                this.Killed();
            if( this.m_Alive ) {
                if( this.m_DamagedTimeRemain > 0 ) {
                    this.m_DamagedTimeRemain -= gameTime.ElapsedGameTime.TotalMilliseconds;
                } else {
                    if( this.m_Color != this.m_StandardColor )
                        this.m_Color = this.m_StandardColor;
                }
                if( ( this.m_BlockRotOnShoot && this.m_SeriesRemain == 0 ) || !this.m_BlockRotOnShoot )
                    this.m_Angle = Functions.Angle( this.m_Pos, this.m_Stage.Player.Position );
                this.Shooting( gameTime );
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
        protected void Shooting(GameTime gameTime) {
            float time = ( float ) gameTime.ElapsedGameTime.TotalMilliseconds;

            if( this.m_SeriesRemain > 0 && this.m_ShootTimeRemain <= 0 ) {
                //strzał
                int angle = ( int ) Functions.CorrectAngle( this.m_Angle - 90 );
                this.m_Stage.AddEntity( new CannonBullet( this.m_BulletTex, this.m_Pos, Functions.CirclePoint( Vector2.Zero, this.m_BulletSpeed, 360, angle ), this.m_Stage ) );
                this.m_ShootTimeRemain = this.m_ShootTime;
                --this.m_SeriesRemain;
            }
            if( this.m_ThinkTimeRemain <= 0 ) {
                this.Think();
            }
            this.m_ShootTimeRemain -= time;
            this.m_ThinkTimeRemain -= time;
        }
        protected void Think() {
            this.m_ThinkTimeRemain = this.m_ThinkTime;
            this.m_SeriesRemain = this.m_Series;
            this.m_ShootTimeRemain = 0;
        }
        protected void Killed() { this.m_Stage.EnemyKilled( this ); this.m_Alive = false; }
    }
    public class CannonBullet : IGameEntity {
        protected Texture2D m_Tex;
        protected Vector2 m_Pos, m_Move;
        protected Color m_Color;
        protected float m_Angle;
        protected float m_Size;
        protected bool m_Allow; //czy ten obiekt bierze jeszcze udział w grze
        protected Stage m_Stage;
        protected int m_Damage;

        public float Size { get { return this.m_Size; } }
        public float Angle { get { return this.m_Angle; } }
        public Color Color { get { return this.m_Color; } }
        public Vector2 Position { get { return this.m_Pos; } }
        public Texture2D Texture { get { return this.m_Tex; } }
        public bool SpecialDraw { get { return false; } }

        /// <summary>
        /// Takimi pociskami posługuje się przeciwnik Cannon
        /// </summary>
        /// <param name="tex">Teksturka obiektu</param>
        /// <param name="pos">Miejsce, w którym pojawia się pocisk</param>
        /// <param name="move">Wektor określający kierunek i prędkość pocisku</param>
        /// <param name="stage">Referencja do poziomu gry</param>
        public CannonBullet(Texture2D tex, Vector2 pos, Vector2 move, Stage stage) {
            this.m_Stage = stage;
            this.m_Tex = tex;
            this.m_Pos = pos;
            this.m_Move = move;
            this.m_Damage = Consts.Values.Enemies.Cannon.Bullet.Damage;
            this.m_Color = Consts.Values.Enemies.Cannon.Bullet.Color;
            this.m_Angle = Functions.Angle( Vector2.Zero, move );
            this.m_Size = Math.Max( tex.Width / 2, tex.Height / 2 );
            this.m_Allow = true;
        }
        public void Update(GameTime gameTime) {
            this.Move( gameTime );
            this.CheckPlayerCollision();
        }
        public bool Draw(SpriteBatch spriteBatch, GameTime gameTime) { return true; }

        protected void CheckPlayerCollision() {
            if( Vector2.Distance( this.m_Stage.Player.Position, this.m_Pos ) <= this.m_Size + this.m_Stage.Player.Size ) {
                this.m_Stage.Player.Damaged( this.m_Damage );
                this.Remove();
            }
        }
        protected void Move(GameTime gameTime) {
            float time = ( float ) gameTime.ElapsedGameTime.TotalMilliseconds;
            Vector2 pos = new Vector2();
            pos = this.m_Pos;
            int sum = 0;
            if( !this.m_Stage.Camera.BackOnScreen( ref pos, ref sum ) && this.m_Stage.AllowMove( this.m_Pos, this.m_Pos + m_Move * time ) )
                this.m_Pos += m_Move * time;
            else
                this.Remove();
        }
        protected void Remove() { this.m_Allow = false; }
        public bool Check() { return this.m_Allow; }
    }
}
