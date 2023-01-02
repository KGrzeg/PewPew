using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;

namespace PewPew.Enemies {
    public class Znake : IEnemy {
        protected Stage m_Stage;
        protected Vector2 m_Pos;
        protected float m_Speed;

        protected int m_Score, m_Health;
        protected Head m_Head;
        protected bool m_Alive;

        public Texture2D Texture { get { return null; } }   //##@! nie powinno nigdy zostać wywołane
        public float Size { get { return 0; } }   //tak jak to
        public float Angle { get { return 0; } } //i to
        public Color Color { get { return Color.White; } } //oraz to...
        public Vector2 Position { get { return this.m_Pos; } }
        public int Score { get { return this.m_Score; } }
        public int Health { get { return this.m_Health; } }
        public int Damage { get { return 0; } }
        public bool SpecialDraw { get { return true; } }

        public Znake(Stage stage, Vector2 where) {
            this.m_Stage = stage;
            this.m_Pos = where;
            this.m_Health = Consts.Values.Enemies.Znake.Health;
            this.m_Speed = Consts.Values.Enemies.Znake.Move_Speed;
            this.m_Score = Consts.Values.Enemies.Znake.Score;
            this.m_Head = new Head( m_Stage, this );
        }
        public void Update(GameTime gameTime) { }
        public bool Draw(SpriteBatch spriteBatch, GameTime gameTime) { return true; }
        public bool Check() { return this.m_Alive; }
        public void Damaged(int damage) {
            this.m_Health -= damage;
            if( this.m_Health < 0 ) {
                this.m_Head.Killed();
                this.m_Alive = false;
                this.m_Stage.EnemyKilled( this );
            }
        }
    }
    public class Head : IEnemy {
        protected Stage m_Stage;
        protected Znake m_Znake;
        protected Tail[] m_Tail;
        protected Texture2D m_Tex;
        protected Vector2 m_Pos;
        protected Color m_Color, m_StandardColor, m_DamagedColor;
        protected float m_Angle, m_Size, m_Speed, m_TargetAngle;
        protected SoundEffect m_HitSound;

        protected bool m_Alive;
        protected int m_Damage, m_Score, m_Health, m_NumOfTailes;
        protected double m_LifeTime, m_SpawnTailCd, m_DamagedTimeRemain, m_DamagedTime;
        protected float m_RotationSpeed;

        public Texture2D Texture { get { return this.m_Tex; } }
        public float Angle { get { return this.m_Angle; } }
        public float Size { get { return this.m_Size; } }
        public Color Color { get { return this.m_Color; } }
        public Vector2 Position { get { return this.m_Pos; } }
        public int Score { get { return this.m_Score; } }
        public int Health { get { return this.m_Health; } }
        public int Damage { get { return this.m_Damage; } }
        public bool SpecialDraw { get { return false; } }

