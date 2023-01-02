using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PewPew.Menus {
    class StagesMenu : Menu{
        private static readonly Color
            col_Green = new Color( 0x80, 0xff, 0x00 ),
            col_Red = new Color( 0xff, 0x25, 0x25 ),
            col_Gray = new Color( 0x97, 0x97, 0x97 );
        private static readonly Vector2
            vec_back = new Vector2( 400, 0 ),
            vec_start = new Vector2( 600, 0 ),
            vec_slider = new Vector2( 0, 128 ),
            vec_stage = new Vector2( 12, 128 ),
            vec_stage_offset = new Vector2( 0, 50 ),
            vec_passed = new Vector2( 290, 0 ),
            vec_number = new Vector2( 8, 8 ),
            vec_name = new Vector2( 191, 15 ),
            vec_main_name = new Vector2( 564, 131 ),
            vec_image = new Vector2( 335, 183 ),
            vec_money = new Vector2( 788, 232 ),
            vec_exp = new Vector2( 788, 250 ),
            vec_desc = new Vector2( 334, 285 );
        private static readonly Rectangle
            rec_menu = new Rectangle( 12, 128, 306, 350 );
        private const int
            slider_height = 317;

        private Interface.Button but_Back, but_Start;
        private Interface.SliderVertical slider;
        private Interface.BarHorizontal bar;
        private float m_Value,
                      m_ValueLast;
        private Vector2 m_SliderOffset_Last,
                        m_SliderOffset;
        private Interface.Button[] but_Stages;
        private int m_NumOfStages,
                    m_Height;

        public StagesMenu() {
            this.but_Back = new Interface.Button( vec_back, Consts.Textures.Menu.Stages.But_Back, Consts.Textures.Menu.Stages.But_Back_f );
            this.but_Start = new Interface.Button( vec_start, Consts.Textures.Menu.Stages.But_Start, Consts.Textures.Menu.Stages.But_Start_f );
            this.slider = new Interface.SliderVertical( Consts.Textures.Menu.Slider_page, vec_slider, slider_height, Color.White, 0f );
            this.bar = new Interface.BarHorizontal( Consts.Values.Menu.Ships.vec_bar_User_Experience, Consts.Values.Menu.Ships.tex_bar_User_Exp,
                                                    Consts.Values.Menu.Ships.col_Experience, ( float ) User.LevelPercent / 100 );
            this.m_NumOfStages = Consts.Values.Stages.Array.Count();
            this.but_Stages = new Interface.Button[this.m_NumOfStages];
            for( int i=0; i < this.m_NumOfStages; ++i ) {
                Vector2 offset = new Vector2() + ( i * vec_stage_offset );
                this.but_Stages[i] = new Interface.Button( vec_stage + offset, Consts.Textures.Menu.Stages.But_Stage, Consts.Textures.Menu.Stages.But_Stage_f );
                this.but_Stages[i].Tex_lock = Consts.Textures.Menu.Stages.But_Stage_l;
                this.but_Stages[i].Tex_special = Consts.Textures.Menu.Stages.But_Stage;
                if( User.Unlocked( i ) == false )
                    this.but_Stages[i].State = Interface.MyButtonState.LOCK;
                if( User.Stage == i )
                    this.but_Stages[i].State = Interface.MyButtonState.SPECIAL;
            }
            this.m_Height = ( int ) ( this.m_NumOfStages * vec_stage_offset.Y );
        }

        public override MenuState Update(GameTime gameTime) {
            if( this.but_Back.Update( gameTime ) )
                return MenuState.SHIPS;
            if( this.but_Start.Update( gameTime ) )
                return MenuState.PLAY;
            this.UpdateSlider( gameTime );
            if( this.slider.Catched == false ) {
                this.slider.Value -= ( Consts.MouseState.ScrollWheelValue - Consts.PreviousMouseState.ScrollWheelValue ) / 1000f;
                if( this.slider.Value > 1 )
                    this.slider.Value = 1;
                if( this.slider.Value < 0 )
                    this.slider.Value = 0;
            }
            for( int i = 0; i < Consts.Values.Stages.Array.Count(); ++i ) {
                if( this.InViewRectangle(this.but_Stages[i].Area) && this.but_Stages[i].Update( gameTime ) ) {
                    if( this.InViewRectangle(this.but_Stages[User.Stage].Area) == true )
                        this.but_Stages[User.Stage].State = Interface.MyButtonState.NORMAL;
                    else
                        this.but_Stages[User.Stage].State = Interface.MyButtonState.LOCK;
                    User.Stage = i;
                    this.but_Stages[i].State = Interface.MyButtonState.SPECIAL;
                }
            }
            if( Functions.KeyboardClick( Microsoft.Xna.Framework.Input.Keys.Up ) && User.Stage > 0 ) {
                if( this.InViewRectangle( this.but_Stages[User.Stage].Area ) == true )
                    this.but_Stages[User.Stage].State = Interface.MyButtonState.NORMAL;
                else
                    this.but_Stages[User.Stage].State = Interface.MyButtonState.LOCK;
                this.but_Stages[--User.Stage].State = Interface.MyButtonState.SPECIAL;
            } else {
                if( Functions.KeyboardClick( Microsoft.Xna.Framework.Input.Keys.Down ) && User.Unlocked( User.Stage + 1 ) ) {
                    if( this.InViewRectangle( this.but_Stages[User.Stage].Area ) == true )
                        this.but_Stages[User.Stage].State = Interface.MyButtonState.NORMAL;
                    else
                        this.but_Stages[User.Stage].State = Interface.MyButtonState.LOCK;
                    this.but_Stages[++User.Stage].State = Interface.MyButtonState.SPECIAL;
                }

            }
            return MenuState.STAGES;
        }
        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime) {
            spriteBatch.Draw( Consts.Textures.Menu.Stages.Background, Vector2.Zero, Color.White );
            this.but_Back.Draw( spriteBatch );
            this.but_Start.Draw( spriteBatch );
            this.slider.Draw( spriteBatch, gameTime );
            this.DrawMenu( spriteBatch );
            this.DrawInfo( spriteBatch );
            spriteBatch.Draw( Consts.Values.Menu.Ships.tex_interface_User, Consts.Values.Menu.Ships.vec_interface_User, Color.White );
            spriteBatch.DrawString( Consts.Values.Menu.Ships.font_Nick, Settings.Nick, Consts.Values.Menu.Ships.vec_string_Nick, Consts.Values.Menu.Ships.col_string_Nick );
            spriteBatch.DrawString( Consts.Values.Menu.Ships.font_User_Level, ( User.Level > 9 ? User.Level.ToString() : "0" + User.Level.ToString() ), Consts.Values.Menu.Ships.vec_string_User_Level, Consts.Values.Menu.Ships.col_Experience );
            spriteBatch.DrawString( Consts.Values.Menu.Ships.font_Level_Percent, User.LevelPercent.ToString() + "%", Consts.Values.Menu.Ships.vec_string_User_Level_Percent, Consts.Values.Menu.Ships.col_Experience );
            spriteBatch.DrawString( Consts.Values.Menu.Ships.font_Money, User.Money.ToString() + "$", Functions.RightAlign( Consts.Fonts.UserMoney, User.Money.ToString() + "$", Consts.Values.Menu.Ships.vec_string_Money ), Consts.Values.Menu.Ships.col_Money );
            this.bar.Draw( spriteBatch, gameTime );
        }
        private void DrawMenu(SpriteBatch spriteBatch) {
            for( int i = 0; i < Consts.Values.Stages.Array.Count(); ++i ) {
                this.but_Stages[i].Draw( spriteBatch );
                Color col;
                if( this.but_Stages[i].State == Interface.MyButtonState.SPECIAL ) {
                    col = col_Green;
                } else {
                    if( this.but_Stages[i].State == Interface.MyButtonState.LOCK ) {
                        col = col_Red;
                    } else {
                        col = col_Gray;
                    }
                }
                Functions.DrawStringAlignMiddle( spriteBatch, Consts.Fonts.StageDesc, Consts.Values.Stages.Array[i].Name, this.but_Stages[i].Position + vec_name, col );
                string num = ( i < 10 ? "0" : "" ) + ( i + 1 ).ToString();
                spriteBatch.DrawString( Consts.Fonts.StageNum, num, this.but_Stages[i].Position + vec_number, col );
                if( User.PassedStages[i] == true )
                    spriteBatch.Draw( Consts.Textures.Menu.Stages.Interface_Passed, this.but_Stages[i].Position + vec_passed, Color.White );
            }
            spriteBatch.Draw( Consts.Textures.Menu.Stages.Mask, Vector2.Zero, Color.White );
        }
        private void DrawInfo(SpriteBatch spriteBatch) {
            spriteBatch.Draw( Consts.Values.Stages.Array[User.Stage].Image, vec_image, Color.White );
            Functions.DrawStringAlignMiddle( spriteBatch, Consts.Fonts.StageName, Consts.Values.Stages.Array[User.Stage].Name, vec_main_name, col_Green );
            Functions.DrawStringAlignRight( spriteBatch, Consts.Fonts.StageAward, Consts.Values.Stages.Array[User.Stage].award_Money.ToString() + "$", vec_money, col_Green );
            Functions.DrawStringAlignRight( spriteBatch, Consts.Fonts.StageAward, Consts.Values.Stages.Array[User.Stage].award_Exp.ToString() + "pkt", vec_exp, col_Green );
            spriteBatch.DrawString( Consts.Fonts.StageDesc, Consts.Values.Stages.Array[User.Stage].Description, vec_desc, col_Green );
        }
        private bool InViewRectangle(Rectangle rec) {
            if( rec.Bottom < rec_menu.Top )
                return false;
            return true;
        }
        private void UpdateButtonsState() {
            for( int i = 0; i < this.m_NumOfStages; ++i ) {
                if( this.InViewRectangle( this.but_Stages[i].Area ) == true ) {
                    if( this.but_Stages[i].State == Interface.MyButtonState.LOCK && User.Unlocked(i) == true)
                        this.but_Stages[i].State = Interface.MyButtonState.NORMAL;
                } else {
                    if( this.but_Stages[i].State != Interface.MyButtonState.LOCK &&
                        this.but_Stages[i].State != Interface.MyButtonState.SPECIAL )
                        this.but_Stages[i].State = Interface.MyButtonState.LOCK;
                }
            }
        }
        private void UpdateSlider(GameTime gameTime) {
            this.slider.Update( gameTime );
            this.m_ValueLast = this.m_Value;
            this.m_Value = slider.Value;
            if( this.m_ValueLast != this.m_Value ) {
                int cutHeight = this.m_Height - rec_menu.Height;
                this.m_SliderOffset_Last = this.m_SliderOffset;
                this.m_SliderOffset = new Vector2( 0, cutHeight * this.m_Value );
                Vector2 diff = this.m_SliderOffset_Last - this.m_SliderOffset;
                for( int i = 0; i < this.m_NumOfStages; ++i ) {
                    this.but_Stages[i].Position += diff;
                }
                this.UpdateButtonsState();
            }
        }
    }
}
