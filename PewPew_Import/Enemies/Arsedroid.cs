using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PewPew.Enemies {
    public class Arsedroid : IEnemy {
        private static readonly Texture2D[] const_Tex = { Consts.Textures.Enemy_Arsedroid1, Consts.Textures.Enemy_Arsedroid2, Consts.Textures.Enemy_Arsedroid3 };
        private static readonly SoundEffect const_wav_Hit = Consts.Audio.Rock;
        private const int const_Damage = 1;
        private const int const_Health = 1;
        private const int const_Score = 1;
        private static readonly Color const_Color = Color.Coral;
        private const float const_Move_MaxSpeed = 0.085f;
        private const float const_Move_RotSpeed = 0.01f;   //zostanie wylosowana liczba z przedziału <-Move_RotSpeed;Move_RotSpeed>
        private const int const_start_Divide = 2;  //ile razy dzieli się od początku               //ilość obiektów można wyliczyć ze wzoru:
        private const int const_Divide_Ammount = 3;    //na ile części dzieli się arsedroid        //Divide_Ammount^start_Divide   (2^5=32)


        protected Texture2D m_Tex;
        protected Vector2 m_Pos, m_Move, m_TexCenter;
        protected Color m_Color, m_StandardColor, m_DamagedColor;
        protected float m_Angle, m_Size, m_VisualAngle, m_Scale;
        protected Stage m_Stage;
        protected SoundEffect m_HitSound;

        protected int m_Damage, m_Score, m_Health, m_DivideRemain, m_DivideAmmount;
        protected double m_DamagedTimeRemain, m_DamagedTime;
        protected float m_MaxSpeed, m_RotSpeed;
        protected bool m_Alive;

        public float Size { get { return this.m_Size; } }
        public float Angle { get { return this.m_VisualAngle; } }
        public Color Color { get { return this.m_Color; } }
        public Vector2 Position { get { return this.m_Pos; } }
        public Texture2D Texture { get { return this.m_Tex; } }
        public int Score { get { return this.m_Score; } }
        public int Health { get { return this.m_Health; } }
        public int Damage { get { return this.m_Damage; } }
        public bool SpecialDraw { get { return true; } }

        public Arsedroid(Stage stage, Vector2 where) {
            this.m_Stage = stage;
            this.m_Tex = const_Tex[Consts.Random.Next( 0, const_Tex.Count() )];
            this.m_StandardColor = this.m_Color = const_Color;
            this.m_DamagedColor = Consts.Values.Enemies.DamagedColor;
            this.m_DamagedTime = Consts.Values.Enemies.DamagedColorTime;

            this.m_Health = const_Health;
            this.m_Score = const_Score;
            this.m_MaxSpeed = const_Move_MaxSpeed;
            this.m_Damage = const_Damage;
            this.m_DivideRemain = const_start_Divide;
            this.m_DivideAmmount = const_Divide_Ammount;
            this.m_RotSpeed = ( float ) Consts.Random.NextDouble() * const_Move_RotSpeed;
            this.m_HitSound = const_wav_Hit;
            this.m_TexCenter = new Vector2( this.m_Tex.Width / 2, this.m_Tex.Height / 2 );
            this.m_DamagedTimeRemain = 0d;
            this.m_Move = Vector2.Zero;
            this.m_Angle = 0f;
            this.m_VisualAngle = Consts.Random.Next( 1, 360 );
            this.m_Pos = where;
            this.m_Size = Math.Max( this.m_Tex.Width / 2, this.m_Tex.Height / 2 );
            this.m_Alive = true;
            this.m_Scale = 1f;
            if( Consts.Random.Next( 0, 1 ) == 0 )
                this.m_RotSpeed /= -1;
            this.ThinkMove();
        }
        public Arsedroid(Stage stage, Vector2 where, int divide)
            : this( stage, where ) {
            this.m_DivideRemain = divide;
            this.m_Scale = ( float ) ( divide + 1 ) / ( float ) ( const_start_Divide + 1 );
            this.m_Size *= this.m_Scale;
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
        public bool Draw(SpriteBatch spriteBatch, GameTime gameTime) {
            spriteBatch.Draw( this.m_Tex, this.Position - this.m_Stage.Camera.Position, null, this.Color, this.Angle, this.m_TexCenter, this.m_Scale, SpriteEffects.None, 1f );
            return true;
        }
        public bool Check() { return this.m_Alive; }
        public void Damaged(int value) {
            this.m_Health -= value;
            this.m_DamagedTimeRemain = this.m_DamagedTime;
            this.m_Color = this.m_DamagedColor;
            Consts.AudioManager.Play( this.m_HitSound );
        }

        protected void Killed() {
            this.m_Stage.EnemyKilled( this );
            this.m_Alive = false;
            if( this.m_DivideRemain-- > 0 ) {
                for( int i=0; i < this.m_DivideAmmount; ++i )
                    this.m_Stage.AddEntity( new Arsedroid( this.m_Stage, this.m_Pos, this.m_DivideRemain ) );
            }
        }
        protected void Move(GameTime gameTime) {
            float time = ( float ) gameTime.ElapsedGameTime.TotalMilliseconds;
            this.m_VisualAngle += this.m_RotSpeed * time;

            if( this.m_Stage.AllowMove( this.m_Pos, this.m_Pos + this.m_Move * time ) )
                this.m_Pos += this.m_Move * time;
            else
                this.ThinkMove();

            Vector2 pos = this.m_Pos;
            int dir = 0;
            if( this.m_Stage.Camera.BackOnScreen( ref pos, ref dir ) ) { this.m_Pos = pos; this.ThinkMove(); }
        }
        protected void ThinkMove() {
            int angle = Consts.Random.Next( 359 );
            this.m_Move = Functions.CirclePoint( Vector2.Zero, this.m_MaxSpeed, 360, angle );
        }
    }
}