        public Head(Stage stage, Znake znake) {
            this.m_Stage = stage;
            this.m_Znake = znake;
            this.m_Pos = znake.Position;
            this.m_Tex = Consts.Values.Enemies.Znake.Head.Tex;
            this.m_Damage = Consts.Values.Enemies.Znake.Head.Damage;
            this.m_Speed = Consts.Values.Enemies.Znake.Move_Speed;
            this.m_Score = Consts.Values.Enemies.Znake.Head.Score;
            this.m_Health = Consts.Values.Enemies.Znake.Head.Health;
            this.m_StandardColor = this.m_Color = Consts.Values.Enemies.Znake.Head.Color;
            this.m_DamagedColor = Consts.Values.Enemies.DamagedColor;
            this.m_DamagedTime = Consts.Values.Enemies.DamagedColorTime;
            this.m_RotationSpeed = Consts.Values.Enemies.Znake.Head.RotationSpeed;
            this.m_HitSound = Consts.Values.Enemies.wav_Hit;
            this.m_SpawnTailCd = Consts.Values.Enemies.Znake.AddTileTime;
            this.m_Tail = new Tail[Consts.Values.Enemies.Znake.Lenght];
            this.m_Angle = Consts.Random.Next( 1, 359 );
            this.m_Size = Math.Max( this.m_Tex.Width / 2, this.m_Tex.Height / 2 );
            this.m_Alive = true;
            this.m_LifeTime = this.m_NumOfTailes = 0;
            this.m_DamagedTimeRemain = 0d;
            this.m_Stage.AddEntity( this );
            this.RotationThink();
        }
        public void Update(GameTime gameTime) {
            this.Move( gameTime );
            if( this.m_NumOfTailes < this.m_Tail.Count() )
                this.AddTail( gameTime );
            if( this.m_DamagedTimeRemain > 0 ) {
                this.m_DamagedTimeRemain -= gameTime.ElapsedGameTime.TotalMilliseconds;
            } else {
                if( this.m_Color != this.m_StandardColor )
                    this.m_Color = this.m_StandardColor;
            }
        }
        public bool Draw(SpriteBatch spriteBatch, GameTime gameTime) { return true; }
        public bool Check() { return this.m_Alive; }
        public void Damaged(int damage) {
            this.m_Znake.Damaged( damage + this.Health );
            this.m_DamagedTimeRemain = this.m_DamagedTime;
            this.m_Color = this.m_DamagedColor;
            Consts.AudioManager.Play( this.m_HitSound );
        }
        public void Killed() {
            for( int i = 0; i < this.m_NumOfTailes; ++i )
                this.m_Tail[i].Killed();
            this.m_Alive = false;
        }
        protected void AddTail(GameTime gameTime) {
            double time = gameTime.ElapsedGameTime.TotalMilliseconds;
            this.m_LifeTime += time;
            this.m_SpawnTailCd -= time;
            if( this.m_SpawnTailCd < 0 ) {
                this.m_SpawnTailCd = Consts.Values.Enemies.Znake.AddTileTime;
                this.m_Tail[this.m_NumOfTailes++] = new Tail( this.m_Stage, this.m_Znake, this, ( this.m_NumOfTailes == 1 ? null : this.m_Tail[m_NumOfTailes - 2] ) );
            }
        }
        protected void Move(GameTime gameTime) {
            this.UpdateRotation( gameTime );
            int angle = ( int ) ( ( this.Angle - 90 ) % 360 );
            if( angle < 0 )
                angle = 360 + angle;
            Vector2 move = Functions.CirclePoint( Vector2.Zero, ( float ) ( this.m_Speed * gameTime.ElapsedGameTime.TotalMilliseconds ), 360, angle );
            if( this.m_Stage.AllowMove( this.m_Pos, this.m_Pos + move ) )
            //if(true)
            {
                this.m_Pos += move;

                Vector2 pos = this.m_Pos;
                int dir = 0;
                if( this.m_Stage.Camera.BackOnScreen( ref pos, ref dir ) ) {
                    this.m_Pos = pos;
                    if( ( dir & Consts.Values.LEFT_EDGE ) == Consts.Values.LEFT_EDGE ) {
                        this.m_TargetAngle = Consts.Random.Next( 45, 155 );
                        this.m_Angle = Consts.Random.Next( 0, 180 );
                    } else if( ( dir & Consts.Values.RIGHT_EDGE ) == Consts.Values.RIGHT_EDGE ) {
                        this.m_TargetAngle = Consts.Random.Next( 225, 315 );
                        this.m_Angle = Consts.Random.Next( 180, 359 );
                    }

                    if( ( dir & Consts.Values.TOP_EDGE ) == Consts.Values.TOP_EDGE ) {
                        this.m_TargetAngle = Consts.Random.Next( 135, 225 );
                        this.m_Angle = Consts.Random.Next( 90, 270 );
                    } else if( ( dir & Consts.Values.BOTTOM_EDGE ) == Consts.Values.BOTTOM_EDGE ) {
                        this.m_TargetAngle = Consts.Random.Next( 0, 45 );
                        this.m_Angle = Consts.Random.Next( 0, 90 );
                        if( Consts.Random.Next( 0, 1 ) == 1 ) {
                            this.m_TargetAngle = 360 - this.m_TargetAngle;
                            this.m_Angle = 360 - this.m_Angle;
                        }
                    }
                }
            } else {
                this.m_TargetAngle /= -1;
                this.m_Angle /= -1;
                this.m_TargetAngle = ( 360 + this.m_TargetAngle + Consts.Random.Next( -45, 45 ) ) % 360;
                this.m_Angle = ( 360 + this.m_Angle + Consts.Random.Next( -45, 45 ) ) % 360;
                if( this.m_TargetAngle < 0 ) this.m_TargetAngle = 360 - this.m_TargetAngle;
                if( this.m_Angle < 0 ) this.m_Angle = 360 - this.m_Angle;
            }
        }
        protected void UpdateRotation(GameTime gameTime) {
            if( this.Angle - this.m_TargetAngle < Consts.Values.Enemies.Znake.Head.RotationError && this.Angle - this.m_TargetAngle > -Consts.Values.Enemies.Znake.Head.RotationError )
                this.RotationThink();

            if( this.Angle < this.m_TargetAngle )
                this.m_Angle += this.m_RotationSpeed * ( float ) gameTime.ElapsedGameTime.TotalMilliseconds;
            if( this.Angle > this.m_TargetAngle )
                this.m_Angle -= this.m_RotationSpeed * ( float ) gameTime.ElapsedGameTime.TotalMilliseconds;

            if( this.Angle >= 0 )
                this.m_Angle %= 360;
            else
                this.m_Angle = ( this.m_Angle % 360 ) / -1;
        }
        protected void RotationThink() {
            this.m_TargetAngle = this.Angle + Consts.Random.Next( -Consts.Values.Enemies.Znake.Head.RotationRandom, Consts.Values.Enemies.Znake.Head.RotationRandom );
            this.m_TargetAngle %= 360;
            if( this.m_TargetAngle < 0 )
                this.m_TargetAngle = 360 - this.m_TargetAngle;
        }
    }
    public class Tail : IEnemy {
        protected Stage m_Stage;
        protected Znake m_Znake;
        protected Head m_Head;
        protected Texture2D m_Tex;
        protected Vector2 m_Pos;
        protected Color m_Color, m_StandardColor, m_DamagedColor;
        protected float m_Angle, m_Size, m_Speed, m_Distance;
        protected SoundEffect m_HitSound;

