using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace PewPew {
    public enum GameState {
        Playing = 0,
        Win,
        Defeat
    }
    public interface ICameraDrawable {
        Texture2D Texture { get; }
        Vector2 Position { get; }
        Color Color { get; }
        float Angle { get; }
        bool SpecialDraw { get; }   //czy do rysowania tego obiektu wykorzystywana jest funkcja ICameraDrawable.Draw()
        bool Draw(SpriteBatch spriteBatch, GameTime gameTime);
    }
    public interface IGameEntity : ICameraDrawable {
        float Size { get; } //promień
        bool Check();
        void Update(GameTime gameTime);
    }
    public interface IEnemy : IGameEntity {
        int Health { get; }
        int Score { get; }
        int Damage { get; }
        void Damaged(int value);
    }
    /// <summary>
    /// Każda plansza ma polimorfona obiekt, którego
    /// rodziem jest StageScript. Ma on dostęp do danych
    /// planszy i jest odpowiedzialny za dodawanie przeciwników,
    /// oraz ewentualne inne działania, specyficzne dla każdego
    /// poziomu.
    /// </summary>
    public interface IStageScript {
        void Init(Stage stage);
        GameState Update(GameTime gameTime);
        void Draw(SpriteBatch spriteBatch, GameTime gameTime);
    }
    /// <summary>
    /// Obiekty tego typu zawierają informacje
    /// o mapie, dzięki którym można ją wyświetlić
    /// w menu, oraz na jego podstawie stworzyć
    /// obiekt typu Stage
    /// </summary>
    public class StageData {
        //dane potrzebne do menu
        public string Name = "",
                      Description = "";
        public bool Passed = false;
        public int award_Exp,
                   award_Money;
        public Texture2D Image;

        //dane potrzebne do inicjalizacji gry
        public IStageScript Script;
        public Texture2D Background;
        public Vector2 Size;
    }
    public class Stage {
        private static readonly Color
            alphaRed = new Color( 255, 0, 0, 40 ),
            alphaGreen = new Color( 0, 255, 0, 20 );

        private Texture2D m_Tex;
        private Color m_Color;
        private IStageScript m_Script;
        public Player Player;
        public Camera Camera;

        //listy przechowujące obiekty gry
        private List<ICameraDrawable> m_Effects;
        private List<IGameEntity> m_Entities;
        private List<Obstacle> m_Obstacles;

        private bool block;
        private int m_Score;
        private GameState m_GameState;
        private double m_ElapsedTime = 0;
        
        public int Score { get { return this.m_Score; } }
        public Vector2 Size { get { return this.Camera.World; } }
        public GameState GameState { get { return this.m_GameState; } }
        public IEnemy[] Enemies {
            get {
                List<IEnemy> list = new List<IEnemy>();
                foreach( IGameEntity enem in this.m_Entities ) {
                    if( enem is IEnemy )
                        list.Add( ( IEnemy ) enem );
                }
                return list.ToArray();
            }
        }
        public int Minutes { get { return ( int ) Math.Floor( this.m_ElapsedTime / 60 ); } }
        public int Seconds { get { return ( int ) ( this.m_ElapsedTime % 60 ); } }
        public double TotalSeconds { get { return this.m_ElapsedTime % 60; } }
        public double TotalMinutes { get { return this.m_ElapsedTime / 60; } }

        public Stage(Texture2D tex, IStageScript script, Vector2 size) {
            this.m_Tex = tex;
            this.m_Color = Color.DarkBlue;
            this.m_Script = script;
            this.m_Script.Init( this );
            this.Player = new Player( this, User.Ships[User.Ship] );
            this.Camera = new Camera( this, size );
            this.m_Score = 0;

            this.m_Effects = new List<ICameraDrawable>();
            this.m_Entities = new List<IGameEntity>();
            this.m_Obstacles = new List<Obstacle>();
            this.m_GameState = GameState.Playing;

            this.block = false;
            //Consts.AudioManager.Play( Consts.Audio.Music );
        }
        public Stage(StageData data)
            : this( data.Background, data.Script, data.Size ) {
        }

        public Menus.MenuState Update(GameTime gameTime) {
            if( this.m_GameState == GameState.Playing ) {
                this.Player.Update( gameTime );
                this.m_GameState = this.m_Script.Update( gameTime );
            }
            this.Camera.Update( gameTime );
            this.CheckEntities();
            this.UpdateEntities( gameTime );
            KeyboardState klawiatura = Keyboard.GetState();
            if( klawiatura.IsKeyDown( Keys.NumPad0 ) ) {
                if( !this.block ) {
                    this.AddEntity( new Enemies.Chaos( this, new Vector2( 300 ) ) );
                }
                this.block = true;
            } else if( klawiatura.IsKeyDown( Keys.NumPad1 ) ) {
                if( !this.block ) {
                    this.AddEntity( new Enemies.Impulse( this, new Vector2( 300 ) ) );
                }
                this.block = true;
            } else if( klawiatura.IsKeyDown( Keys.NumPad2 ) ) {
                if( !this.block ) {
                    this.AddEntity( new Enemies.Cannon( this, new Vector2( 300 + Consts.Random.Next( -150, 150 ), 300 + Consts.Random.Next( -150, 150 ) ) ) );
                }
                this.block = true;
            } else if( klawiatura.IsKeyDown( Keys.NumPad3 ) ) {
                if( !this.block ) {
                    this.AddEntity( new Enemies.Znake( this, new Vector2( 300 ) ) );
                }
                this.block = true;
            } else if( klawiatura.IsKeyDown( Keys.NumPad4 ) ) {
                if( !this.block ) {
                    this.AddEntity( new Enemies.Arsedroid( this, new Vector2( 300 ) ) );
                }
                this.block = true;
            } else if( klawiatura.IsKeyDown( Keys.NumPad5 ) ) {
                if( !this.block ) {
                    this.AddEntity( new Enemies.Phantom( this, new Vector2( 300 ) ) );
                }
                this.block = true;
            } else {
                this.block = false;
            }
            if( klawiatura.IsKeyDown( Keys.Escape ) )
                return Menus.MenuState.START;
            this.m_ElapsedTime += gameTime.ElapsedGameTime.TotalSeconds;

            return Menus.MenuState.PLAY;
        }
        public void Draw(SpriteBatch spriteBatch, GameTime gameTime) {
            //tło
            spriteBatch.Draw( this.m_Tex, -this.Camera.Position, this.m_Color );
            //przeszkody
            for( int i = 0; i < this.m_Obstacles.Count(); ++i )
                m_Obstacles[i].Draw( spriteBatch, this.Camera.Position );
            //gracz
            if( m_GameState == GameState.Playing )
                this.Player.Draw( spriteBatch, gameTime );
            //obiekty (przeciwnicy, pociski, etc.)
            for( int i = 0; i < this.m_Entities.Count(); ++i )
                if( this.m_Entities[i] != null )
                    if( this.m_Entities[i].SpecialDraw )
                        this.m_Entities[i].Draw( spriteBatch, gameTime );
                    else
                        this.Camera.Draw( spriteBatch, this.m_Entities[i] );
            for( int i = 0; i < this.m_Effects.Count(); ++i )
                if( this.m_Effects[i] != null )
                    if( this.m_Effects[i].SpecialDraw ) {
                        if( !this.m_Effects[i].Draw( spriteBatch, gameTime ) )
                            this.m_Effects[i] = null;
                    } else
                        this.Camera.Draw( spriteBatch, this.m_Effects[i] );
            //interface
            this.Camera.Draw( spriteBatch, gameTime );
            this.m_Script.Draw( spriteBatch, gameTime );

            if( this.m_GameState == GameState.Defeat )
                spriteBatch.Draw( Consts.Textures.Dot, Consts.GraphicsDevice.Viewport.TitleSafeArea, alphaRed );
            if( this.m_GameState == GameState.Win )
                spriteBatch.Draw( Consts.Textures.Dot, Consts.GraphicsDevice.Viewport.TitleSafeArea, alphaGreen );
        }
        public bool AllowMove(Vector2 from, Vector2 to) {
            for( int i = 0; i < this.m_Obstacles.Count(); ++i )
                if( this.m_Obstacles[i].Intersect( from, to ) )
                    return false;
            return true;
        }

        public void EnemyKilled(IEnemy enemy) {
            this.m_Score += enemy.Score;
            this.m_Effects.Add( new PopUp( this.Camera, Consts.Values.PopUps.Score.Font, Consts.Values.PopUps.Score.Prefix + enemy.Score.ToString(), Consts.Values.PopUps.Score.Color, enemy.Position, Consts.Values.PopUps.Score.Time, Consts.Values.PopUps.Score.Distance, Consts.Values.PopUps.Score.Direction ) );
        }
        public void AddEntity(IGameEntity Entity) { this.m_Entities.Add( Entity ); }
        public void AddObstacle(Obstacle obstacle) { this.m_Obstacles.Add( obstacle ); }
        public IEnemy[] GetEnemies(Vector2 position, float range) {
            List<IGameEntity> result = this.m_Entities.FindAll(
                delegate(IGameEntity enemy) {
                    if( !( enemy is IEnemy ) )
                        return false;
                    if( Vector2.Distance( position, enemy.Position ) <= range + enemy.Size )
                        return true;
                    else
                        return false;
                } );
            IEnemy[] enemies = new IEnemy[result.Count()];
            int i = 0;
            foreach( IEnemy enemy in result )
                enemies[i++] = enemy;
            return enemies;
        }
        public void Defeat() {
            this.m_GameState = GameState.Defeat;
            User.Money += 250;
            User.Experience += 30;
        }

        private void CheckEntities() {
            for( int i = 0; i < m_Entities.Count(); ++i )
                if( m_Entities[i] != null )
                    if( !m_Entities[i].Check() )
                        m_Entities.RemoveAt( i );
        }
        private void UpdateEntities(GameTime gameTime) {
            for( int i = 0; i < m_Entities.Count(); ++i )
                if( m_Entities[i] != null )
                    m_Entities[i].Update( gameTime );
        }
    }


    public class Script_1 : IStageScript {
        private static readonly double[]
            spawnTime = { 4, 10, 17, 25, 35 };
        private static readonly int[]
            spawnAmmount = { 10, 20, 35, 50, 80 };
        private static readonly Vector2[]
            spawnPoints = { new Vector2( 300, 400 ), new Vector2( 600, 100 ), new Vector2( 800, 100 ), new Vector2( 150, 600 ), new Vector2( 800, 200 ) };
        private Stage m_Stage;
        private int m_CurrentSpawn = 0;

        public Script_1() {
        }
        public void Init(Stage stage) {
            this.m_Stage = stage;
        }
        public GameState Update(GameTime gameTime) {
            if( this.m_CurrentSpawn < spawnTime.Count() && gameTime.TotalGameTime.TotalSeconds >= spawnTime[this.m_CurrentSpawn] ) {
                for( int i = 0; i < spawnAmmount[this.m_CurrentSpawn]; ++i ) 
                    this.m_Stage.AddEntity( new Enemies.Chaos( this.m_Stage, spawnPoints[this.m_CurrentSpawn] ) );
                ++this.m_CurrentSpawn;
            }

            if( gameTime.TotalGameTime.TotalSeconds >= spawnTime[spawnTime.Count() - 1] &&
                this.m_Stage.Enemies.Count() == 0 )
                return GameState.Win;

            return GameState.Playing;
        }
        public void Draw(SpriteBatch spriteBatch, GameTime gameTime) {
            spriteBatch.DrawString( Consts.Fonts.font1, this.m_CurrentSpawn.ToString()+"\n"+this.m_Stage.Enemies.Count().ToString(), new Vector2( 20, 450 ), Color.Red );
        }
    }
}
