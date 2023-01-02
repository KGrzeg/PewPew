using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;

namespace PewPew {
    public class Bullet : IGameEntity {
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

        public Bullet(Texture2D tex, Vector2 pos, Vector2 move, Stage stage) {
            this.m_Stage = stage;
            this.m_Tex = tex;
            this.m_Pos = pos;
            this.m_Move = move;
            this.m_Color = Consts.Values.Bullet.Color;
            this.m_Angle = Functions.Angle( Vector2.Zero, move );
            this.m_Size = Consts.Values.Bullet.Size;
            this.m_Allow = true;
            this.m_Damage = Consts.Values.Bullet.Damage;
        }
        public void Update(GameTime gameTime) {
            this.Move( gameTime );
            this.CheckEnemyCollision();
        }
        public bool Draw(SpriteBatch spriteBatch, GameTime gameTime) { return true; }

        protected void CheckEnemyCollision() {
            IEnemy[] colis = this.m_Stage.GetEnemies( this.m_Pos, this.Size );
            if( colis.Count() > 0 ) {
                colis[0].Damaged( this.m_Damage );
                this.Remove();
            }
        }
        protected void Move(GameTime gameTime) {
            float lastTime = ( float ) gameTime.ElapsedGameTime.TotalMilliseconds;
            Vector2 pos = new Vector2();
            pos = this.m_Pos;
            int sum = 0;
            if( !this.m_Stage.Camera.BackOnScreen( ref pos, ref sum ) && this.m_Stage.AllowMove( this.m_Pos, this.m_Pos + m_Move * lastTime ) )
                this.m_Pos += m_Move * lastTime;
            else
                this.Remove();
        }
        protected void Remove() { this.m_Allow = false; }
        public bool Check() { return this.m_Allow; }
    }
}
