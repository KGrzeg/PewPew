using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PewPew.Interface {
    public class BarHorizontal {
        public Vector2 Position;
        private Texture2D m_Tex;
        private Color m_Color;
        private int m_Width;

        public int Width { get { return this.m_Width; } }
        public float Value { set { this.m_Width = ( int ) ( this.m_Tex.Width * value ); } }
        public BarHorizontal(Vector2 pos, Texture2D tex, Color color) {
            this.Position = pos;
            this.m_Tex = tex;
            this.m_Color = color;
        }
        public BarHorizontal(Vector2 pos, Texture2D tex, Color color, float value)
            : this( pos, tex, color ) {
            this.Value = value;
        }
        public void Draw(SpriteBatch spriteBatch, GameTime gameTime) {
            //spriteBatch.Draw( Consts.Textures.Dot, new Rectangle( ( int ) this.Position.X, ( int ) this.Position.Y, this.m_Tex.Width, this.m_Tex.Height ), this.m_Color );
            if( this.m_Width > 0 ) {
                Rectangle destRec = new Rectangle( ( int ) this.Position.X, ( int ) this.Position.Y, this.m_Width, this.m_Tex.Height );
                Rectangle locRec = new Rectangle( 0, 0, this.m_Width, this.m_Tex.Height );
                spriteBatch.Draw( this.m_Tex, destRec, locRec, this.m_Color );
            }
            //spriteBatch.DrawString( Consts.Fonts.font1, this.Position.ToString(), this.Position, Color.White );
        }
    }
}
