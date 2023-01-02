using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace PewPew {
    public class Player : ICameraDrawable {
        private Stage m_Stage;
        private Texture2D m_Tex;
        private Vector2 m_Pos, m_Move;
        private Color m_Color;
        private SoundEffect wav_Shoot, wav_Spawn;
        private float m_Angle,
                      m_Size,
                      m_MaxSpeed,
                      m_Acc,
                      m_Drag;
        private int m_Health,
                    m_MaxHealth,
                    m_Lives;
        private bool m_Intouchable;
        private double m_moveCoolDown,
                       m_lastShoot,
                       m_shootCooldown,
                       m_intouchableBlinkCoolDown,
                       m_intouchableTime;

        public bool Intouchable { get { return m_Intouchable; } }
        public int Lives { get { return this.m_Lives; } }
        public int Health { get { return this.m_Health; } }
        public int MaxHealth { get { return this.m_MaxHealth; } }
        public Vector2 Position { get { return this.m_Pos; } }
        public Texture2D Texture { get { return this.m_Tex; } }
        public Color Color { get { return this.m_Color; } }
        public float Size { get { return this.m_Size; } }
        public float Angle { get { return this.m_Angle; } }
        public bool SpecialDraw { get { return true; } }//nigdy nie zostanie wywołane

        public Player(Stage stage) {
            this.m_Stage = stage;
            this.m_Tex = Consts.Textures.ship_Avast;
            this.m_Pos = new Vector2( 200 );
            this.m_Move = new Vector2();
            this.m_MaxHealth = this.m_Health = Consts.Values.Player.MaxHealth;
            this.m_MaxSpeed = Consts.Values.Player.move_MaxSpeed;
            this.m_Acc = Consts.Values.Player.move_Accelerate;
            this.m_Drag = Consts.Values.Player.move_Drag;
            this.m_Color = Settings.Color_Player;
            this.m_Lives = Consts.Values.Player.start_Lives;
            this.wav_Shoot = Consts.Values.Player.wav_Shoot;
            this.wav_Spawn = Consts.Values.Player.wav_Spawn;
            this.m_Size = this.m_Tex.Width / 2;
            this.m_Intouchable = false;

            this.m_shootCooldown = Consts.Values.Player.fire_cooldown;
            this.m_lastShoot = this.m_shootCooldown;
            this.m_moveCoolDown = 0;
            this.m_intouchableBlinkCoolDown = 0;
            this.m_intouchableTime = 0;
        }
        public Player(Stage stage, Ship ship)
            : this( stage ) {
            this.m_Tex = ship.Texture;
            this.m_Health = ship.Health;
            this.m_MaxSpeed = ship.Speed;
            this.m_Acc = ship.Agility;
            this.m_Drag = ship.Drag;
        }

        public void Update(GameTime gameTime) {
            if( this.m_Health <= 0 )
                this.Killed();
            this.ControlCooldowns( gameTime );
            this.Move( gameTime );
            this.UpdateRotation();
            this.CheckDamaged();

            if( Mouse.GetState().LeftButton == ButtonState.Pressed && this.m_lastShoot <= 0 )
                Shoot( gameTime );
            if( this.m_Color != Settings.Color_Player )
                this.m_Color = Settings.Color_Player;
        }
        public bool Draw(SpriteBatch spriteBatch, GameTime gameTime) {
            double time = gameTime.ElapsedGameTime.TotalMilliseconds;

            if( this.m_Intouchable ) {
                if( this.m_intouchableBlinkCoolDown < 0 )
                    this.m_intouchableBlinkCoolDown = Consts.Values.Player.intouchable_blink_time + Consts.Values.Player.intouchable_blink_invisible_time;
                if( !( this.m_intouchableBlinkCoolDown >= Consts.Values.Player.intouchable_blink_invisible_time ) )
                    spriteBatch.Draw( this.m_Tex, new Rectangle( ( int ) ( this.m_Pos.X - this.m_Stage.Camera.Position.X ), ( int ) ( this.m_Pos.Y - this.m_Stage.Camera.Position.Y ), this.m_Tex.Width, this.m_Tex.Height ), null, this.m_Color, MathHelper.ToRadians( this.m_Angle ), new Vector2( this.m_Tex.Width / 2, this.m_Tex.Height / 2 ), SpriteEffects.None, 1f );

            } else {
                spriteBatch.Draw( this.m_Tex, new Rectangle( ( int ) ( this.m_Pos.X - this.m_Stage.Camera.Position.X ), ( int ) ( this.m_Pos.Y - this.m_Stage.Camera.Position.Y ), this.m_Tex.Width, this.m_Tex.Height ), null, this.m_Color, MathHelper.ToRadians( this.m_Angle ), new Vector2( this.m_Tex.Width / 2, this.m_Tex.Height / 2 ), SpriteEffects.None, 1f );
            }
            return true;
        }
        public void Damaged(int damage) {
            if( !this.m_Intouchable ) {
                this.m_Health -= damage;
                this.EnableIntouchable( Consts.Values.Player.intouchable_time );
            }
        }

        private void Respawn() {
            this.m_Health = Consts.Values.Player.MaxHealth;
            this.m_Pos = Consts.Values.Player.start_Position;
            this.m_Move = Vector2.Zero;
            Consts.AudioManager.Play( this.wav_Spawn );
        }
        private void EnableIntouchable(double time) { this.m_Intouchable = true; this.m_intouchableTime = time; }
        private void DisableIntouchable() { this.m_Intouchable = false; }
        private void CheckDamaged() {
            IEnemy[] enemies = this.m_Stage.GetEnemies( this.m_Pos, Consts.Values.Player.size );

            if( enemies.Count() > 0 )
                this.Damaged( enemies[0].Damage );
        }
        private void ControlCooldowns(GameTime gameTime) {
            double time = gameTime.ElapsedGameTime.TotalMilliseconds;
            if( this.m_lastShoot > 0 ) { this.m_lastShoot -= time; }
            if( this.m_moveCoolDown > 0 ) { this.m_moveCoolDown -= time; }
            if( this.m_Intouchable ) {
                this.m_intouchableBlinkCoolDown -= time;
                this.m_intouchableTime -= time;
                if( this.m_intouchableTime < 0 )
                    this.DisableIntouchable();
            }
        }
        private void Shoot(GameTime gameTime) {
            this.m_lastShoot = this.m_shootCooldown;
            int ang = ( int ) this.Angle - 90;
            if( ang <= 0 )
                ang = 360 + ang;
            Vector2 target = Functions.CirclePoint( Vector2.Zero, Consts.Values.Bullet.Speed, 360, ang );
            m_Stage.AddEntity( new Bullet( Consts.Values.Bullet.Tex, this.m_Pos, target, this.m_Stage ) );
            Consts.AudioManager.Play( this.wav_Shoot );
        }
        private void Move(GameTime gameTime) {
            KeyboardState stan = Keyboard.GetState();
            bool moveX, moveY;
            moveX = moveY = false;
            float lastTime = ( float ) gameTime.ElapsedGameTime.TotalMilliseconds;

            if( stan.IsKeyDown( Keys.A ) ) {
                this.m_Move.X -= this.m_Acc * lastTime;
                moveX = true;
            } else {
                if( stan.IsKeyDown( Keys.D ) ) {
                    this.m_Move.X += this.m_Acc * lastTime;
                    moveX = true;
                }
            }
            if( stan.IsKeyDown( Keys.W ) ) {
                this.m_Move.Y -= this.m_Acc * lastTime;
                moveY = true;
            } else {
                if( stan.IsKeyDown( Keys.S ) ) {
                    this.m_Move.Y += this.m_Acc * lastTime;
                    moveY = true;
                }
            }
            if( this.m_Move.X < -this.m_MaxSpeed )
                this.m_Move.X = -this.m_MaxSpeed;
            if( this.m_Move.X > this.m_MaxSpeed )
                this.m_Move.X = this.m_MaxSpeed;
            if( this.m_Move.Y < -this.m_MaxSpeed )
                this.m_Move.Y = -this.m_MaxSpeed;
            if( this.m_Move.Y > this.m_MaxSpeed )
                this.m_Move.Y = this.m_MaxSpeed;

            if( !moveX ) {
                if( this.m_Move.X < 0 ) {
                    this.m_Move.X += this.m_Acc * lastTime * this.m_Drag;
                    if( this.m_Move.X > 0 )
                        this.m_Move.X = 0;
                } else {
                    if( this.m_Move.X > 0 ) {
                        this.m_Move.X -= this.m_Acc * lastTime * this.m_Drag;
                        if( this.m_Move.X < 0 )
                            this.m_Move.X = 0;
                    }
                }
            }
            if( !moveY ) {
                if( this.m_Move.Y < 0 ) {
                    this.m_Move.Y += this.m_Acc * lastTime * this.m_Drag;
                    if( this.m_Move.Y > 0 )
                        this.m_Move.Y = 0;
                } else {
                    if( this.m_Move.Y > 0 ) {
                        this.m_Move.Y -= this.m_Acc * lastTime * this.m_Drag;
                        if( this.m_Move.Y < 0 )
                            this.m_Move.Y = 0;
                    }
                }
            }
            if( this.m_Stage.AllowMove( this.m_Pos, this.m_Pos + this.m_Move * lastTime ) )
                this.m_Pos += this.m_Move * lastTime;

            //pilnowanie, by gracz nie wychodził poza mapę
            Vector2 pos = new Vector2( this.m_Pos.X, this.m_Pos.Y );
            int dir = 0;
            if( this.m_Stage.Camera.BackOnScreen( ref pos, ref dir ) )
                this.m_Pos = pos;
        }
        private void UpdateRotation() { this.m_Angle = Functions.Angle( m_Pos, this.m_Stage.Camera.GetMousePosition() ); }
        private void Killed() {
            if( this.m_Lives > 0 ) {
                --this.m_Lives;
                this.Respawn();
            } else {
                this.m_Stage.Defeat();
            }
        }
    }
}
