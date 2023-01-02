using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PewPew.Menus {
    public enum MenuState {
        START = 0,  //pierwszy ekran gry
        STAGES,//wybór poziomu
        SHIPS, //wybór statku
        PLAY,       //gra trwa
        PLAY_MENU,  //menu podczas gry
        PLAY_MOVIE, //przerywnik fabularny podczas gry
        SETTINGS,   //ustawienia
        AUTHORS,    //ekran z informacją o autorach
        EXIT        //koniec gry - wyjście
    }
    public abstract class Menu {
        protected Texture2D m_Tex;
        public abstract MenuState Update(GameTime gameTime);
        public abstract void Draw(SpriteBatch spriteBatch, GameTime gameTime);
    }
}
