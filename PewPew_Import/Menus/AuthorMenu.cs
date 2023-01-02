using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PewPew.Menus {
    public class AuthorMenu : Menu {
        private Interface.Button but_Back;

        public AuthorMenu() {
            base.m_Tex = Consts.Values.Menu.Author.tex_Background;

            this.but_Back = new Interface.Button( Consts.Values.Menu.Author.vec_but_Back, Consts.Values.Menu.Author.tex_but_Back, Consts.Values.Menu.Author.tex_but_Back_f );
        }

        public override MenuState Update(GameTime gameTime) {
            if( this.but_Back.Update( gameTime ) )
                return MenuState.START;
            return MenuState.AUTHORS;
        }
        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime) {
            spriteBatch.Draw( base.m_Tex, Consts.GraphicsDevice.Viewport.TitleSafeArea, Color.White );
            this.but_Back.Draw( spriteBatch );
        }
    }
}
