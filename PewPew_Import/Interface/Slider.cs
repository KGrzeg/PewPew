using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace PewPew.Interface {
    public class SliderHorizontal {
        private Texture2D m_Tex;
        private Vector2 m_Pos, m_StartPos;
        private Color m_Color;
        private int m_Width;
        private bool m_Catch;
        public bool Catched { get { return this.m_Catch; } }
        public float Value {
            get { return ( this.m_Pos.X - this.m_StartPos.X ) / ( float ) this.m_Width; }
            set { this.m_Pos.X = this.m_StartPos.X + ( int ) ( this.m_Width * value ); }
        }

        /// <summary>
        /// Tworzy instacje typu Slider
        /// </summary>
        /// <param name="tex">textura ruchomego elementu slidera</param>
        /// <param name="pos">pozycja startu slidera (lewy gorny rog)</param>
        /// <param name="width">szerokosc slidera</param>
        /// <param name="color">kolor slidera</param>
        public SliderHorizontal(Texture2D tex, Vector2 pos, int width, Color color) {
            this.m_Tex = tex;
            this.m_Color = color;
            this.m_Pos = this.m_StartPos = pos;
            this.m_Width = width;
            this.m_Pos.X += width / 2;
        }

        /// <summary>
        /// Tworzy instacje typu Slider
        /// </summary>
        /// <param name="tex">textura ruchomego elementu slidera</param>
        /// <param name="pos">pozycja startu slidera (lewy gorny rog)</param>
        /// <param name="width">szerokosc slidera</param>
        /// <param name="startval">startowa wartosc slidera</param>
        /// <param name="color">kolor slidera</param>
        public SliderHorizontal(Texture2D tex, Vector2 pos, int width, Color color, float startval)
            : this( tex, pos, width, color ) {
            this.m_Pos.X = this.m_StartPos.X + ( int ) ( width * startval );
        }
        public bool Update(GameTime gameTime) {
            if( this.m_Catch == true && Consts.MouseState.LeftButton == ButtonState.Released )
                this.m_Catch = false;

            Rectangle rec = Functions.TexToRec( this.m_Tex );
            rec.X = ( int ) this.m_Pos.X;
            rec.Y = ( int ) this.m_Pos.Y;
            if( Functions.Intersects( rec, Functions.GetMousePosition() ) ) {
                //mysz znajduje sie nad ruchomym elementem slidera
                if( this.m_Catch == false &&
                    Consts.PreviousMouseState.LeftButton == ButtonState.Released &&
                    Consts.MouseState.LeftButton == ButtonState.Pressed ) {
                    //dopiero nacisnieto przycisk
                    this.m_Catch = true;
                }
            }
            if( this.m_Catch ) {
                this.m_Pos.X = Consts.MouseState.X - this.m_Tex.Width / 2;
                if( this.m_Pos.X < this.m_StartPos.X )
                    this.m_Pos.X = this.m_StartPos.X;
                else
                    if( this.m_Pos.X > this.m_StartPos.X + this.m_Width )
                        this.m_Pos.X = this.m_StartPos.X + this.m_Width;
            }
            return this.m_Catch;
        }
        public void Draw(SpriteBatch spriteBatch, GameTime gameTime) {
            spriteBatch.Draw( this.m_Tex, this.m_Pos, this.m_Color );
        }
    }
    public class SliderVertical {
        private Texture2D m_Tex;
        private Vector2 m_Pos, m_StartPos;
        private Color m_Color;
        private int m_Height;
        private bool m_Catch;
        public bool Catched { get { return this.m_Catch; } }
        public float Value {
            get { return ( this.m_Pos.Y - this.m_StartPos.Y ) / ( float ) this.m_Height; }
            set { this.m_Pos.Y = this.m_StartPos.Y + ( int ) ( this.m_Height * value ); }
        }

        public SliderVertical(Texture2D tex, Vector2 pos, int height, Color color) {
            this.m_Tex = tex;
            this.m_Color = color;
            this.m_Pos = this.m_StartPos = pos;
            this.m_Height = height;
            this.m_Pos.Y += height / 2;
        }

        public SliderVertical(Texture2D tex, Vector2 pos, int height, Color color, float startval)
            : this( tex, pos, height, color ) {
            this.m_Pos.Y = this.m_StartPos.Y + ( int ) ( height * startval );
        }
        public bool Update(GameTime gameTime) {
            if( this.m_Catch == true && Consts.MouseState.LeftButton == ButtonState.Released )
                this.m_Catch = false;

            Rectangle rec = Functions.TexToRec( this.m_Tex );
            rec.X = ( int ) this.m_Pos.X;
            rec.Y = ( int ) this.m_Pos.Y;
            if( Functions.Intersects( rec, Functions.GetMousePosition() ) ) {
                //mysz znajduje sie nad ruchomym elementem slidera
                if( this.m_Catch == false &&
                    Consts.PreviousMouseState.LeftButton == ButtonState.Released &&
                    Consts.MouseState.LeftButton == ButtonState.Pressed ) {
                    //dopiero nacisnieto przycisk
                    this.m_Catch = true;
                }
            }
            if( this.m_Catch ) {
                this.m_Pos.Y = Consts.MouseState.Y - this.m_Tex.Height / 2;
                if( this.m_Pos.Y < this.m_StartPos.Y )
                    this.m_Pos.Y = this.m_StartPos.Y;
                else
                    if( this.m_Pos.Y > this.m_StartPos.Y + this.m_Height )
                        this.m_Pos.Y = this.m_StartPos.Y + this.m_Height;
            }
            return this.m_Catch;
        }
        public void Draw(SpriteBatch spriteBatch, GameTime gameTime) {
            spriteBatch.Draw( this.m_Tex, this.m_Pos, this.m_Color );
        }
    }
}


