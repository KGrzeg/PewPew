using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PewPew.Menus {
    public class MainMenu : Menu {
        private Interface.Button but_Author, but_Settings, but_StartGame, but_Exit;
        public MainMenu() {
            base.m_Tex = Consts.Values.Menu.Main.tex_Background;

            this.but_Author = new Interface.Button( Consts.Values.Menu.Main.vec_but_Author, Consts.Values.Menu.Main.tex_but_Author, Consts.Values.Menu.Main.tex_but_Author_f );
            this.but_Settings = new Interface.Button( Consts.Values.Menu.Main.vec_but_Settings, Consts.Values.Menu.Main.tex_but_Settings, Consts.Values.Menu.Main.tex_but_Settings_f );
            this.but_StartGame = new Interface.Button( Consts.Values.Menu.Main.vec_but_StartGame, Consts.Values.Menu.Main.tex_but_StartGame, Consts.Values.Menu.Main.tex_but_StartGame_f );
            this.but_Exit = new Interface.Button( Consts.Values.Menu.Main.vec_but_Exit, Consts.Values.Menu.Main.tex_but_Exit, Consts.Values.Menu.Main.tex_but_Exit_f );
        }

        public override MenuState Update(GameTime gameTime) {
            if( this.but_StartGame.Update( gameTime ) )
                return MenuState.SHIPS;
            if( this.but_Settings.Update( gameTime ) )
                return MenuState.SETTINGS;
            if( this.but_Author.Update( gameTime ) )
                return MenuState.AUTHORS;
            if( this.but_Exit.Update( gameTime ) )
                return MenuState.EXIT;
            return MenuState.START;
        }
        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime) {
            spriteBatch.Draw( this.m_Tex, Consts.GraphicsDevice.Viewport.TitleSafeArea, Color.White );
            this.but_StartGame.Draw( spriteBatch );
            this.but_Settings.Draw( spriteBatch );
            this.but_Author.Draw( spriteBatch );
            this.but_Exit.Draw( spriteBatch );
        }
    }
}
