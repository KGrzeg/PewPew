using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace PewPew {
    public static class Consts {
        public static Random Random;
        public static GraphicsDevice GraphicsDevice;
        public static Interface.Pointer Pointer;
        public static AudioManager AudioManager;
        public static KeyboardState PreviousKeyboardState, KeyboardState;
        public static MouseState PreviousMouseState, MouseState;
        public static StorageManager StorageManager;
        
        public static void Load(GraphicsDevice graph) {
            Consts.Random = new Random();
            Consts.GraphicsDevice = graph;
            Consts.Pointer = new Interface.Pointer();
            Consts.AudioManager = new AudioManager();
            Consts.StorageManager = new StorageManager();
        }

        public static class Textures {
            public static Texture2D Dot,
                                    GameBackground,
                                    GameBackground1,
                                    Crosshair,
                                    Bullet,
                                    Pointer_Arrow;
            public static Texture2D Enemy_Chaos,
                                    Enemy_Head,
                                    Enemy_Tail,
                                    Enemy_Impulse,
                                    Enemy_Cannon,
                                    Enemy_Phantom,
                                    Enemy_Arsedroid1,
                                    Enemy_Arsedroid2,
                                    Enemy_Arsedroid3;
            public static Texture2D ship_Avast,
                                    ship_Avg,
                                    ship_Norton;
            public static class Stages {
                public static Texture2D
                    Image_1;
                public static void Load(ContentManager Content) {
                    Image_1 = Content.Load<Texture2D>( "Textures/Stages/image_1" );
                }
            }
            /// <summary>
            /// Tekstury związane z interfejsem gry
            /// </summary>
            public static class Interface {
                public static Texture2D Health;
                public static Texture2D Health_Bar;
                public static Texture2D Live;
            }
            /// <summary>
            /// Teksutry używane we wszystkich menu
            /// </summary>
            public static class Menu {
                public static class Stages {
                    public static Texture2D
                        Background,
                        Mask,
                        But_Back,
                        But_Back_f,
                        But_Start,
                        But_Start_f,
                        But_Stage,
                        But_Stage_f,
                        But_Stage_l,
                        Interface_Passed;
                    public static void Load(ContentManager Content) {
                        Background = Content.Load<Texture2D>( "Textures/Menu/Stages/background" );
                        Mask = Content.Load<Texture2D>( "Textures/Menu/Stages/mask" );
                        But_Back = Content.Load<Texture2D>( "Textures/Menu/Stages/button_back" );
                        But_Back_f = Content.Load<Texture2D>( "Textures/Menu/Stages/button_back_f" );
                        But_Start = Content.Load<Texture2D>( "Textures/Menu/Stages/button_start" );
                        But_Start_f = Content.Load<Texture2D>( "Textures/Menu/Stages/button_start_f" );
                        But_Stage = Content.Load<Texture2D>( "Textures/Menu/Stages/button_stage" );
                        But_Stage_f = Content.Load<Texture2D>( "Textures/Menu/Stages/button_stage_f" );
                        But_Stage_l = Content.Load<Texture2D>( "Textures/Menu/Stages/button_stage_l" );
                        Interface_Passed = Content.Load<Texture2D>( "Textures/Menu/Stages/interface_passed" );
                    }
                }
                public static Texture2D Background_Main,
                                        Background_Author,
                                        Background_Settings,
                                        Background_Ships;
                public static Texture2D Slider_b,
                                        Slider_s,
                                        Slider_page,
                                        But_Main_Author,
                                        But_Main_Author_f,
                                        But_Main_Play,
                                        But_Main_Play_f,
                                        But_Main_Settings,
                                        But_Main_Settings_f,
                                        But_Main_Exit,
                                        But_Main_Exit_f,
                                        But_Author_Back,
                                        But_Author_Back_f,
                                        But_Settings_Back,
                                        But_Settings_Back_f,
                                        But_Settings_Save,
                                        But_Settings_Save_f,
                                        But_Settings_Reset,
                                        But_Settings_Reset_f,
                                        But_Settings_SureY,
                                        But_Settings_SureN,
                                        But_Settings_SureY_f,
                                        But_Settings_SureN_f,
                                        But_Ships_Back,
                                        But_Ships_Back_f,
                                        But_Ships_Help,
                                        But_Ships_Help_f,
                                        But_Ships_Next,
                                        But_Ships_Next_f,
                                        But_Ships_Upgrade,
                                        But_Ships_Upgrade_f,
                                        But_Ships_Upgrade_l,
                                        But_Ships_Choose,
                                        But_Ships_Choose_f,
                                        But_Ships_Choose_l,
                                        But_Ships_Buy,
                                        But_Ships_Buy_f,
                                        But_Ships_Buy_l,
                                        Interface_Ships_User,
                                        Interface_Ships_Ship,
                                        Interface_Ships_Mask,
                                        Interface_Settings_AreYouSure,
                                        Bar_Ships_Ship_Exp,
                                        Bar_Ships_User_Exp,
                                        Bar_Ships_Ship_Stats;
                public static Texture2D[] Interface_Ships_Step = new Texture2D[4];
            }
            /// <summary>
            /// Funkcja ładująca tekstury do obiektu
            /// </summary>
            /// <param name="Content"></param>
            public static void Load(ContentManager Content) {
                Consts.Textures.Dot = Content.Load<Texture2D>( "Textures/dot" );
                Consts.Textures.Bullet = Content.Load<Texture2D>( "Textures/Bullets/simple" );
                Consts.Textures.GameBackground = Content.Load<Texture2D>( "Textures/Stages/level_x" );
                Consts.Textures.GameBackground1 = Content.Load<Texture2D>( "Textures/Stages/level_1" );
                Consts.Textures.Crosshair = Content.Load<Texture2D>( "Textures/crosshair" );
                Consts.Textures.Pointer_Arrow = Content.Load<Texture2D>( "Textures/pointer_arrow_bw" );

                Consts.Textures.Enemy_Chaos = Content.Load<Texture2D>( "Textures/Enemies/enemy_chaos" );
                Consts.Textures.Enemy_Head = Content.Load<Texture2D>( "Textures/Enemies/enemy_head" );
                Consts.Textures.Enemy_Tail = Content.Load<Texture2D>( "Textures/Enemies/enemy_tail" );
                Consts.Textures.Enemy_Impulse = Content.Load<Texture2D>( "Textures/Enemies/enemy_impulse" );
                Consts.Textures.Enemy_Cannon = Content.Load<Texture2D>( "Textures/Enemies/enemy_cannon" );
                Consts.Textures.Enemy_Phantom = Content.Load<Texture2D>( "Textures/Enemies/enemy_phantom" );
                Consts.Textures.Enemy_Arsedroid1 = Content.Load<Texture2D>( "Textures/Enemies/enemy_arsedroidm1" );
                Consts.Textures.Enemy_Arsedroid2 = Content.Load<Texture2D>( "Textures/Enemies/enemy_arsedroidm2" );
                Consts.Textures.Enemy_Arsedroid3 = Content.Load<Texture2D>( "Textures/Enemies/enemy_arsedroidm3" );

                Consts.Textures.ship_Avast = Content.Load<Texture2D>( "Textures/Ships/avast" );
                Consts.Textures.ship_Avg = Content.Load<Texture2D>( "Textures/Ships/avg" );
                Consts.Textures.ship_Norton = Content.Load<Texture2D>( "Textures/Ships/norton" );

                Consts.Textures.Interface.Health = Content.Load<Texture2D>( "Textures/GameInterface/health" );
                Consts.Textures.Interface.Health_Bar = Content.Load<Texture2D>( "Textures/GameInterface/health_bar" );
                Consts.Textures.Interface.Live = Content.Load<Texture2D>( "Textures/GameInterface/live" );

                Consts.Textures.Menu.Background_Main = Content.Load<Texture2D>( "Textures/Menu/main_background" );
                Consts.Textures.Menu.Background_Author = Content.Load<Texture2D>( "Textures/Menu/background_author" );
                Consts.Textures.Menu.Background_Settings = Content.Load<Texture2D>( "Textures/Menu/background_settings" );
                Consts.Textures.Menu.Background_Ships = Content.Load<Texture2D>( "Textures/Menu/background_ships" );

                Consts.Textures.Menu.Slider_b = Content.Load<Texture2D>( "Textures/Menu/slider_b" );
                Consts.Textures.Menu.Slider_s = Content.Load<Texture2D>( "Textures/Menu/slider_s" );
                Consts.Textures.Menu.Slider_page = Content.Load<Texture2D>( "Textures/Menu/slider_page" );
                Consts.Textures.Menu.But_Main_Author = Content.Load<Texture2D>( "Textures/Menu/button_authors" );
                Consts.Textures.Menu.But_Main_Author_f = Content.Load<Texture2D>( "Textures/Menu/button_authors_f" );
                Consts.Textures.Menu.But_Main_Play = Content.Load<Texture2D>( "Textures/Menu/button_start_game" );
                Consts.Textures.Menu.But_Main_Play_f = Content.Load<Texture2D>( "Textures/Menu/button_start_game_f" );
                Consts.Textures.Menu.But_Main_Settings = Content.Load<Texture2D>( "Textures/Menu/button_settings" );
                Consts.Textures.Menu.But_Main_Settings_f = Content.Load<Texture2D>( "Textures/Menu/button_settings_f" );
                Consts.Textures.Menu.But_Main_Exit = Content.Load<Texture2D>( "Textures/Menu/button_exit" );
                Consts.Textures.Menu.But_Main_Exit_f = Content.Load<Texture2D>( "Textures/Menu/button_exit_f" );
                Consts.Textures.Menu.But_Author_Back = Content.Load<Texture2D>( "Textures/Menu/button_back" );
                Consts.Textures.Menu.But_Author_Back_f = Content.Load<Texture2D>( "Textures/Menu/button_back_f" );
                Consts.Textures.Menu.But_Settings_Back = Content.Load<Texture2D>( "Textures/Menu/button_back" );
                Consts.Textures.Menu.But_Settings_Back_f = Content.Load<Texture2D>( "Textures/Menu/button_back_f" );
                Consts.Textures.Menu.But_Settings_Reset = Content.Load<Texture2D>( "Textures/Menu/Settings/button_reset" );
                Consts.Textures.Menu.But_Settings_Reset_f = Content.Load<Texture2D>( "Textures/Menu/Settings/button_reset_f" );
                Consts.Textures.Menu.But_Settings_Save = Content.Load<Texture2D>( "Textures/Menu/Settings/button_save" );
                Consts.Textures.Menu.But_Settings_Save_f = Content.Load<Texture2D>( "Textures/Menu/Settings/button_save_f" );
                Consts.Textures.Menu.But_Settings_SureY = Content.Load<Texture2D>( "Textures/Menu/Settings/button_sureY" );
                Consts.Textures.Menu.But_Settings_SureN = Content.Load<Texture2D>( "Textures/Menu/Settings/button_sureN" );
                Consts.Textures.Menu.But_Settings_SureY_f = Content.Load<Texture2D>( "Textures/Menu/Settings/button_sureY_f" );
                Consts.Textures.Menu.But_Settings_SureN_f = Content.Load<Texture2D>( "Textures/Menu/Settings/button_sureN_f" );
                Consts.Textures.Menu.But_Ships_Back = Content.Load<Texture2D>( "Textures/Menu/Ships/button_back" );
                Consts.Textures.Menu.But_Ships_Back_f = Content.Load<Texture2D>( "Textures/Menu/Ships/button_back_f" );
                Consts.Textures.Menu.But_Ships_Help = Content.Load<Texture2D>( "Textures/Menu/Ships/button_help" );
                Consts.Textures.Menu.But_Ships_Help_f = Content.Load<Texture2D>( "Textures/Menu/Ships/button_help_f" );
                Consts.Textures.Menu.But_Ships_Next = Content.Load<Texture2D>( "Textures/Menu/Ships/button_next" );
                Consts.Textures.Menu.But_Ships_Next_f = Content.Load<Texture2D>( "Textures/Menu/Ships/button_next_f" );
                Consts.Textures.Menu.But_Ships_Choose = Content.Load<Texture2D>( "Textures/Menu/Ships/button_wybierz" );
                Consts.Textures.Menu.But_Ships_Choose_f = Content.Load<Texture2D>( "Textures/Menu/Ships/button_wybierz_f" );
                Consts.Textures.Menu.But_Ships_Choose_l = Content.Load<Texture2D>( "Textures/Menu/Ships/button_wybierz_l" );
                Consts.Textures.Menu.But_Ships_Upgrade = Content.Load<Texture2D>( "Textures/Menu/Ships/button_ulepsz" );
                Consts.Textures.Menu.But_Ships_Upgrade_f = Content.Load<Texture2D>( "Textures/Menu/Ships/button_ulepsz_f" );
                Consts.Textures.Menu.But_Ships_Upgrade_l = Content.Load<Texture2D>( "Textures/Menu/Ships/button_ulepsz_l" );
                Consts.Textures.Menu.But_Ships_Buy = Content.Load<Texture2D>( "Textures/Menu/Ships/button_kup" );
                Consts.Textures.Menu.But_Ships_Buy_f = Content.Load<Texture2D>( "Textures/Menu/Ships/button_kup_f" );
                Consts.Textures.Menu.But_Ships_Buy_l = Content.Load<Texture2D>( "Textures/Menu/Ships/button_kup_l" );
                Consts.Textures.Menu.Interface_Ships_User = Content.Load<Texture2D>( "Textures/Menu/Ships/interface_user" );
                Consts.Textures.Menu.Interface_Ships_Ship = Content.Load<Texture2D>( "Textures/Menu/Ships/interface_ship" );
                Consts.Textures.Menu.Interface_Ships_Mask = Content.Load<Texture2D>( "Textures/Menu/Ships/mask" );
                Consts.Textures.Menu.Interface_Ships_Step[0] = Content.Load<Texture2D>( "Textures/Menu/Ships/interface_step1" );
                Consts.Textures.Menu.Interface_Ships_Step[1] = Content.Load<Texture2D>( "Textures/Menu/Ships/interface_step2" );
                Consts.Textures.Menu.Interface_Ships_Step[2] = Content.Load<Texture2D>( "Textures/Menu/Ships/interface_step3" );
                Consts.Textures.Menu.Interface_Ships_Step[3] = Content.Load<Texture2D>( "Textures/Menu/Ships/interface_step4" );
                Consts.Textures.Menu.Interface_Settings_AreYouSure = Content.Load<Texture2D>( "Textures/Menu/Settings/interface_areyousure" );
                Consts.Textures.Menu.Bar_Ships_Ship_Exp = Content.Load<Texture2D>( "Textures/Menu/Ships/bar_ship_exp" );
                Consts.Textures.Menu.Bar_Ships_User_Exp = Content.Load<Texture2D>( "Textures/Menu/Ships/bar_user_exp" );
                Consts.Textures.Menu.Bar_Ships_Ship_Stats = Content.Load<Texture2D>( "Textures/Menu/Ships/bar_stats" );
                Consts.Textures.Menu.Stages.Load( Content );
                Consts.Textures.Stages.Load( Content );
            }
        }
        public static class Audio {
            public static SoundEffect Shoot;
            public static SoundEffect Bell;
            public static SoundEffect Spawn;
            public static SoundEffect Rock;

            public static Song Music;

            public static void Load(ContentManager Content) {
                Consts.Audio.Shoot = Content.Load<SoundEffect>( "Audio/shoot" );
                Consts.Audio.Bell = Content.Load<SoundEffect>( "Audio/bell" );
                Consts.Audio.Spawn = Content.Load<SoundEffect>( "Audio/spawn" );
                Consts.Audio.Rock = Content.Load<SoundEffect>( "Audio/rockBreak" );

                Consts.Audio.Music = Content.Load<Song>( "Audio/music1" );
            }
        }
        public static class Fonts {
            public static SpriteFont 
                font1,
                calculator,
                PopUp,
                Nick,
                LevelPercent,
                UserLevel,
                StatsLevel,
                UserMoney,
                ShipName,
                ShipLevel,
                ShipCost,
                StatsCost,
                StageDesc,
                StageName,
                StageAward,
                StageNum;
            public static void Load(ContentManager Content) {
                Consts.Fonts.font1 = Content.Load<SpriteFont>( "Fonts/Font1" );
                Consts.Fonts.calculator = Content.Load<SpriteFont>( "Fonts/Calculator" );
                Consts.Fonts.PopUp = Content.Load<SpriteFont>( "Fonts/PopUp" );
                Consts.Fonts.Nick = Content.Load<SpriteFont>( "Fonts/userNick" );
                Consts.Fonts.UserLevel = Content.Load<SpriteFont>( "Fonts/UserLevel" );
                Consts.Fonts.LevelPercent = Content.Load<SpriteFont>( "Fonts/UserLevelPercent" );
                Consts.Fonts.ShipName = Content.Load<SpriteFont>( "Fonts/shipName" );
                Consts.Fonts.ShipLevel = Content.Load<SpriteFont>( "Fonts/shipLevel" );
                Consts.Fonts.ShipCost = Content.Load<SpriteFont>( "Fonts/shipCost" );
                Consts.Fonts.StatsCost = Content.Load<SpriteFont>( "Fonts/shipStatsCost" );
                Consts.Fonts.UserMoney = Content.Load<SpriteFont>( "Fonts/userMoney" );
                Consts.Fonts.StageDesc = Content.Load<SpriteFont>( "Fonts/StageNorm" );
                Consts.Fonts.StageName = Content.Load<SpriteFont>( "Fonts/StageBig" );
                Consts.Fonts.StageNum = Content.Load<SpriteFont>( "Fonts/StageNum" );
                Consts.Fonts.StageAward = Content.Load<SpriteFont>( "Fonts/stageAward" );
            }
        }
        public static class Values {
            public const int LEFT_EDGE = 1;
            public const int RIGHT_EDGE = 2;
            public const int BOTTOM_EDGE = 4;
            public const int TOP_EDGE = 8;
            public static class Audio {
                public const float start_Volume = 1f;
                public const float start_Volume_Sfx = 0.6f;
                public const float start_Volume_Music = 1f;
            }
            public static class PopUps {
                public static class Score {
                    public static SpriteFont Font = Consts.Fonts.PopUp;
                    public static Color Color = Color.GreenYellow;
                    public const int Distance = 10;
                    public const int Direction = 270;
                    public const double Time = 1200;
                    public const string Prefix = "+";
                }
            }
            public static class GameInteface {
                public const string str_score = "punkty: "; //nie obsługuje '\n'
                public const int n_max_blanc_letter_in_score = 6;
                public const int n_max_visible_lives = 20;
                public static Vector2 vec_health = new Vector2( 20 );
                public static Vector2 vec_health_bar = new Vector2( 50, 25 );
                public static Vector2 vec_firsLive = new Vector2( 53, 42 );
                public static Vector2 vec_liveOffset = new Vector2( 8, 0 );
                public static Vector2 vec_score = new Vector2( 20, 55 );

                public static Rectangle rec_health_bar_fill = new Rectangle( 56, 31, 158, 8 );

                public static Color color_health_image = Color.GreenYellow;
                public static Color color_health_bar = Color.GreenYellow;
                public static Color color_health_bar_fill = Color.GreenYellow;
                public static Color color_live = Color.Peru;
                public static Color color_score = Color.Khaki;
                public static Color color_score_blanc = new Color( 70, 70, 70 );
            }
            public static class Camera {
                public static Rectangle Bounding = new Rectangle( 0, 0, 800, 480 );   //wymiary renderingu kamery (rozmiar okna - panel intefejsu, jeśli taki będzie)
                public const float Speed = 0.005f;  //odległość gracza od środka ekranu * ilość milisekund od ostatniej klatki * prędkość
            }
            public static class Player {    //gracz
                public static int[] LevelsTable = { 50, 150, 300, 500 };

                public static Texture2D Texture = Consts.Textures.ship_Avast; //grafika gracza
                public static Color Color = Color.Turquoise; //kolor pojazdu
                public static Vector2 start_Position = new Vector2( 150 );    //miejsce pojawienia się gracza
                public static SoundEffect wav_Shoot = Consts.Audio.Shoot;   //dźwięk strzału
                public static SoundEffect wav_Spawn = Consts.Audio.Spawn;   //dźwięk pojawiającego się gracza
                public const int start_Lives = 1;   //domyślna ilość dodatkowych żyć
                public const int MaxHealth = 10;    //domyślna i maksymalna ilość punktów życia
                public const float move_MaxSpeed = 0.240f; //maksymalna prędkość na jednej osi
                public const float move_Accelerate = 0.00090f;    //prędkość przyśpieszania
                public const float move_Drag = 0.6f;   //opór przy zwalnianiu (start_Acc * start_Drag)
                public const float size = 15f;  //promień gracza przy obliczaniu zderzenia z przeciwnikiem
                public const double fire_cooldown = 90;    //czas, jaki trzeba odczekać przed następnym strzałem
                public const double intouchable_time = 700; //czas, przez jaki gracz jest nietykalny po oberwaniu
                public const double intouchable_blink_time = 200;    //co jaki czas znika sprites gracza podczas nietykalnosci
                public const double intouchable_blink_invisible_time = 100;  //przez jaki czas spritesu nie widac
            }
            public static class Bullet {    //kule, pociski
                public const float Speed = 0.450f; //domyślna prędkość pocisku
                public const float Size = 10f; //promień pocisku
                public const int Damage = 1;  //obrażenia
                public static Texture2D Tex = Consts.Textures.Bullet; //grafika pocisku
                public static Color Color = Color.Violet;
            }
            public static class Enemies {
                public static SoundEffect wav_Hit = Consts.Audio.Bell;
                public static Color DamagedColor = Color.Red;
                public const double DamagedColorTime = 200;

                public static class Chaos {
                    public static Texture2D Tex = Consts.Textures.Enemy_Chaos;
                    public const int Damage = 1;
                    public const int Health = 1;
                    public const int Score = 1;
                    public static Color Color = Color.Orange;
                    public const double ThinkTime = 1800;
                    public const float Move_MaxSpeed = 0.085f;
                }
                public static class Znake {
                    public const int Health = 60;
                    public const float Move_Speed = 0.12f;
                    public const int Lenght = 15;    //ilość elementów ogona
                    public const int Score = 5;
                    public const double AddTileTime = 100;  //co jaki czas spawnować kolejną część ogona

                    public static class Head {
                        public static Texture2D Tex = Consts.Textures.Enemy_Head;
                        public const int Damage = 2;
                        public const int Health = 4;    //ta wartość reguluje obrażenia (jest dodawana do dmg), jakie są zadawane przeciwnikowi po trafieniu w ten element
                        public const int Score = 0; //nieważna wartość
                        public static Color Color = Color.LawnGreen;
                        public const float RotationSpeed = 0.03f; //prędkość obracania się
                        public const int RotationRandom = 60;   //o ile stopni może zmienić kierunek (zakres <0,RotationRandom> )
                        public const int RotationError = 3;
                    }
                    public static class Tail {   //ogon przekierowuje obrażenia na głowę
                        public static Texture2D Tex = Consts.Textures.Enemy_Tail;
                        public const int Damage = 1;
                        public const int Health = 1;    //ta wartość reguluje obrażenia (jest dodawana do dmg), jakie są zadawane przeciwnikowi po trafieniu w ten element
                        public const int score = 0; //nieważna wartość
                        public static Color Color = Color.ForestGreen;
                        public static Color DamagedColor = Color.Orange;
                        public const float Distance = 0.075f; //dystans to nie wartość rzeczywista, tylko odległość procentowa od następnego elementu ogona
                    }
                }
                public static class Impulse {
                    public static Texture2D Tex = Consts.Textures.Enemy_Impulse;
                    public const int Damage = 1;
                    public const int Health = 5;
                    public const int Score = 3;
                    public const float Move_MaxSpeed = 0.3f;
                    public static Color Color = Color.Teal;
                    public const double ThinkTime = 1800;
                    public const float move_Accelerate = 1.08f;
                    public const float move_Drag = 1 - 0.05f;
                    public const float move_Minimum = 0.0002f;
                }
                public static class Cannon {
                    public static Texture2D Tex = Consts.Textures.Enemy_Cannon;
                    public static Color Color = Color.Yellow;
                    public const int Damage = 8;    //obrażenia podczas zderzenia
                    public const int Health = 10;
                    public const int Score = 10;
                    public const int Series = 5;    //ilość pocisków na jedną serię
                    public const double ThinkTime = 3000;   //co jaki czas strzela
                    public const double SeriesInterval = 250;   //czas pomiędzy strzałami podczas trwania serii
                    public const bool BlockRotOnShoot = true;   //czy może się obracać podczas strzelania
                    public static class Bullet {
                        public static Texture2D Tex = Consts.Textures.Bullet;
                        public static Color Color = Color.Red;
                        public const float Speed = 0.2f;
                        public const int Damage = 1;
                    }
                }
                public static class Phantom {
                    public static Texture2D Tex = Consts.Textures.Enemy_Phantom;
                    public static Color Color = Color.GreenYellow;
                    public const int Damage = 2;
                    public const int Health = 2;
                    public const int Score = 4;
                    public const float Size = 12f;
                    public const float move_MinSpeed = 0.02f;
                    public const float move_MaxSpeed = 0.2f;
                    public const float move_Accelerate = 1.037f;
                    public const float move_Drag = 1 - 0.02f;
                    public const float Distance = 100;   //w jakiej odległości od gracza się trzyma
                }
                public static class Arsedroid {
                    public static Texture2D[] Tex = { Consts.Textures.Enemy_Arsedroid1, Consts.Textures.Enemy_Arsedroid2, Consts.Textures.Enemy_Arsedroid3 };
                    public static SoundEffect wav_Hit = Consts.Audio.Rock;
                    public const int Damage = 1;
                    public const int Health = 1;
                    public const int Score = 1;
                    public static Color Color = Color.Coral;
                    public const float Move_MaxSpeed = 0.085f;
                    public const float Move_RotSpeed = 0.01f;   //zostanie wylosowana liczba z przedziału <-Move_RotSpeed;Move_RotSpeed>
                    public const int start_Divide = 2;  //ile razy dzieli się od początku               //ilość obiektów można wyliczyć ze wzoru:
                    public const int Divide_Ammount = 10;    //na ile części dzieli się arsedroid        //Divide_Ammount^start_Divide   (2^5=32)
                }
            }
            public static class Pointers {
                public static Texture2D tex_Normal = Consts.Textures.Pointer_Arrow;
                public static Texture2D tex_InGame = Consts.Textures.Crosshair;
                public static Color col_Menu = Color.Green;
                public static Color col_Crosshair = Color.Yellow;
                public static class AuthorMenu {
                    //public static Texture2D tex_Head = Consts.Values.Enemies.Znake.Head.Tex;
                    public static Texture2D tex_Head = Consts.Values.Player.Texture;
                    public static Texture2D tex_Tail = Consts.Values.Enemies.Znake.Tail.Tex;
                    public static Color col_Head = Color.Khaki;
                    public static Color col_Tail = Color.GreenYellow;
                    public const int Lenght = 30;
                    public const float Distance = 0.15f;
                    public const double SpawnTime = 10;
                }
            }
            public static class Menu {
                /// <summary>
                /// Menu, które pojawia się po włączeniu gry
                /// </summary>
                public static class Main {
                    public static Texture2D tex_Background =    Consts.Textures.Menu.Background_Main;
                    public static Texture2D tex_but_Author = Consts.Textures.Menu.But_Main_Author;
                    public static Texture2D tex_but_Author_f = Consts.Textures.Menu.But_Main_Author_f;
                    public static Texture2D tex_but_StartGame =  Consts.Textures.Menu.But_Main_Play;
                    public static Texture2D tex_but_StartGame_f= Consts.Textures.Menu.But_Main_Play_f;
                    public static Texture2D tex_but_Settings =  Consts.Textures.Menu.But_Main_Settings;
                    public static Texture2D tex_but_Settings_f= Consts.Textures.Menu.But_Main_Settings_f;
                    public static Texture2D tex_but_Exit = Consts.Textures.Menu.But_Main_Exit;
                    public static Texture2D tex_but_Exit_f = Consts.Textures.Menu.But_Main_Exit_f;
                    public static Vector2 vec_but_Author = new Vector2( 401, 301 );
                    public static Vector2 vec_but_StartGame =   new Vector2( 200, 180 );
                    public static Vector2 vec_but_Settings =   new Vector2( 200, 354 );
                    public static Vector2 vec_but_Exit = new Vector2( 401, 354 );
                }
                public static class Author {
                    public static Texture2D tex_Background = Consts.Textures.Menu.Background_Author;
                    public static Texture2D tex_but_Back = Consts.Textures.Menu.But_Author_Back;
                    public static Texture2D tex_but_Back_f = Consts.Textures.Menu.But_Author_Back_f;
                    public static Vector2 vec_but_Back = new Vector2( 479, 377 );
                }
                public static class Settings {
                    public static Texture2D tex_Background = Consts.Textures.Menu.Background_Settings;
                    public static Texture2D tex_but_Back = Consts.Textures.Menu.But_Settings_Back;
                    public static Texture2D tex_but_Back_f = Consts.Textures.Menu.But_Settings_Back_f;
                    public static Texture2D tex_slider_b = Consts.Textures.Menu.Slider_b;
                    public static Texture2D tex_slider_s = Consts.Textures.Menu.Slider_s;
                    public static Texture2D tex_ship = Consts.Textures.ship_Avast;

                    public static Vector2 vec_but_Back = new Vector2( 460, 292 ),
                                          vec_but_Save = new Vector2( 340, 183 ),
                                          vec_but_Reset = new Vector2( 322, 118 ),
                                          vec_slider_Music = new Vector2( 260, 216 ),
                                          vec_slider_Effects = new Vector2( 260, 264 ),
                                          vec_slider_R = new Vector2( 462, 235 ),
                                          vec_slider_G = new Vector2( 462, 251 ),
                                          vec_slider_B = new Vector2( 462, 267 ),
                                          vec_Ship = new Vector2( 499, 162 );
                    public static Rectangle rec_input = new Rectangle( 260, 151, 168, 26 );
                    public static Color col_slider_R = Color.Red,
                                        col_slider_G = Color.Green,
                                        col_slider_B = Color.Blue,
                                        col_slider = Color.YellowGreen,
                                        col_input = new Color( 0x63, 0xd0, 0x4f );
                    public const int slider_width_b = 158,
                                     slider_width_s = 70,
                                     input_maxChar = 10;
                }
                public static class Ships {
                    public static Texture2D tex_Background = Consts.Textures.Menu.Background_Ships,
                                            tex_but_Back = Consts.Textures.Menu.But_Ships_Back,
                                            tex_but_Back_f = Consts.Textures.Menu.But_Ships_Back_f,
                                            tex_but_Help = Consts.Textures.Menu.But_Ships_Help,
                                            tex_but_Help_f = Consts.Textures.Menu.But_Ships_Help_f,
                                            tex_but_Next = Consts.Textures.Menu.But_Ships_Next,
                                            tex_but_Next_f = Consts.Textures.Menu.But_Ships_Next_f,
                                            tex_but_Choose = Consts.Textures.Menu.But_Ships_Choose,
                                            tex_but_Choose_f = Consts.Textures.Menu.But_Ships_Choose_f,
                                            tex_but_Buy = Consts.Textures.Menu.But_Ships_Buy,
                                            tex_but_Buy_f = Consts.Textures.Menu.But_Ships_Buy_f,
                                            tex_but_Buy_l = Consts.Textures.Menu.But_Ships_Buy_l,
                                            tex_slider = Consts.Textures.Menu.Slider_page,
                                            tex_interface_User = Consts.Textures.Menu.Interface_Ships_User,
                                            tex_interface_Ship = Consts.Textures.Menu.Interface_Ships_Ship,
                                            tex_bar_User_Exp = Consts.Textures.Menu.Bar_Ships_User_Exp,
                                            tex_bar_Ship_Exp = Consts.Textures.Menu.Bar_Ships_Ship_Exp,
                                            tex_bar_Stats = Consts.Textures.Menu.Bar_Ships_Ship_Stats;
                    public static Vector2 vec_but_Back = new Vector2( 600, 0 ),
                                          vec_but_Help = new Vector2( 500, 0 ),
                                          vec_but_Next = new Vector2( 700, 0 ),
                                          vec_but_ChooseBuy = new Vector2( 43, 195 ),
                                          vec_but_Upgrade_Speed = new Vector2( 351,152 ),
                                          vec_but_Upgrade_Agility = new Vector2( 351, 187 ),
                                          vec_but_Upgrade_Health = new Vector2( 351, 221 ),
                                          vec_slider = new Vector2( 0, 133 ),
                                          vec_image_mid = new Vector2( 61, 170 ),
                                          vec_interface_User = new Vector2( 0 ),
                                          vec_interface_Ship = new Vector2( 24, 127 ),
                                          vec_interface_Ship_Offset = new Vector2( 0, 20 ),
                                          vec_interface_Mask = Vector2.Zero,
                                          vec_string_Nick = new Vector2( 90, 45 ),
                                          vec_string_User_Level = new Vector2( 7, -4 ),
                                          vec_string_User_Level_Percent = new Vector2( 245, -3 ),
                                          vec_string_Ship_Name = new Vector2( 140, 145 ),
                                          vec_string_Ship_Level = new Vector2( 215, 202 ),
                                          vec_string_Ship_Cost = new Vector2( 242, 182 ),
                                          vec_string_Stats_Speed_Level = new Vector2( 280, 146 ),
                                          vec_string_Stats_Agility_Level = new Vector2( 280, 181 ),
                                          vec_string_Stats_Health_Level = new Vector2( 280, 213 ),
                                          vec_string_Stats_Speed_Cost = new Vector2( 346, 151 ),
                                          vec_string_Stats_Agility_Cost = new Vector2( 346, 186 ),
                                          vec_string_Stats_Health_Cost = new Vector2( 346, 220 ),
                                          vec_string_Money = new Vector2( 79, 73 ),
                                          vec_bar_User_Experience = new Vector2( 102, 23 ),
                                          vec_bar_Ship_Experience = new Vector2( 151, 183 ),
                                          vec_bar_Stats_Speed = new Vector2( 362, 140 ),
                                          vec_bar_Stats_Agility = new Vector2( 362, 175 ),
                                          vec_bar_Stats_Health = new Vector2( 362, 210 );
                    private static Color cut_code = new Color( 0x80, 0xff, 0x00 );  //wszędzie powinien być ten sam kolor, utnę trochę kodu...
                    public static Color col_slider = Color.White,
                                        col_Experience = cut_code,
                                        col_Money = cut_code,
                                        col_string_Nick = cut_code,
                                        col_string_Avaible = cut_code,
                                        col_string_NotAvaible = new Color( 0xfa, 0x39, 0x2b ),
                                        col_bar_Stats_Speed = Color.Yellow,
                                        col_bar_Stats_Agility = Color.Orange,
                                        col_bar_Stats_Health = Color.IndianRed,
                                        col_bar_Stats_Upgrade = Color.Gray,
                                        col_bar_Ship_Experience = Color.White;
                    public static Rectangle rec_View_Menu = new Rectangle( 0, 118, 504, 362 );
                    public static SpriteFont font_Nick = Consts.Fonts.Nick,
                                             font_User_Level = Consts.Fonts.UserLevel,
                                             font_Money = Consts.Fonts.UserMoney,
                                             font_Level_Percent = Consts.Fonts.LevelPercent,
                                             font_Ship_Name = Consts.Fonts.ShipName,
                                             font_Ship_Level = Consts.Fonts.ShipLevel;
                    public const int slider_height = 303;
                }
            }
            public static class Ships {
                public const int reference_Health = 15;
                public const float reference_Speed = 3f;
                public const float reference_Agility = 0.01f;
                public static int[] tableExperience = { 50, 160, 280, 500, 840 };
                
                private static float[] avast_speed = { 0.4f, 0.50f, 0.6f };
                private static float[] avast_accelerate = { 0.001f, 0.0015f, 0.002f };
                private static float[] avast_drag = { 0.50f, 0.55f, 0.6f };
                private static int[]   avast_health = { 8, 12, 15 };
                private static int[]   avast_level_speed= { 1, 2, 4 };
                private static int[]   avast_level_agility= { 1, 2, 4 };
                private static int[]   avast_level_health= { 1, 2, 4 };
                private static int[]   avast_cost_speed = { 500, 1750, 3000 };
                private static int[]   avast_cost_agility = { 500, 1750, 3000 };
                private static int[]   avast_cost_health = { 500, 1750, 3000 };
                private static Ship Avast = new Ship() {
                    Name = "Avast",
                    Texture = Consts.Textures.ship_Avast,
                    Cost = 0,
                    LevelCost = 0,
                    tableHealth = avast_health,
                    tableSpeed = avast_speed,
                    tableAccelerate = avast_accelerate,
                    tableDrag = avast_drag,
                    LevelSpeed = 0,                    
                    LevelAgility = 0,
                    LevelHealth = 0,
                    Experience = 25,
                    needLevelSpeed = avast_level_speed,
                    needLevelAgility = avast_level_agility,
                    needLevelHealth = avast_level_health,
                    costSpeed = avast_cost_speed,
                    costAgility = avast_cost_agility,
                    costHealth = avast_cost_health
                };
                private static float[] avg_speed = { 0.85f, 0.90f, 1f };
                private static float[] avg_accelerate = { 0.003f, 0.0035f, 0.004f };
                private static float[] avg_drag = { 0.50f, 0.55f, 0.6f };
                private static int[]   avg_health = { 2, 3, 4 };
                private static int[]   avg_level_speed= { 1, 2, 4 };
                private static int[]   avg_level_agility= { 1, 2, 4 };
                private static int[]   avg_level_health= { 1, 1, 3 };
                private static int[]   avg_cost_speed = { 500, 1750, 3000 };
                private static int[]   avg_cost_agility = { 500, 1750, 3000 };
                private static int[]   avg_cost_health = { 500, 1750, 3000 };
                private static Ship AVG = new Ship() {
                    Name = "AVG",
                    Texture = Consts.Textures.ship_Avg,
                    Cost = 10000,
                    LevelCost = 3,
                    tableHealth = avg_health,
                    tableSpeed = avg_speed,
                    tableAccelerate = avg_accelerate,
                    tableDrag = avg_drag,
                    LevelSpeed = 0,
                    LevelAgility = 0,
                    LevelHealth = 0,
                    Experience = 0,
                    needLevelSpeed = avg_level_speed,
                    needLevelAgility = avg_level_agility,
                    needLevelHealth = avg_level_health,
                    costSpeed = avg_cost_speed,
                    costAgility = avg_cost_agility,
                    costHealth = avg_cost_health
                };
                private static float[] norton_speed = { 0.85f, 0.90f, 1f };
                private static float[] norton_accelerate = { 0.003f, 0.0035f, 0.004f };
                private static float[] norton_drag = { 0.50f, 0.55f, 0.6f };
                private static int[]   norton_health = { 2, 3, 4 };
                private static int[]   norton_level_speed= { 1, 2, 4 };
                private static int[]   norton_level_agility= { 1, 2, 4 };
                private static int[]   norton_level_health= { 1, 1, 3 };
                private static int[]   norton_cost_speed = { 500, 1750, 3000 };
                private static int[]   norton_cost_agility = { 500, 1750, 3000 };
                private static int[]   norton_cost_health = { 500, 1750, 3000 };
                private static Ship Norton = new Ship() {
                    Name = "Norton",
                    Texture = Consts.Textures.ship_Norton,
                    Cost = 9990,
                    LevelCost = 3,
                    tableHealth = norton_health,
                    tableSpeed = norton_speed,
                    tableAccelerate = norton_accelerate,
                    tableDrag = norton_drag,
                    LevelSpeed = 0,
                    LevelAgility = 0,
                    LevelHealth = 0,
                    Experience = 0,
                    needLevelSpeed = norton_level_speed,
                    needLevelAgility = norton_level_agility,
                    needLevelHealth = norton_level_health,
                    costSpeed = norton_cost_speed,
                    costAgility = norton_cost_agility,
                    costHealth = norton_cost_health
                };
                private static float[] spybot_speed = { 0.85f, 0.90f, 1f };
                private static float[] spybot_accelerate = { 0.003f, 0.0035f, 0.004f };
                private static float[] spybot_drag = { 0.50f, 0.55f, 0.6f };
                private static int[]   spybot_health = { 2, 3, 4 };
                private static int[]   spybot_level_speed= { 1, 2, 4 };
                private static int[]   spybot_level_agility= { 1, 2, 4 };
                private static int[]   spybot_level_health= { 1, 1, 3 };
                private static int[]   spybot_cost_speed = { 500, 1750, 3000 };
                private static int[]   spybot_cost_agility = { 500, 1750, 3000 };
                private static int[]   spybot_cost_health = { 500, 1750, 3000 };
                private static Ship Spybot = new Ship() {
                    Name = "Spybot",
                    Texture = Consts.Textures.ship_Avast,
                    Cost = 9990,
                    LevelCost = 3,
                    tableHealth = spybot_health,
                    tableSpeed = spybot_speed,
                    tableAccelerate = spybot_accelerate,
                    tableDrag = spybot_drag,
                    LevelSpeed = 0,
                    LevelAgility = 0,
                    LevelHealth = 0,
                    Experience = 0,
                    needLevelSpeed = spybot_level_speed,
                    needLevelAgility = spybot_level_agility,
                    needLevelHealth = spybot_level_health,
                    costSpeed = spybot_cost_speed,
                    costAgility = spybot_cost_agility,
                    costHealth = spybot_cost_health
                };
                public static Ship[] Array = { Avast, AVG, Norton, Spybot };
            }
            public static class Stages {
                private static StageData lvl1 = new StageData() {
                    Name = "Czyszczenie dysku",
                    award_Exp = 25,
                    award_Money = 700,
                    Description = "Pomóż kumplowi wyczyścić kompa!",
                    Image = Consts.Textures.Stages.Image_1,

                    Background = Consts.Textures.GameBackground1,
                    Script = new Script_1(),
                    Size = new Vector2(900,600)
                };
                private static StageData lvl2 = new StageData() {
                    Name = "Skrzynka mailowa",
                    award_Exp = 85,
                    award_Money = 1100,
                    Description = "Pewien administrator (...) ...Usuń spam!",
                    Image = Consts.Textures.Stages.Image_1,

                    Background = Consts.Textures.GameBackground1,
                    Script = new Script_1(),
                    Size = new Vector2( 900, 600 )
                };
                private static StageData lvl3 = new StageData() {
                    Name = "Internetowe Prześladowanie",
                    award_Exp = 1,
                    award_Money = 100,
                    Description = "OOOOOOOOO1OOOOOOOOO2OOOOOOOOO3OOOOOOOOO\n4OOOOOOOOO5OOOOOOOOO6OOOOOOOOO7OOOOOOOOO\n8OOOOOOOOO9",
                    Image = Consts.Textures.Stages.Image_1,

                    Background = Consts.Textures.GameBackground1,
                    Script = new Script_1(),
                    Size = new Vector2( 900, 600 )
                };
                private static StageData lvlx = new StageData() {
                    Name = "#ERROR",
                    award_Exp = 1,
                    award_Money = 100,
                    Description = "Copy > Paste",
                    Image = Consts.Textures.Stages.Image_1,

                    Background = Consts.Textures.GameBackground1,
                    Script = new Script_1(),
                    Size = new Vector2( 900, 600 )
                };
                public static StageData[] Array = { lvl1, lvl2, lvl3, lvlx, lvlx, lvlx, lvlx, lvlx, lvlx, lvlx };
            }
        }
    }
}
