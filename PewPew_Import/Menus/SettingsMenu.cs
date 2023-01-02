using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PewPew.Menus {
    public class SettingsMenu : Menu {
        private static readonly Vector2
            const_SureN = new Vector2( 275, 268 ),
            const_SureY = new Vector2( 447, 268 ),
            const_interface = new Vector2( 250, 140 );

        private Interface.Button but_Back,
                       but_Save,
                       but_Reset,
                       but_SureY, 
                       but_SureN;
        private Interface.SliderHorizontal slider_Music,
                       slider_Effects,
                       slider_r,
                       slider_g,
                       slider_b;
        private Texture2D m_tex_Ship;
        private Vector2 m_vec_Ship;
        private Interface.InputText m_Input;
        private bool AreYouSure = false;
        public SettingsMenu() {
            this.m_Tex = Consts.Values.Menu.Settings.tex_Background;
            this.m_tex_Ship = Consts.Values.Menu.Settings.tex_ship;
            this.m_vec_Ship = Consts.Values.Menu.Settings.vec_Ship - new Vector2( this.m_tex_Ship.Width / 2, this.m_tex_Ship.Height / 2 );
            this.m_Input = new Interface.InputText( Consts.Values.Menu.Settings.rec_input, Consts.Fonts.Nick, Consts.Values.Menu.Settings.input_maxChar, Consts.Values.Menu.Settings.col_input, Settings.Nick );
            this.but_Back = new Interface.Button( Consts.Values.Menu.Settings.vec_but_Back, Consts.Values.Menu.Settings.tex_but_Back, Consts.Values.Menu.Settings.tex_but_Back_f );
            this.but_Save = new Interface.Button( Consts.Values.Menu.Settings.vec_but_Save, Consts.Textures.Menu.But_Settings_Save, Consts.Textures.Menu.But_Settings_Save_f );
            this.but_Reset = new Interface.Button( Consts.Values.Menu.Settings.vec_but_Reset, Consts.Textures.Menu.But_Settings_Reset, Consts.Textures.Menu.But_Settings_Reset_f );
            this.but_SureY = new Interface.Button( const_SureY, Consts.Textures.Menu.But_Settings_SureY, Consts.Textures.Menu.But_Settings_SureY_f );
            this.but_SureN = new Interface.Button( const_SureN, Consts.Textures.Menu.But_Settings_SureN, Consts.Textures.Menu.But_Settings_SureN_f );
            this.slider_Music = new Interface.SliderHorizontal( Consts.Values.Menu.Settings.tex_slider_b, Consts.Values.Menu.Settings.vec_slider_Music,
                                            Consts.Values.Menu.Settings.slider_width_b, Consts.Values.Menu.Settings.col_slider,
                                            Settings.Volume_Music );
            this.slider_Effects = new Interface.SliderHorizontal( Consts.Values.Menu.Settings.tex_slider_b, Consts.Values.Menu.Settings.vec_slider_Effects,
                                              Consts.Values.Menu.Settings.slider_width_b, Consts.Values.Menu.Settings.col_slider,
                                              Settings.Volume_Sfx );
            this.slider_r = new Interface.SliderHorizontal( Consts.Values.Menu.Settings.tex_slider_s, Consts.Values.Menu.Settings.vec_slider_R,
                                        Consts.Values.Menu.Settings.slider_width_s, Consts.Values.Menu.Settings.col_slider_R,
                                        ( float ) ( Settings.Color_Player.R ) / 255 );
            this.slider_g = new Interface.SliderHorizontal( Consts.Values.Menu.Settings.tex_slider_s, Consts.Values.Menu.Settings.vec_slider_G,
                                        Consts.Values.Menu.Settings.slider_width_s, Consts.Values.Menu.Settings.col_slider_G,
                                        ( float ) ( Settings.Color_Player.G ) / 255 );
            this.slider_b = new Interface.SliderHorizontal( Consts.Values.Menu.Settings.tex_slider_s, Consts.Values.Menu.Settings.vec_slider_B,
                                        Consts.Values.Menu.Settings.slider_width_s, Consts.Values.Menu.Settings.col_slider_B,
                                        ( float ) ( Settings.Color_Player.B ) / 255 );
        }

        public override MenuState Update(GameTime gameTime) {
            if( this.AreYouSure == true ) {
                if(this.but_SureY.Update(gameTime)){
                    User.Reset();
                    this.AreYouSure = false;
                }
                if( this.but_SureN.Update( gameTime ) ) {
                    this.AreYouSure = false;
                }
                if(Functions.MouseLeftClick() &&    //kliknięgo, gdy mysz była poza obszarem menu
                    !Functions.Intersects(new Rectangle((int)const_interface.X, (int)const_interface.Y, Consts.Textures.Menu.Interface_Settings_AreYouSure.Width, Consts.Textures.Menu.Interface_Settings_AreYouSure.Height), Functions.GetMousePosition()))
                this.AreYouSure = false;
            } else {
                if( this.but_Back.Update( gameTime ) )
                    return MenuState.START;
                if( this.slider_Music.Update( gameTime ) ) { Settings.Volume_Music = this.slider_Music.Value; Consts.AudioManager.Update(); }
                if( this.slider_Effects.Update( gameTime ) ) Settings.Volume_Sfx = this.slider_Effects.Value;
                if( this.slider_r.Update( gameTime ) ) Settings.Color_Player.R = ( byte ) ( 255 * this.slider_r.Value );
                if( this.slider_g.Update( gameTime ) ) Settings.Color_Player.G = ( byte ) ( 255 * this.slider_g.Value );
                if( this.slider_b.Update( gameTime ) ) Settings.Color_Player.B = ( byte ) ( 255 * this.slider_b.Value );
                if( this.but_Save.Update( gameTime ) ) Settings.Nick = this.m_Input.Data;
                if( this.but_Reset.Update( gameTime ) ) this.AreYouSure = true; ;
                this.m_Input.Update( gameTime );
            }
            return MenuState.SETTINGS;
        }
        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime) {
            spriteBatch.Draw( this.m_Tex, Consts.GraphicsDevice.Viewport.TitleSafeArea, Color.White );
            this.slider_Music.Draw( spriteBatch, gameTime );
            this.slider_Effects.Draw( spriteBatch, gameTime );
            this.slider_r.Draw( spriteBatch, gameTime );
            this.slider_g.Draw( spriteBatch, gameTime );
            this.slider_b.Draw( spriteBatch, gameTime );
            this.but_Back.Draw( spriteBatch );
            this.but_Save.Draw( spriteBatch );
            this.but_Reset.Draw( spriteBatch );
            this.m_Input.Draw( spriteBatch, gameTime );
            spriteBatch.Draw( this.m_tex_Ship, this.m_vec_Ship, Settings.Color_Player );
            if( this.AreYouSure == true ) {
                spriteBatch.Draw( Consts.Textures.Menu.Interface_Settings_AreYouSure, const_interface, Color.White );
                this.but_SureY.Draw( spriteBatch );
                this.but_SureN.Draw( spriteBatch );
            }
        }
    }
}
