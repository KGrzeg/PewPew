using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PewPew {
    public class PopUp : ICameraDrawable {
        protected Camera m_Camera;
        protected SpriteFont m_Font;
        protected string m_Text;
        protected Color m_Color;
        protected Vector2 m_Pos;
        protected float m_Direction;

        protected bool m_Alive;
        protected float m_Speed;
        protected int m_Distance;
        protected Vector2 m_start_position;

        public Texture2D Texture { get { return null; } }
        public Vector2 Position { get { return this.m_Pos; } }
        public Color Color { get { return this.m_Color; } }
        public float Angle { get { return 1; } }
        public bool SpecialDraw { get { return true; } }

        public PopUp(Camera camera, SpriteFont font, string text, Color color, Vector2 position, double time, int distance, int direction) {
            this.m_Camera = camera;
            this.m_Font = font;
            this.m_Text = text;
            this.m_Color = color;
            this.m_Pos = this.m_start_position = position;
            this.m_Distance = distance;
            this.m_Direction = direction;  //90=dół
            this.m_Speed = ( float ) ( distance / time );

            this.m_Alive = true;
        }

        public void Update(GameTime gameTime) {
            float distance = Vector2.Distance( this.m_start_position, this.m_Pos );
            if( distance > this.m_Distance )
                this.m_Alive = false;

            //przesunięcie w danym kierunku
            float droga = ( float ) ( m_Speed * gameTime.ElapsedGameTime.TotalMilliseconds );
            this.m_Pos = Functions.CirclePoint( this.m_Pos, droga, 360, ( int ) this.m_Direction );

            //zanikanie
            float progress = distance > 0 ? distance / this.m_Distance : 0;
            this.m_Color.A = ( byte ) ( progress * 255 );
        }
        public bool Draw(SpriteBatch spriteBatch, GameTime gameTime) {
            this.Update( gameTime );
            spriteBatch.DrawString( this.m_Font, this.m_Text, this.m_Pos - this.m_Camera.Position, this.m_Color );
            return this.m_Alive;
        }
    }
}
