using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace PewPew.Interface {
    class InputText {
        private const double blinkInterval = 300;
        private Rectangle m_Area;
        private Color m_Color;
        private SpriteFont m_Font;
        private string m_Data;
        private int m_MaxChar;
        private bool m_Active,
                     m_BlinkState;
        private double m_BlinkTime;

        public string Data { get { return this.m_Data; } }
        public bool Active { get { return this.m_Active; } }

        public InputText(Rectangle area, SpriteFont font, int maxChar, Color color) {
            this.m_Area = area;
            this.m_Font = font;
            this.m_MaxChar = maxChar;
            this.m_Color = color;
            this.m_Data = "";
            this.m_Active = false;
            this.m_BlinkTime = 0;
        }
        public InputText(Rectangle area, SpriteFont font, int maxChar, Color color, string startData)
            : this( area, font, maxChar, color ) {
            this.m_Data = startData;
        }

        public void Update(GameTime gameTime) {
            if( Functions.Intersects( this.m_Area, Functions.GetMousePosition() ) == true ) {
                if( this.m_Active == false ) {
                    if( Functions.MouseLeftClick() ) {
                        this.m_Active = true;
                    }
                }
            } else {    //myszka poza obszarem inputa
                if( this.m_Active == true && Functions.MouseLeftClick() ) {
                    this.m_Active = false;
                }
            }

            if( this.m_Active ) {
                this.m_BlinkTime -= gameTime.ElapsedGameTime.TotalMilliseconds;
                if( this.m_BlinkTime <= 0 ) {
                    this.m_BlinkState = !this.m_BlinkState;
                    this.m_BlinkTime = blinkInterval;
                }
                Keys[] keys = Consts.KeyboardState.GetPressedKeys();
                foreach( Keys key in keys ) {
                    bool clicked = false;
                    if( Functions.KeyboardClick( key ) ) {
                        if( Consts.KeyboardState.IsKeyDown( Keys.Back ) ) {    //backspace powinien być sprawdzony PRZED resztą klawiszy
                            if( this.m_Data.Length > 0 ) {
                                this.m_Data = this.m_Data.Remove( this.m_Data.Length - 1, 1 );
                                break;
                            }
                        } else {
                            switch( key ) {
                                case Keys.Escape: {
                                        this.m_Active = false;
                                        break;
                                    }
                                case Keys.LeftShift: {
                                        break;
                                    }
                                case Keys.RightShift: {
                                        break;
                                    }
                                default: {
                                        if( key != Keys.LeftShift && key != Keys.RightShift ) {
                                            if( this.m_Data.Length < this.m_MaxChar ) {
                                                int nkey = 0;
                                                bool shift = false;
                                                if( Consts.KeyboardState.IsKeyDown( Keys.LeftShift ) || Consts.KeyboardState.IsKeyDown( Keys.RightShift ) )
                                                    shift = true;
                                                if( shift ) {
                                                    nkey = ( int ) key;
                                                    if( nkey < 65 || nkey > 90 )
                                                        break;
                                                } else {
                                                    nkey = ( int ) key + 32;
                                                    if( nkey < 97 || nkey > 122 )
                                                        break;
                                                }
                                                this.m_Data += ( ( char ) nkey ).ToString();
                                                clicked = true;
                                            }
                                        }
                                        break;
                                    }
                            }
                        }
                    }
                    if( clicked )
                        break;
                }
            }
        }
        public void Draw(SpriteBatch spriteBatch, GameTime gameTime) {
            spriteBatch.DrawString( this.m_Font, this.m_Data + ( this.m_Active && this.m_BlinkState ? "|" : "" ), new Vector2( this.m_Area.X, this.m_Area.Y ), this.m_Color );
        }
    }
}
