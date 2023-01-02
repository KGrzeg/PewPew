using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace PewPew.Interface {
    public enum MyButtonState {
        NORMAL = 0,
        LOCK,   //=1
        FOCUS,  //=2
        SPECIAL //=3
    }
    public class Button {
        private Texture2D m_tex_normal;
        private Texture2D m_tex_focus;
        private Texture2D m_tex_lock;
        private Texture2D m_tex_special;
        private bool m_focused;

        public Vector2 Position;
        public Texture2D Tex_lock { get { return this.m_tex_lock; } set { if( value.Width != this.m_tex_normal.Width || value.Height != this.m_tex_normal.Height ) throw new ArgumentException( "Rozne wymiary tekstur" ); else m_tex_lock = value; } }
        public Texture2D Tex_special { get { return this.m_tex_special; } set { if( value.Width != this.m_tex_normal.Width || value.Height != this.m_tex_normal.Height ) throw new ArgumentException( "Rozne wymiary tekstur" ); else m_tex_special = value; } }
        public bool Focus { get { return this.m_focused; } }
        public MyButtonState State;
        public Rectangle Area { get { return new Rectangle( ( int ) this.Position.X, ( int ) this.Position.Y, this.m_tex_normal.Width, this.m_tex_normal.Height ); } }

        private Button() {
            this.State = MyButtonState.NORMAL;
            this.m_focused = false;
        }
        /// <summary>
        /// Tworzy obiekt guzika
        /// </summary>
        /// <param name="position">Pozycja guzika na ekranie</param>
        /// <param name="normal">Tekstura guzika</param>
        /// <param name="focus">Tekstura guzika, gdy wisi nad nim kursor</param>
        public Button(Vector2 position, Texture2D normal, Texture2D focus)
            : this() {
            this.Position = position;
            this.m_tex_normal = normal;
            this.m_tex_focus = focus;

            if( normal.Width != focus.Width || normal.Height != focus.Height )
                throw new ArgumentException( "Rozne wymiary tekstur", "focus" );
        }
        public void Draw(SpriteBatch spriteBatch) {
            switch( State ) {
                case MyButtonState.NORMAL:
                    spriteBatch.Draw( m_tex_normal, Position, Color.White );
                    break;
                case MyButtonState.LOCK:
                    if( this.m_tex_lock != null )
                        spriteBatch.Draw( m_tex_lock, Position, Color.White );
                    else
                        throw new ArgumentNullException( "m_tex_lock" );
                    break;
                case MyButtonState.FOCUS:
                    spriteBatch.Draw( m_tex_focus, Position, Color.White );
                    break;
                case MyButtonState.SPECIAL:
                    if( m_tex_special != null )
                        spriteBatch.Draw( m_tex_special, Position, Color.White );
                    else
                        throw new ArgumentNullException( "m_tex_special" );
                    break;
            }
        }
        public bool Update(GameTime gameTime) //zwraca true tylko, jesli przycisk został naciśnięty
        {
            if( State == MyButtonState.SPECIAL || State == MyButtonState.LOCK )
                return false;

            //myszka poza oknem gry
            if( !Functions.Intersects( new Rectangle( 0, 0, Consts.GraphicsDevice.Viewport.Width, Consts.GraphicsDevice.Viewport.Height ), Functions.GetMousePosition() ) )
                return false;

            //czy myszka znajduje się nad przyciskiem
            Rectangle rec = Functions.TexToRec( this.m_tex_normal );
            rec.X = ( int ) this.Position.X;
            rec.Y = ( int ) this.Position.Y;
            if( Functions.Intersects( rec, Functions.GetMousePosition() ) ) {
                this.m_focused = true;
                State = MyButtonState.FOCUS;
            } else {
                this.m_focused = false;
                State = MyButtonState.NORMAL;
            }

            if( Consts.MouseState.LeftButton == ButtonState.Pressed &&
                Consts.PreviousMouseState.LeftButton == ButtonState.Released &&
                this.m_focused ) {
                return true;
            }
            return false;
        }
    }
}
