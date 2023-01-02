using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
//using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
//using Microsoft.Xna.Framework.Storage;

namespace PewPew {
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Root : Microsoft.Xna.Framework.Game {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Stage Plansza;
        Menus.MenuState MenuState, lastMenuState, nextState;
        Menus.Menu CurrentMenu;
        bool first = true;

        public Root() {
            graphics = new GraphicsDeviceManager( this );
            Content.RootDirectory = "Content";
            //this.Components.Add( new GamerServicesComponent( this ) );
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize() {
            // TODO: Add your initialization logic here
            base.Initialize();
            this.MenuState = Menus.MenuState.START;
            this.lastMenuState = Menus.MenuState.EXIT;    //cokolwiek inne ni¿ START
            this.CurrentMenu = new Menus.MainMenu();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent() {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch( this.GraphicsDevice );
            Consts.Audio.Load( this.Content );
            Consts.Textures.Load( this.Content );
            Consts.Fonts.Load( this.Content );
            Consts.Load( this.GraphicsDevice );
}

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent() {
            // TODO: Unload any non ContentManager content here
            Content.Unload();
        }
        /// <summary>
        /// Metoda jest wywoy³wana przy pierwszym loopie
        /// gry, na poczatku metody Update
        /// </summary>
        protected void StartGame() {
            User.Reset();
            Consts.StorageManager.LoadSettings();
            Consts.StorageManager.LoadUser();
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime) {
            if( this.IsActive == true ) {   //jesli okienko z gra jest aktywne
                Consts.PreviousKeyboardState = Consts.KeyboardState;
                Consts.PreviousMouseState = Consts.MouseState;
                Consts.KeyboardState = Keyboard.GetState();
                Consts.MouseState = Mouse.GetState();
                this.MenuState = this.nextState;
                if( this.lastMenuState != this.MenuState ) {
                    if( this.lastMenuState == PewPew.Menus.MenuState.SETTINGS ) {
                        Consts.StorageManager.SaveSettings( SaveGameDataSettings.CreateFromSettings() );
                    }
                    if( this.lastMenuState == PewPew.Menus.MenuState.PLAY ) {
                        Consts.StorageManager.SaveUser( SaveGameDataUser.CreateFromStatic() );
                    }
                    switch( this.MenuState ) {  //do którego menu wchodzimy
                        case Menus.MenuState.EXIT: {
                                this.Exit();
                                break;
                            }
                        case Menus.MenuState.PLAY: {
                                Consts.Pointer.SetColor( Consts.Values.Pointers.col_Crosshair );
                                Consts.Pointer.SetPointer( Interface.EPointer.crosshair );
                                Plansza = new Stage( Consts.Values.Stages.Array[User.Stage] );
                                break;
                            }
                        case Menus.MenuState.START: {
                                Consts.Pointer.SetColor( Consts.Values.Pointers.col_Menu );
                                Consts.Pointer.SetPointer( Interface.EPointer.arrow );
                                if( this.first ) {  //pierwszy raz od kiedy uruchomiona gre wyswietlono menu glowne
                                    this.StartGame();
                                    this.first = false;
                                }
                                this.CurrentMenu = new Menus.MainMenu();
                                break;
                            }
                        case Menus.MenuState.AUTHORS: {
                                Consts.Pointer.SetColor( Consts.Values.Pointers.col_Menu );
                                Consts.Pointer.SetPointer( Interface.EPointer.author );
                                this.CurrentMenu = new Menus.AuthorMenu();
                                break;
                            }
                        case Menus.MenuState.SETTINGS: {
                            this.CurrentMenu = new Menus.SettingsMenu();
                                break;
                            }
                        case Menus.MenuState.SHIPS: {
                            this.CurrentMenu = new Menus.ShipsMenu();
                                break;
                            }
                        case Menus.MenuState.STAGES: {
                                this.CurrentMenu = new Menus.StagesMenu();
                                break;
                            }
                    }
                    this.lastMenuState = this.MenuState;
                }
                if( Consts.Pointer.Special )
                    Consts.Pointer.Update( gameTime );

                // TODO: Add your update logic here
                switch( this.MenuState ) {
                    case Menus.MenuState.PLAY: {
                            this.nextState = this.Plansza.Update( gameTime );
                            break;
                        }
                    case Menus.MenuState.EXIT: {
                            this.Exit();
                            break;
                        }
                    default: {
                            this.nextState = this.CurrentMenu.Update( gameTime );
                            break;
                        }
                }
            }
            base.Update( gameTime );
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>

        protected override void Draw(GameTime gameTime) {
            GraphicsDevice.Clear( Color.Black );

            // TODO: Add your drawing code here
            spriteBatch.Begin();

            switch( this.MenuState ) {
                case Menus.MenuState.PLAY: {
                        Plansza.Draw( spriteBatch, gameTime );
                        break;
                    }
                default: {
                        CurrentMenu.Draw( spriteBatch, gameTime );
                        break;
                    }
            }
            Consts.Pointer.Draw( spriteBatch, gameTime );
						Functions.DrawCircle( spriteBatch, new Vector2( 100 ), 20, 4, 10, Color.YellowGreen );
            spriteBatch.End();
            base.Draw( gameTime );
        }
    }
}