        protected bool m_Alive;
        protected int m_Damage, m_Score, m_Health;
        protected Tail m_NextTail;
        protected double m_DamagedTimeRemain, m_DamagedTime;

        public Texture2D Texture { get { return this.m_Tex; } }
        public float Angle { get { return this.m_Angle; } }
        public float Size { get { return this.m_Size; } }
        public Color Color { get { return this.m_Color; } }
        public Vector2 Position { get { return this.m_Pos; } }
        public int Score { get { return this.m_Score; } }
        public int Health { get { return this.m_Health; } }
        public int Damage { get { return this.m_Damage; } }
        public bool SpecialDraw { get { return false; } }

        public Tail(Stage stage, Znake znake, Head head, Tail tail) {
            this.m_Stage = stage;
            this.m_Znake = znake;
            this.m_Head = head;
            this.m_NextTail = tail; //null, jesli to pierwsza część
            this.m_Pos = head.Position;
            this.m_Tex = Consts.Values.Enemies.Znake.Tail.Tex;
            this.m_StandardColor = this.m_Color = Consts.Values.Enemies.Znake.Tail.Color;
            this.m_DamagedColor = Consts.Values.Enemies.Znake.Tail.DamagedColor;    //inny niż standardowy
            this.m_Health = Consts.Values.Enemies.Znake.Tail.Health;
            this.m_Damage = Consts.Values.Enemies.Znake.Tail.Damage;
            this.m_Score = Consts.Values.Enemies.Znake.Tail.score;
            this.m_Speed = Consts.Values.Enemies.Znake.Move_Speed;
            this.m_Distance = Consts.Values.Enemies.Znake.Tail.Distance;
            this.m_DamagedTime = Consts.Values.Enemies.DamagedColorTime;
            this.m_HitSound = Consts.Values.Enemies.wav_Hit;
            this.m_Size = Math.Max( this.m_Tex.Width / 2, this.m_Tex.Height / 2 );
            this.m_DamagedTimeRemain = 0;
            this.m_Alive = true;
            this.m_Stage.AddEntity( this );
        }
        public void Update(GameTime gameTime) {
            this.Move( gameTime );
            if( this.m_DamagedTimeRemain > 0 ) {
                this.m_DamagedTimeRemain -= gameTime.ElapsedGameTime.TotalMilliseconds;
            } else {
                if( this.m_Color != this.m_StandardColor )
                    this.m_Color = this.m_StandardColor;
            }
        }
        public bool Draw(SpriteBatch spriteBatch, GameTime gameTime) { return true; }
        public bool Check() { return this.m_Alive; }
        public void Damaged(int damage) {
            this.m_Znake.Damaged( damage + this.m_Health );
            this.m_DamagedTimeRemain = this.m_DamagedTime;
            this.m_Color = this.m_DamagedColor;
            Consts.AudioManager.Play( this.m_HitSound );
        }
        public void Killed() { this.m_Alive = false; }
        protected void Move(GameTime gameTime) {
            this.UpdateRotation();
            Vector2 target = new Vector2();
            if( this.m_NextTail != null )
                target = this.m_NextTail.Position;
            else
                target = this.m_Head.Position;
            Vector2 move = new Vector2();
            Vector2.Lerp( ref this.m_Pos, ref target, this.m_Distance, out move );

            this.m_Pos = move;
        }
        protected void UpdateRotation() {
            if( this.m_NextTail != null )
                this.m_Angle = Functions.Angle( this.Position, this.m_NextTail.Position );
            else
                this.m_Angle = Functions.Angle( this.Position, this.m_Head.Position );
        }
    }
}
