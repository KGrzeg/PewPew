using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PewPew.Menus {
    public class ShipsMenu : Menu {
        private const int freeExp = 10;
        private Interface.Button but_Help, but_Back, but_Next;
        private Interface.BarHorizontal bar_User_Exp;

        //potrzebne do rysowania menu statkow
        private Interface.SliderVertical slider;
        private Interface.BarHorizontal[] bar_ships_Health,
                                bar_ships_Speed,
                                bar_ships_Agility,
                                bar_ships_UpHealth,
                                bar_ships_UpSpeed,
                                bar_ships_UpAgility,
                                bar_ships_Experience;
        private Interface.Button[] but_Choose, but_Buy,
                         but_Upgrade_Speed,
                         but_Upgrade_Agility,
                         but_Upgrade_Health;
        private int m_height, //wysokość listy statkow
                    m_NumOfShips,
                    m_helpStep;//krok w tutorialu
        private float m_Value,
                      m_ValueLast;
        private Vector2 m_SliderOffset_Last,
                        m_SliderOffset;
        private bool m_GiveExp = false;

        public ShipsMenu() {
            base.m_Tex = Consts.Values.Menu.Ships.tex_Background;

            this.but_Help = new Interface.Button( Consts.Values.Menu.Ships.vec_but_Help, Consts.Values.Menu.Ships.tex_but_Help, Consts.Values.Menu.Ships.tex_but_Help_f );
            this.but_Back = new Interface.Button( Consts.Values.Menu.Ships.vec_but_Back, Consts.Values.Menu.Ships.tex_but_Back, Consts.Values.Menu.Ships.tex_but_Back_f );
            this.but_Next = new Interface.Button( Consts.Values.Menu.Ships.vec_but_Next, Consts.Values.Menu.Ships.tex_but_Next, Consts.Values.Menu.Ships.tex_but_Next_f );
            this.bar_User_Exp = new Interface.BarHorizontal( Consts.Values.Menu.Ships.vec_bar_User_Experience, Consts.Values.Menu.Ships.tex_bar_User_Exp,
                                                   Consts.Values.Menu.Ships.col_Experience, ( float ) User.LevelPercent / 100 );
            this.slider = new Interface.SliderVertical( Consts.Values.Menu.Ships.tex_slider, Consts.Values.Menu.Ships.vec_slider, Consts.Values.Menu.Ships.slider_height, Color.White, 0f );
            this.m_NumOfShips = Consts.Values.Ships.Array.Count();
            this.m_height = ( int ) ( Consts.Values.Menu.Ships.tex_interface_Ship.Height + Consts.Values.Menu.Ships.vec_interface_Ship_Offset.Y ) * this.m_NumOfShips;
            this.bar_ships_Health = new Interface.BarHorizontal[this.m_NumOfShips];
            this.bar_ships_Speed = new Interface.BarHorizontal[this.m_NumOfShips];
            this.bar_ships_Agility = new Interface.BarHorizontal[this.m_NumOfShips];
            this.bar_ships_UpHealth = new Interface.BarHorizontal[this.m_NumOfShips];
            this.bar_ships_UpSpeed = new Interface.BarHorizontal[this.m_NumOfShips];
            this.bar_ships_UpAgility = new Interface.BarHorizontal[this.m_NumOfShips];
            this.bar_ships_Experience = new Interface.BarHorizontal[this.m_NumOfShips];
            this.but_Choose = new Interface.Button[this.m_NumOfShips];
            this.but_Buy = new Interface.Button[this.m_NumOfShips];
            this.but_Upgrade_Speed = new Interface.Button[this.m_NumOfShips];
            this.but_Upgrade_Agility = new Interface.Button[this.m_NumOfShips];
            this.but_Upgrade_Health = new Interface.Button[this.m_NumOfShips];

            Texture2D tex_cut = Consts.Values.Menu.Ships.tex_bar_Stats;
            Vector2 offset;
            for( int i = 0; i < m_NumOfShips; ++i ) {
                offset = new Vector2( Consts.Values.Menu.Ships.vec_interface_Ship_Offset.X, ( Consts.Values.Menu.Ships.tex_interface_Ship.Height + Consts.Values.Menu.Ships.vec_interface_Ship_Offset.Y ) * i );
                //okienko po lewej
                this.but_Choose[i] = new Interface.Button( Consts.Values.Menu.Ships.vec_but_ChooseBuy + offset, Consts.Values.Menu.Ships.tex_but_Choose, Consts.Values.Menu.Ships.tex_but_Choose_f );
                this.but_Choose[i].Tex_lock = Consts.Textures.Menu.But_Ships_Choose_l;
                this.but_Buy[i] = new Interface.Button( Consts.Values.Menu.Ships.vec_but_ChooseBuy + offset, Consts.Values.Menu.Ships.tex_but_Buy, Consts.Values.Menu.Ships.tex_but_Buy_f );
                this.but_Buy[i].Tex_lock = Consts.Values.Menu.Ships.tex_but_Buy_l;
                if( User.Money < User.Ships[i].Cost )
                    this.but_Buy[i].State = Interface.MyButtonState.LOCK;

                //statystyki
                this.but_Upgrade_Speed[i] = new Interface.Button( Consts.Values.Menu.Ships.vec_but_Upgrade_Speed + offset, Consts.Textures.Menu.But_Ships_Upgrade, Consts.Textures.Menu.But_Ships_Upgrade_f );
                this.but_Upgrade_Agility[i] = new Interface.Button( Consts.Values.Menu.Ships.vec_but_Upgrade_Agility + offset, Consts.Textures.Menu.But_Ships_Upgrade, Consts.Textures.Menu.But_Ships_Upgrade_f );
                this.but_Upgrade_Health[i] = new Interface.Button( Consts.Values.Menu.Ships.vec_but_Upgrade_Health + offset, Consts.Textures.Menu.But_Ships_Upgrade, Consts.Textures.Menu.But_Ships_Upgrade_f );
                this.but_Upgrade_Speed[i].Tex_lock = this.but_Upgrade_Agility[i].Tex_lock = this.but_Upgrade_Health[i].Tex_lock = Consts.Textures.Menu.But_Ships_Upgrade_l;

                this.bar_ships_UpSpeed[i] = new Interface.BarHorizontal( Consts.Values.Menu.Ships.vec_bar_Stats_Speed + offset, tex_cut, Consts.Values.Menu.Ships.col_bar_Stats_Upgrade );
                this.bar_ships_UpAgility[i] = new Interface.BarHorizontal( Consts.Values.Menu.Ships.vec_bar_Stats_Agility + offset, tex_cut, Consts.Values.Menu.Ships.col_bar_Stats_Upgrade );
                this.bar_ships_UpHealth[i] = new Interface.BarHorizontal( Consts.Values.Menu.Ships.vec_bar_Stats_Health + offset, tex_cut, Consts.Values.Menu.Ships.col_bar_Stats_Upgrade );
                this.bar_ships_Speed[i] = new Interface.BarHorizontal( Consts.Values.Menu.Ships.vec_bar_Stats_Speed + offset, tex_cut, Consts.Values.Menu.Ships.col_bar_Stats_Speed );
                this.bar_ships_Agility[i] = new Interface.BarHorizontal( Consts.Values.Menu.Ships.vec_bar_Stats_Agility + offset, tex_cut, Consts.Values.Menu.Ships.col_bar_Stats_Agility );
                this.bar_ships_Health[i] = new Interface.BarHorizontal( Consts.Values.Menu.Ships.vec_bar_Stats_Health + offset, tex_cut, Consts.Values.Menu.Ships.col_bar_Stats_Health );
                this.bar_ships_Experience[i] = new Interface.BarHorizontal( Consts.Values.Menu.Ships.vec_bar_Ship_Experience, Consts.Values.Menu.Ships.tex_bar_Ship_Exp,
                                                              Consts.Values.Menu.Ships.col_bar_Ship_Experience, ( float ) User.Ships[i].LevelPercent / 100 );
            }
            this.UpdateBars();
            this.UpdateButtonsStates();
            //zrezygnowałem z przepisywania wszystkich tekstur poraz kolejny
            //dane zostaną wybrane prosto ze statycznej klasy Consts
        }

        public override MenuState Update(GameTime gameTime) {
            if( this.m_helpStep == 0 ) {
                if( this.but_Help.Update( gameTime ) ) {
                    this.m_helpStep = 1;
                    this.slider.Value = 0f;
                    if( User.Experience == 0 )
                        this.m_GiveExp = true;
                }
                if( this.but_Back.Update( gameTime ) )
                    return MenuState.START;
                if( this.but_Next.Update( gameTime ) )
                    return MenuState.STAGES;

                this.UpdateSlider( gameTime );
                if( this.slider.Catched == false ) {
                    this.slider.Value -= ( Consts.MouseState.ScrollWheelValue - Consts.PreviousMouseState.ScrollWheelValue ) / 1000f;
                    if( this.slider.Value > 1 )
                        this.slider.Value = 1;
                    if( this.slider.Value < 0 )
                        this.slider.Value = 0;
                }

                for( int i = 0; i < this.m_NumOfShips; ++i ) {
                    if( this.but_Upgrade_Speed[i].Update( gameTime ) ) {
                        User.Ships[i].UpgradeSpeed();
                        this.UpdateBars();
                        this.UpdateButtonsStates();
                    }
                    if( this.but_Upgrade_Agility[i].Update( gameTime ) ) {
                        User.Ships[i].UpgradeAgility();
                        this.UpdateBars();
                        this.UpdateButtonsStates();
                    }
                    if( this.but_Upgrade_Health[i].Update( gameTime ) ) {
                        User.Ships[i].UpgradeHealth();
                        this.UpdateBars();
                        this.UpdateButtonsStates();
                    }
                    if( User.BuyedShips[i] == true ) {
                        if( this.but_Choose[i].Update( gameTime ) ) {
                            User.Ship = i;
                            this.UpdateButtonsStates();
                        }
                    } else {
                        if( this.but_Buy[i].Update( gameTime ) ) {
                            this.BuyShip( i );
                            this.UpdateButtonsStates();
                        }
                    }
                }
            } else {    //this.m_helpStep != 0
                if( Functions.MouseLeftClick() ) {
                    if( ++this.m_helpStep == 4 &&
                        this.m_GiveExp == true ) {
                            User.Experience = freeExp;
                    }
                }
                if( this.m_helpStep >= 5 ||
                    ( this.m_helpStep == 4 && this.m_GiveExp == false ) ) {
                    this.m_helpStep = 0;
                    this.m_GiveExp = false;
                }
            }
            return MenuState.SHIPS;
        }
        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime) {
            //tło
            spriteBatch.Draw( this.m_Tex, Consts.GraphicsDevice.Viewport.TitleSafeArea, Color.White );

            for( int i=0; i < this.m_NumOfShips; ++i ) {
                Vector2 offset = new Vector2( 0, ( Consts.Values.Menu.Ships.vec_interface_Ship_Offset.Y + Consts.Values.Menu.Ships.tex_interface_Ship.Height ) * i ) - this.m_SliderOffset;
                spriteBatch.Draw( Consts.Values.Menu.Ships.tex_interface_Ship, Consts.Values.Menu.Ships.vec_interface_Ship + offset, Color.White );
                if( User.Ship == i )
                    spriteBatch.Draw( User.Ships[i].Texture, Functions.TexMid( User.Ships[i].Texture, Consts.Values.Menu.Ships.vec_image_mid ) + offset, Settings.Color_Player );
                else
                    spriteBatch.Draw( User.Ships[i].Texture, Functions.TexMid( User.Ships[i].Texture, Consts.Values.Menu.Ships.vec_image_mid ) + offset, Color.White );

                this.but_Upgrade_Speed[i].Draw( spriteBatch );
                this.but_Upgrade_Agility[i].Draw( spriteBatch );
                this.but_Upgrade_Health[i].Draw( spriteBatch );

                if( User.BuyedShips[i] == true ) {    //ten statek jest już zakupiony
                    spriteBatch.DrawString( Consts.Values.Menu.Ships.font_Ship_Name, User.Ships[i].Name, Consts.Values.Menu.Ships.vec_string_Ship_Name + offset, Consts.Values.Menu.Ships.col_string_Avaible );
                    spriteBatch.DrawString( Consts.Values.Menu.Ships.font_Ship_Level, User.Ships[i].Level.ToString(), Consts.Values.Menu.Ships.vec_string_Ship_Level + offset, Consts.Values.Menu.Ships.col_Experience );
                    #region Ulepszanie_Cena_i_Level
                    Color col = Consts.Values.Menu.Ships.col_string_Avaible;
                    if( User.Ships[i].CanUpgradeSpeed() == false )
                        col = Consts.Values.Menu.Ships.col_string_NotAvaible;
                    spriteBatch.DrawString( Consts.Fonts.ShipLevel, User.Ships[i].NextLevelSpeed.ToString(), Consts.Values.Menu.Ships.vec_string_Stats_Speed_Level + offset, col );
                    col = Consts.Values.Menu.Ships.col_string_Avaible;
                    if( User.Money < User.Ships[i].NextCostSpeed )
                        col = Consts.Values.Menu.Ships.col_string_NotAvaible;
                    spriteBatch.DrawString( Consts.Fonts.StatsCost, User.Ships[i].NextCostSpeed.ToString() + "$",
                                            Functions.RightAlign( Consts.Fonts.StatsCost, User.Ships[i].NextCostSpeed.ToString() + "$", Consts.Values.Menu.Ships.vec_string_Stats_Speed_Cost ) + offset, col );
                    col = Consts.Values.Menu.Ships.col_string_Avaible;
                    if( User.Ships[i].CanUpgradeAgility() == false )
                        col = Consts.Values.Menu.Ships.col_string_NotAvaible;
                    spriteBatch.DrawString( Consts.Fonts.ShipLevel, User.Ships[i].NextLevelAgility.ToString(), Consts.Values.Menu.Ships.vec_string_Stats_Agility_Level + offset, col );
                    col = Consts.Values.Menu.Ships.col_string_Avaible;
                    if( User.Money < User.Ships[i].NextCostAgility )
                        col = Consts.Values.Menu.Ships.col_string_NotAvaible;
                    spriteBatch.DrawString( Consts.Fonts.StatsCost, User.Ships[i].NextCostAgility.ToString() + "$",
                                            Functions.RightAlign( Consts.Fonts.StatsCost, User.Ships[i].NextCostAgility.ToString() + "$", Consts.Values.Menu.Ships.vec_string_Stats_Agility_Cost ) + offset, col );
                    col = Consts.Values.Menu.Ships.col_string_Avaible;
                    if( User.Ships[i].CanUpgradeHealth() == false )
                        col = Consts.Values.Menu.Ships.col_string_NotAvaible;
                    spriteBatch.DrawString( Consts.Fonts.ShipLevel, User.Ships[i].NextLevelHealth.ToString(), Consts.Values.Menu.Ships.vec_string_Stats_Health_Level + offset, col );
                    col = Consts.Values.Menu.Ships.col_string_Avaible;
                    if( User.Money < User.Ships[i].NextCostHealth )
                        col = Consts.Values.Menu.Ships.col_string_NotAvaible;
                    spriteBatch.DrawString( Consts.Fonts.StatsCost, User.Ships[i].NextCostHealth.ToString() + "$",
                                            Functions.RightAlign( Consts.Fonts.StatsCost, User.Ships[i].NextCostHealth.ToString() + "$", Consts.Values.Menu.Ships.vec_string_Stats_Health_Cost ) + offset, col );
                    #endregion
                    this.bar_ships_Experience[i].Draw( spriteBatch, gameTime );
                    this.but_Choose[i].Draw( spriteBatch );

                    this.bar_ships_UpSpeed[i].Draw( spriteBatch, gameTime );
                    this.bar_ships_UpSpeed[i].Draw( spriteBatch, gameTime );
                    this.bar_ships_UpAgility[i].Draw( spriteBatch, gameTime );
                    this.bar_ships_UpAgility[i].Draw( spriteBatch, gameTime );
                    this.bar_ships_UpHealth[i].Draw( spriteBatch, gameTime );
                    this.bar_ships_UpHealth[i].Draw( spriteBatch, gameTime );
                    this.bar_ships_Speed[i].Draw( spriteBatch, gameTime );
                    this.bar_ships_Speed[i].Draw( spriteBatch, gameTime );
                    this.bar_ships_Agility[i].Draw( spriteBatch, gameTime );
                    this.bar_ships_Agility[i].Draw( spriteBatch, gameTime );
                    this.bar_ships_Health[i].Draw( spriteBatch, gameTime );
                    this.bar_ships_Health[i].Draw( spriteBatch, gameTime );

                } else {    //ten statek nie jest jeszcze zakupiony
                    spriteBatch.DrawString( Consts.Values.Menu.Ships.font_Ship_Name, User.Ships[i].Name, Consts.Values.Menu.Ships.vec_string_Ship_Name + offset, Consts.Values.Menu.Ships.col_string_NotAvaible );
                    Color col = Consts.Values.Menu.Ships.col_string_Avaible;
                    if( User.Money < User.Ships[i].Cost )
                        col = Consts.Values.Menu.Ships.col_string_NotAvaible;
                    spriteBatch.DrawString( Consts.Fonts.ShipCost, User.Ships[i].Cost.ToString() + "$", Functions.RightAlign( Consts.Fonts.ShipCost, User.Ships[i].Cost.ToString() + "$", Consts.Values.Menu.Ships.vec_string_Ship_Cost ) + offset, col );
                    col = Consts.Values.Menu.Ships.col_string_Avaible;
                    if( User.Level < User.Ships[i].LevelCost )
                        col = Consts.Values.Menu.Ships.col_string_NotAvaible;
                    spriteBatch.DrawString( Consts.Fonts.ShipLevel, User.Ships[i].LevelCost.ToString(), Consts.Values.Menu.Ships.vec_string_Ship_Level + offset, col );

                    this.but_Buy[i].Draw( spriteBatch );
                    this.bar_ships_Speed[i].Draw( spriteBatch, gameTime );
                    this.bar_ships_Speed[i].Draw( spriteBatch, gameTime );
                    this.bar_ships_Agility[i].Draw( spriteBatch, gameTime );
                    this.bar_ships_Agility[i].Draw( spriteBatch, gameTime );
                    this.bar_ships_Health[i].Draw( spriteBatch, gameTime );
                    this.bar_ships_Health[i].Draw( spriteBatch, gameTime );
                }
            }
            spriteBatch.Draw( Consts.Textures.Menu.Interface_Ships_Mask, Consts.Values.Menu.Ships.vec_interface_Mask, Color.White );
            spriteBatch.Draw( Consts.Values.Menu.Ships.tex_interface_User, Consts.Values.Menu.Ships.vec_interface_User, Color.White );
            spriteBatch.DrawString( Consts.Values.Menu.Ships.font_Nick, Settings.Nick, Consts.Values.Menu.Ships.vec_string_Nick, Consts.Values.Menu.Ships.col_string_Nick );
            spriteBatch.DrawString( Consts.Values.Menu.Ships.font_User_Level, ( User.Level > 9 ? User.Level.ToString() : "0" + User.Level.ToString() ), Consts.Values.Menu.Ships.vec_string_User_Level, Consts.Values.Menu.Ships.col_Experience );
            spriteBatch.DrawString( Consts.Values.Menu.Ships.font_Level_Percent, User.LevelPercent.ToString() + "%", Consts.Values.Menu.Ships.vec_string_User_Level_Percent, Consts.Values.Menu.Ships.col_Experience );
            spriteBatch.DrawString( Consts.Values.Menu.Ships.font_Money, User.Money.ToString() + "$", Functions.RightAlign( Consts.Fonts.UserMoney, User.Money.ToString() + "$", Consts.Values.Menu.Ships.vec_string_Money ), Consts.Values.Menu.Ships.col_Money );
            this.bar_User_Exp.Draw( spriteBatch, gameTime );
            this.but_Help.Draw( spriteBatch );
            this.but_Back.Draw( spriteBatch );
            this.but_Next.Draw( spriteBatch );
            this.slider.Draw( spriteBatch, gameTime );

            if( this.m_helpStep != 0 ) {
                spriteBatch.Draw( Consts.Textures.Menu.Interface_Ships_Step[this.m_helpStep-1], Vector2.Zero, Color.White );
            }
        }

        private void UpdateSlider(GameTime gameTime) {
            this.slider.Update( gameTime );
            this.m_ValueLast = this.m_Value;
            this.m_Value = slider.Value;
            if( this.m_ValueLast != this.m_Value ) {
                int cutHeight = this.m_height - Consts.Values.Menu.Ships.rec_View_Menu.Height;
                this.m_SliderOffset_Last = this.m_SliderOffset;
                this.m_SliderOffset = new Vector2( 0, cutHeight * this.m_Value );

                for( int i = 0; i < this.m_NumOfShips; ++i ) {
                    this.but_Choose[i].Position += this.m_SliderOffset_Last - this.m_SliderOffset;
                    this.but_Buy[i].Position += this.m_SliderOffset_Last - this.m_SliderOffset;
                    this.but_Upgrade_Speed[i].Position += this.m_SliderOffset_Last - this.m_SliderOffset;
                    this.but_Upgrade_Agility[i].Position += this.m_SliderOffset_Last - this.m_SliderOffset;
                    this.but_Upgrade_Health[i].Position += this.m_SliderOffset_Last - this.m_SliderOffset;

                    this.bar_ships_Speed[i].Position += this.m_SliderOffset_Last - this.m_SliderOffset;
                    this.bar_ships_Agility[i].Position += this.m_SliderOffset_Last - this.m_SliderOffset;
                    this.bar_ships_Health[i].Position += this.m_SliderOffset_Last - this.m_SliderOffset;
                    this.bar_ships_UpSpeed[i].Position += this.m_SliderOffset_Last - this.m_SliderOffset;
                    this.bar_ships_UpAgility[i].Position += this.m_SliderOffset_Last - this.m_SliderOffset;
                    this.bar_ships_UpHealth[i].Position += this.m_SliderOffset_Last - this.m_SliderOffset;
                    this.bar_ships_Experience[i].Position += this.m_SliderOffset_Last - this.m_SliderOffset;
                }
                this.UpdateButtonsStates();
            }
        }
        private void UpdateButtonsStates() {
            for( int i=0; i < this.m_NumOfShips; ++i ) {
                if( User.BuyedShips[i] == true ) {
                    if( this.but_Choose[i].State == Interface.MyButtonState.LOCK && User.Ship != i && this.InViewRectangle( this.but_Choose[i].Area ) == true )
                        this.but_Choose[i].State = Interface.MyButtonState.NORMAL;
                    if( this.but_Choose[i].State != Interface.MyButtonState.LOCK && ( User.Ship == i || this.InViewRectangle( this.but_Choose[i].Area ) == false ) )
                        this.but_Choose[i].State = Interface.MyButtonState.LOCK;
                    if( this.but_Upgrade_Speed[i].State == Interface.MyButtonState.LOCK &&
                        User.Money >= User.Ships[i].NextCostSpeed && User.Ships[i].Level >= User.Ships[i].NextLevelSpeed &&
                        this.InViewRectangle( this.but_Upgrade_Speed[i].Area ) == true )
                        this.but_Upgrade_Speed[i].State = Interface.MyButtonState.NORMAL;
                    if( this.but_Upgrade_Speed[i].State != Interface.MyButtonState.LOCK &&
                        ( User.Money < User.Ships[i].NextCostSpeed || User.Ships[i].Level < User.Ships[i].NextLevelSpeed ||
                        this.InViewRectangle( this.but_Upgrade_Speed[i].Area ) == false ) )
                        this.but_Upgrade_Speed[i].State = Interface.MyButtonState.LOCK;
                    if( this.but_Upgrade_Agility[i].State == Interface.MyButtonState.LOCK &&
                        User.Money >= User.Ships[i].NextCostAgility && User.Ships[i].Level >= User.Ships[i].NextLevelAgility &&
                        this.InViewRectangle( this.but_Upgrade_Agility[i].Area ) == true )
                        this.but_Upgrade_Agility[i].State = Interface.MyButtonState.NORMAL;
                    if( this.but_Upgrade_Agility[i].State != Interface.MyButtonState.LOCK &&
                        ( User.Money < User.Ships[i].NextCostAgility || User.Ships[i].Level < User.Ships[i].NextLevelAgility ||
                        this.InViewRectangle( this.but_Upgrade_Agility[i].Area ) == false ) )
                        this.but_Upgrade_Agility[i].State = Interface.MyButtonState.LOCK;
                    if( this.but_Upgrade_Health[i].State == Interface.MyButtonState.LOCK &&
                        User.Money >= User.Ships[i].NextCostHealth && User.Ships[i].Level >= User.Ships[i].NextLevelHealth &&
                        this.InViewRectangle( this.but_Upgrade_Health[i].Area ) == true )
                        this.but_Upgrade_Health[i].State = Interface.MyButtonState.NORMAL;
                    if( this.but_Upgrade_Health[i].State != Interface.MyButtonState.LOCK &&
                        ( User.Money < User.Ships[i].NextCostHealth || User.Ships[i].Level < User.Ships[i].NextLevelHealth ||
                        this.InViewRectangle( this.but_Upgrade_Health[i].Area ) == false ) )
                        this.but_Upgrade_Health[i].State = Interface.MyButtonState.LOCK;
                } else {
                    if( this.but_Buy[i].State != Interface.MyButtonState.LOCK &&
                      ( User.Money < User.Ships[i].Cost ||
                        User.Level < User.Ships[i].LevelCost ||
                        this.InViewRectangle( this.but_Buy[i].Area ) == false ) )
                        this.but_Buy[i].State = Interface.MyButtonState.LOCK;
                    if( this.but_Buy[i].State == Interface.MyButtonState.LOCK &&
                        User.Money >= User.Ships[i].Cost &&
                        User.Level >= User.Ships[i].LevelCost &&
                        this.InViewRectangle( this.but_Buy[i].Area ) == true )
                        this.but_Buy[i].State = Interface.MyButtonState.NORMAL;
                    if( this.but_Upgrade_Speed[i].State != Interface.MyButtonState.LOCK )
                        this.but_Upgrade_Speed[i].State = Interface.MyButtonState.LOCK;
                    if( this.but_Upgrade_Agility[i].State != Interface.MyButtonState.LOCK )
                        this.but_Upgrade_Agility[i].State = Interface.MyButtonState.LOCK;
                    if( this.but_Upgrade_Health[i].State != Interface.MyButtonState.LOCK )
                        this.but_Upgrade_Health[i].State = Interface.MyButtonState.LOCK;
                }
            }
        }
        private bool InViewRectangle(Rectangle rec) {
            if( rec.Bottom < Consts.Values.Menu.Ships.rec_View_Menu.Top )
                return false;
            return true;
        }
        private void UpdateBars() {
            for( int i = 0; i < this.m_NumOfShips; ++i ) {
                this.bar_ships_UpSpeed[i].Value = User.Ships[i].NextSpeed / Consts.Values.Ships.reference_Speed;
                this.bar_ships_UpAgility[i].Value = User.Ships[i].NextAgility / Consts.Values.Ships.reference_Agility;
                this.bar_ships_UpHealth[i].Value = ( float ) User.Ships[i].NextHealth / Consts.Values.Ships.reference_Health;
                this.bar_ships_Speed[i].Value = User.Ships[i].Speed / Consts.Values.Ships.reference_Speed;
                this.bar_ships_Health[i].Value = ( float ) User.Ships[i].Health / Consts.Values.Ships.reference_Health;
                this.bar_ships_Agility[i].Value = User.Ships[i].Agility / Consts.Values.Ships.reference_Agility;
            }
        }
        private void BuyShip(int index) {
            if(     /*User.BuyedShips[index] == false &&*/  //ta linijka powinna być zbędna!s
                    User.Level >= User.Ships[index].LevelCost &&
                    User.Money >= User.Ships[index].Cost ) {
                User.Money -= User.Ships[index].Cost;
                User.BuyedShips[index] = true;
            }
        }
    }
}
