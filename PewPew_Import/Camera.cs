using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PewPew {
    public class Camera {
        private static readonly Vector2
            vec_Timer = new Vector2( 400, 10 );
        private static readonly Color
            col_Timer = Color.YellowGreen;
        private static readonly SpriteFont
            font_Timer = Consts.Fonts.calculator;
        private Stage m_Stage;
        private Vector2 m_World;
        private Vector2 m_Pos;
        private Curve m_Krzywa;
        private Vector2[] m_Vectors;

        public Vector2 World { get { return this.m_World; } }
        public Vector2 Position { get { return this.m_Pos; } }
        public Vector2 MidPoint { get { return new Vector2( this.m_Pos.X + Consts.GraphicsDevice.Viewport.Width / 2, this.m_Pos.Y + Consts.GraphicsDevice.Viewport.Height / 2 ); } }
        public GameTime zegar;

        public Camera(Stage stage, Vector2 world) {
            this.m_Stage = stage;
            this.m_World = world;
            this.m_Pos = new Vector2();

            this.m_Krzywa = new Curve();

            this.m_Krzywa.Keys.Add( new CurveKey( 0, 20 ) );
            this.m_Krzywa.Keys.Add( new CurveKey( 10, 40 ) );
            this.m_Krzywa.Keys.Add( new CurveKey( 20, 10 ) );
            this.m_Krzywa.Keys.Add( new CurveKey( 30, 70 ) );
            this.m_Krzywa.Keys.Add( new CurveKey( 40, -10 ) );
            this.m_Vectors = new Vector2[this.m_Krzywa.Keys.Count()];
            this.zegar = new GameTime();

            for( int i = 0; i < this.m_Krzywa.Keys.Count(); ++i )
                this.m_Vectors[i] = new Vector2( this.m_Krzywa.Keys[i].Position, this.m_Krzywa.Keys[i].Value );

        }

        public void Update(GameTime gameTime) {
            this.AutoMove( gameTime );
        }
        public void Draw(SpriteBatch spriteBatch, GameTime gameTime) {
            //poziom zdrowia i życia
            spriteBatch.Draw( Consts.Textures.Interface.Health, Consts.Values.GameInteface.vec_health, Consts.Values.GameInteface.color_health_image );
            spriteBatch.Draw( Consts.Textures.Interface.Health_Bar, Consts.Values.GameInteface.vec_health_bar, Consts.Values.GameInteface.color_health_bar );
            int width = ( int ) ( ( ( float ) this.m_Stage.Player.Health / this.m_Stage.Player.MaxHealth ) * Consts.Values.GameInteface.rec_health_bar_fill.Width );
            Rectangle rec = Consts.Values.GameInteface.rec_health_bar_fill;
            rec.Width = width;
            spriteBatch.Draw( Consts.Textures.Dot, rec, Consts.Values.GameInteface.color_health_bar_fill );
            for( int i = 0; i < ( ( this.m_Stage.Player.Lives > Consts.Values.GameInteface.n_max_visible_lives ) ? Consts.Values.GameInteface.n_max_visible_lives : this.m_Stage.Player.Lives ); ++i )
                spriteBatch.Draw( Consts.Textures.Interface.Live, Consts.Values.GameInteface.vec_firsLive + i * Consts.Values.GameInteface.vec_liveOffset, Consts.Values.GameInteface.color_live );

            //punkty
            string txt = Consts.Values.GameInteface.str_score;
            for( int i = 0; i < Consts.Values.GameInteface.n_max_blanc_letter_in_score; ++i )
                txt += "8";
            spriteBatch.DrawString( Consts.Fonts.calculator, txt, Consts.Values.GameInteface.vec_score, Consts.Values.GameInteface.color_score_blanc );
            txt = Consts.Values.GameInteface.str_score;
            if( Consts.Values.GameInteface.n_max_blanc_letter_in_score - this.m_Stage.Score.ToString().Count() > 1 ) {
                txt += "[color:" + Functions.ToHex( Consts.Values.GameInteface.color_score_blanc, true ) + "]";
                for( int i = 0; i < Consts.Values.GameInteface.n_max_blanc_letter_in_score - this.m_Stage.Score.ToString().Count(); ++i )
                    txt += "8";
                txt += "[/color]";
            }
            txt += this.m_Stage.Score.ToString();
            Functions.DrawColorFormattedText( spriteBatch, Consts.Fonts.calculator, Consts.Values.GameInteface.vec_score, txt, Consts.Values.GameInteface.color_score );

            //licznik czasu
            string time = this.m_Stage.Minutes.ToString() + ":" + this.m_Stage.Seconds.ToString();
            Functions.DrawStringAlignMiddle( spriteBatch, font_Timer, time, vec_Timer, col_Timer );

            //tymczasowe//debugging

            spriteBatch.DrawString( Consts.Fonts.font1, "Zegar:\n" + this.zegar.ToString() + "\n" + this.zegar.TotalGameTime.Seconds.ToString(), new Vector2( 720, 180 ), Color.YellowGreen );
            spriteBatch.DrawString( Consts.Fonts.font1, "0-Chaos\n1-Impulse\n2-Cannon\n3-Znake\n4-Arsedroid\n5-Phantom", new Vector2( 720, 380 ), Color.YellowGreen );
        }

        public bool BackOnScreen(ref Vector2 vec, ref int sum) {   //jeśli vec 2 znajduje się poza ekranem, cofa go, jako zwrat mówi, czy obiekt był poza ekranem
            bool ret = false;
            if( vec.X < 0 ) { vec.X = 0; ret = true; sum |= Consts.Values.LEFT_EDGE; }
            if( vec.X > this.m_World.X ) { vec.X = this.m_World.X; ret = true; sum |= Consts.Values.RIGHT_EDGE; }
            if( vec.Y < 0 ) { vec.Y = 0; ret = true; sum |= Consts.Values.TOP_EDGE; }
            if( vec.Y > this.m_World.Y ) { vec.Y = this.m_World.Y; ret = true; sum |= Consts.Values.BOTTOM_EDGE; }
            return ret;
        }
        public Vector2 GetMousePosition() {
            return Functions.GetMousePosition() + this.Position;
        }
        public void Draw(SpriteBatch spriteBatch, ICameraDrawable drawMe) {
            spriteBatch.Draw( drawMe.Texture, new Rectangle( ( int ) ( drawMe.Position.X - this.Position.X ), ( int ) ( drawMe.Position.Y - this.Position.Y ), drawMe.Texture.Width, drawMe.Texture.Height ), null, drawMe.Color, MathHelper.ToRadians( drawMe.Angle ), new Vector2( drawMe.Texture.Width / 2, drawMe.Texture.Height / 2 ), SpriteEffects.None, 1f );
        }

        private void AutoMove(GameTime gameTime) {   //kamera sprawdza, jak daleko znajduje się postać gracza od środka ekranu
            //następnie przesuwa się w kierunku gracza; im dalej gracz jest od środka
            //ekranu, tym szybciej porusza się kamera;
            float time = ( float ) gameTime.ElapsedGameTime.TotalMilliseconds;
            Vector2 player = this.m_Stage.Player.Position;
            this.m_Pos -= ( this.MidPoint - player ) * Consts.Values.Camera.Speed * time;

            //powrot kamery, jeśli wyjeżdża poza zasięg planszy
            Vector2 pos = this.m_Pos;
            if( this.m_Pos.X < 0 )
                this.m_Pos.X = 0;
            if( this.m_Pos.X + Consts.Values.Camera.Bounding.Width > this.m_World.X )
                this.m_Pos.X = this.m_World.X - Consts.Values.Camera.Bounding.Width;
            if( this.m_Pos.Y < 0 )
                this.m_Pos.Y = 0;
            if( this.m_Pos.Y + Consts.Values.Camera.Bounding.Height > this.m_World.Y )
                this.m_Pos.Y = this.m_World.Y - Consts.Values.Camera.Bounding.Height;
        }
    }
}
