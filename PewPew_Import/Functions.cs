using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace PewPew {
    public static class Functions {
        #region Funkcje Rysujące
        public static void DrawVector( SpriteBatch spriteBatch, Vector2 where, int size, Color color ) {
            Rectangle rec = new Rectangle( ( int )where.X - size / 2, ( int )where.Y - size / 2, size, size );
            spriteBatch.Draw( Consts.Textures.Dot, rec, color );
        }
        public static void DrawCircle( SpriteBatch spriteBatch, Vector2 O, float range, int total, int pointsize, Color color ) {
            Vector2 point = new Vector2();
            for( int i = 1; i <= total; ++i ) {
                point = Functions.CirclePoint( O, range, total, i );
                Functions.DrawVector( spriteBatch, point, pointsize, color );
            }
        }
        public static void DrawRegularPolygon( SpriteBatch spriteBatch, Vector2 O, float range, int total, int thickness, Color color ) {
            Vector2[] points = new Vector2[total];
            Line line = new Line( Vector2.Zero, Vector2.Zero, thickness, color );
            for( int i=0; i < total; ++i )
                points[i] = Functions.CirclePoint( O, range, total, i+1 );
            for( int i=0; i < total; ++i ) {
                Vector2 p = points[i];
                Vector2 pn;
                if( i + 1 < total ) {
                    pn = points[i + 1];
                } else {
                    pn = points[0];
                }
                line.p1 = p; line.p2 = pn;
                line.Update();
                line.Draw( spriteBatch );
                //Functions.DrawVector( spriteBatch, p, 2, color );
            }
        }
        public static void DrawRectangle( SpriteBatch spriteBatch, Rectangle rec, Color color ) {
            spriteBatch.Draw( Consts.Textures.Dot, rec, color );
        }
        public static void DrawBounds( SpriteBatch spriteBatch, Rectangle rec, int thickness, Color color ) {
            Vector2[] corners = new Vector2[4];
            corners[0] = new Vector2( rec.X, rec.Y );
            corners[1] = corners[0] + new Vector2( rec.Width, 0 );
            corners[2] = corners[1] + new Vector2( 0, rec.Height );
            corners[3] = corners[0] + new Vector2( 0, rec.Height );
            Line line = new Line( corners[0], corners[1], thickness, color );
            line.Update(); line.Draw( spriteBatch );
            line.p1 = corners[1]; line.p2 = corners[2];
            line.Update(); line.Draw( spriteBatch );
            line.p1 = corners[2]; line.p2 = corners[3];
            line.Update(); line.Draw( spriteBatch );
            line.p1 = corners[3]; line.p2 = corners[0];
            line.Update(); line.Draw( spriteBatch );
        }
        public static void DrawColorFormattedText(SpriteBatch spriteBatch, SpriteFont font, Vector2 position, string text, Color defaultcolor)
        {   //źródło:http://superiorcode.com/blog/?p=141
            Color defaultColor = defaultcolor;
            // only bother if we have color commands involved
            if (text.Contains("[color:"))
            {
                // how far in x to offset from position
                int currentOffset = 0;

                // example: 
                // string.Format("You attempt to hit the [color:#FFFF0000]{0}[/color] but [color:{1}]MISS[/color]!", 
                // currentMonster.Name, Color.Red.ToHex(true));
                string[] splits = text.Split(new string[] { "[color:" }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var str in splits)
                {
                    // if this section starts with a color
                    if (str.StartsWith("#"))
                    {
                        // #AARRGGBB
                        // #FFFFFFFFF
                        // #123456789
                        string color = str.Substring(0, 9);

                        // any subsequent msgs after the [/color] tag are defaultColor
                        string[] msgs = str.Substring(10).Split(new string[] { "[/color]" }, StringSplitOptions.RemoveEmptyEntries);

                        // always draw [0] there should be at least one
                        spriteBatch.DrawString(font, msgs[0], position + new Vector2(currentOffset, 0), color.ToColor());
                        currentOffset += (int)font.MeasureString(msgs[0]).X;

                        // there should only ever be one other string or none
                        if (msgs.Length == 2)
                        {
                            spriteBatch.DrawString(font, msgs[1], position + new Vector2(currentOffset, 0), defaultColor);
                            currentOffset += (int)font.MeasureString(msgs[1]).X;
                        }
                    }
                    else
                    {
                        spriteBatch.DrawString(font, str, position + new Vector2(currentOffset, 0), defaultColor);
                        currentOffset += (int)font.MeasureString(str).X;
                    }
                }
            }
            else
            {
                // just draw the string as ordered
                spriteBatch.DrawString(font, text, position, defaultColor);
            }
        }
        public static void DrawStringAlignMiddle(SpriteBatch spriteBatch, SpriteFont font, string text, Vector2 position, Color color) {
            Vector2 pos = position - new Vector2( font.MeasureString( text ).X / 2, 0 );
            spriteBatch.DrawString( font, text, pos, color );
        }
        public static void DrawStringAlignRight(SpriteBatch spriteBatch, SpriteFont font, string text, Vector2 position, Color color) {
            Vector2 pos = position - new Vector2( font.MeasureString( text ).X, 0 );
            spriteBatch.DrawString( font, text, pos, color );
        }
        #endregion
        public static Rectangle TexToRec(Texture2D tex) {
            return new Rectangle( 0, 0, tex.Width, tex.Height );
        }
        public static Vector2 TexToVec(Texture2D tex) {
            return new Vector2( tex.Width, tex.Height );
        }
        public static bool GetSign(float val) {
            if( val < 0 )
                return true;
            else
                return false;
        }
        public static bool Intersects( Rectangle rectangle, Vector2 point ){ //sprawdza, czy punkt znajduje się wewnątrz prostokątu
            if( ( int )point.X < rectangle.Left || 
                ( int )point.X > rectangle.Right ||
                ( int )point.Y < rectangle.Top ||
                ( int )point.Y > rectangle.Bottom )
                return false;
            else
                return true;
        }
        public static float Det(Vector2 A, Vector2 B, Vector2 C)
        {   //det = wyznacznik macierzy
            //Wygląda strasznie, a tak na prawdę, jest jeszcze straszniejsze... ( MNOŻENIE MACIERZY ~[DOOM DOOM DOOM]~ )
            return A.X * B.Y + B.X * C.Y + A.Y * C.X - B.Y * C.X - A.X * C.Y - A.Y * B.X;
        }
        public static bool PointOnLine(Vector2 l1, Vector2 l2, Vector2 p)
        {   //sprawdza, czy punkt p należy do odcinka |l1 l2|
            if (p == l1 || p == l2)
                return true;
            if (Functions.Det(l1, l2, p) == 0)
            {
                if (Math.Min(l1.X, l2.X) <= p.X && p.X <= Math.Max(l1.X, l2.X) &&
                    Math.Min(l1.Y, l2.Y) <= p.Y && p.Y <= Math.Max(l1.Y, l2.Y))
                    return true;
            }

            return false;
        }
        public static bool IntersectLines(Vector2 p1, Vector2 p2, Vector2 p3, Vector2 p4)
        {
            if( Functions.PointOnLine(p1, p2, p3) || Functions.PointOnLine(p1, p2, p4) ||
                Functions.PointOnLine(p3, p4, p1) || Functions.PointOnLine(p3, p4, p2) )
                return true;
            if (Functions.GetSign(Functions.Det(p1, p2, p3)) == Functions.GetSign(Functions.Det(p1, p2, p4)))
                return false;
            else
                if (Functions.GetSign(Functions.Det(p3, p4, p1)) == Functions.GetSign(Functions.Det(p3, p4, p2)))
                    return false;
                else
                    return true;
        }
        public static bool MouseLeftClick() {
            if( Consts.PreviousMouseState.LeftButton == ButtonState.Released &&
                Consts.MouseState.LeftButton == ButtonState.Pressed )
                return true;
            return false;
        }
        public static bool KeyboardClick(Keys button) {
            if( Consts.PreviousKeyboardState.IsKeyUp( button ) &&
                Consts.KeyboardState.IsKeyDown( button ) )
                return true;
            return false;
        }
        /// <summary>
        /// Poprawia liczbę, by znajdowała się w zakresie 1-360
        /// </summary>
        /// <param name="angle">Kąt do poprawki</param>
        /// <returns></returns>
        public static int CorrectAngle(int angle) {
            if( angle > 0 && angle <= 360 )
                return angle;
            int ret = angle % 360;
            if( ret < 1 )
                ret = 360 + ret;
            return ret;
        }
        public static float CorrectAngle(float angle) {
            if( angle > 0 && angle <= 360 )
                return angle;
            float ret = angle % 360;
            if( ret < 1 )
                ret = 360 + ret;
            return ret;
        }
        public static float NextFloat( Random random, float range ) {
            //losowa liczba z zakresu (-range do range)
            double val = random.NextDouble(); // range 0.0 to 1.0
            val -= 0.5; // expected range now -0.5 to +0.5
            val *= 2; // expected range now -1.0 to +1.0
            return range * ( float )val;
        }
        public static float Angle( Vector2 from, Vector2 to ) {
            float ret = MathHelper.ToDegrees( ( float )Math.Atan2( from.X - to.X, to.Y - from.Y ) ) + 180;
            return Functions.CorrectAngle(ret);
        }
        public static Vector2 RightAlign(SpriteFont font, string text, Vector2 pos) {
            Vector2 size = font.MeasureString( text );
            return new Vector2( pos.X - size.X, pos.Y );
        }
        public static Vector2 TexMid(Texture2D tex, Vector2 pos) {
            return new Vector2( pos.X - tex.Width / 2, pos.Y - tex.Height / 2 );
        }
        public static Vector2 GetMousePosition() {
            MouseState kot = Consts.MouseState;
            return new Vector2( kot.X, kot.Y );
        }
        public static Vector2 CirclePoint( Vector2 O, float range, int total, int index ) {
            if( 0 >= index || index > total || range <= 0 )
                return Vector2.Zero;    //błędne dane

            Vector2 result = new Vector2();
            result.X = ( float )( O.X + range * Math.Cos( ( 6.2832 * ( double )index ) / total ) );
            result.Y = ( float )( O.Y + range * Math.Sin( ( 6.2832 * ( double )index ) / total ) );
            return result;
        }
        /// <summary>
        /// Creates an ARGB hex string representation of the <see cref="Color"/> value.
        /// </summary>
        /// <param name="color">The <see cref="Color"/> value to parse.</param>
        /// <param name="includeHash">Determines whether to include the hash mark (#) character in the string.</param>
        /// <returns>A hex string representation of the specified <see cref="Color"/> value.</returns>
        /// </summary>
        /// źródło:http://thedeadpixelsociety.com/2012/01/hex-colors-in-xna/
        public static string ToHex(this Color color, bool includeHash)
        {
            string[] argb = {
                color.A.ToString("X2"),
                color.R.ToString("X2"),
                color.G.ToString("X2"),
                color.B.ToString("X2"),
            };
                return (includeHash ? "#" : string.Empty) + string.Join(string.Empty, argb);
        }
        /// Creates a <see cref="Color"/> value from an ARGB or RGB hex string.  The string may
        /// begin with or without the hash mark (#) character.
        /// <summary>
        /// <param name="hexString">The ARGB hex string to parse.</param>
        /// <returns>
        /// A <see cref="Color"/> value as defined by the ARGB or RGB hex string.
        /// </returns>
        /// <exception cref="InvalidOperationException">Thrown if the string is not a valid ARGB or RGB hex value.</exception>
        /// </summary>
        /// źródło:http://thedeadpixelsociety.com/2012/01/hex-colors-in-xna/
        public static Color ToColor(this string hexString)
        {
            if (hexString.StartsWith("#"))
                hexString = hexString.Substring(1);
            uint hex = uint.Parse(hexString, System.Globalization.NumberStyles.HexNumber, CultureInfo.InvariantCulture);
            Color color = Color.White;
            if (hexString.Length == 8)
            {
                color.A = (byte)(hex >> 24);
                color.R = (byte)(hex >> 16);
                color.G = (byte)(hex >> 8);
                color.B = (byte)(hex);
            }
            else if (hexString.Length == 6)
            {
                color.R = (byte)(hex >> 16);
                color.G = (byte)(hex >> 8);
                color.B = (byte)(hex);
            }
            else
            {
                throw new InvalidOperationException("Invald hex representation of an ARGB or RGB color value.");
            }
            return color;
        }
    }
    public class Line {
        //źródło: http://stackoverflow.com/questions/270138/how-do-i-draw-lines-using-xna
        protected Texture2D pixel = Consts.Textures.Dot;
        public Vector2 p1, p2; //this will be the position in the center of the line
        protected int length;
        public int thickness; //length and thickness of the line, or width and height of rectangle
        protected Rectangle rect; //where the line will be drawn
        protected float rotation; // rotation of the line, with axis at the center of the line
        public Color color;

        //p1 and p2 are the two end points of the line
        public Line( Vector2 p1, Vector2 p2, int thickness, Color color ) {
            this.p1 = p1;
            this.p2 = p2;
            this.thickness = thickness;
            this.color = color;
            this.Update();
        }

        public void Update() {
            length = ( int )Vector2.Distance( p1, p2 ); //gets distance between the points
            rotation = getRotation( p1.X, p1.Y, p2.X, p2.Y ); //gets angle between points(method on bottom)
            rect = new Rectangle( ( int )p1.X, ( int )p1.Y, length, thickness );

            //To change the line just change the positions of p1 and p2
        }
        public void Draw( SpriteBatch spriteBatch) {
            spriteBatch.Draw( pixel, rect, null, color, rotation, Vector2.Zero, SpriteEffects.None, 0f );
        }

        public bool OnScreen()
        {   //funkcja sprawdza, czy linia znajduje się na ekranie (Algorytm Cohena-Sutherlanda)
            int code1 = 0, code2 = 0;
            float maxX = (float)Consts.GraphicsDevice.Viewport.Width, maxY = (float)Consts.GraphicsDevice.Viewport.Height;

            if (this.p1.X < 0)      code1 |= Consts.Values.LEFT_EDGE;
            if (this.p1.X > maxX)   code1 |= Consts.Values.RIGHT_EDGE;
            if (this.p1.Y < 0)      code1 |= Consts.Values.TOP_EDGE;
            if (this.p1.Y > maxY)   code1 |= Consts.Values.BOTTOM_EDGE;
            if (this.p2.X < 0)      code2 |= Consts.Values.LEFT_EDGE;
            if (this.p2.X > maxX)   code2 |= Consts.Values.RIGHT_EDGE;
            if (this.p2.Y < 0)      code2 |= Consts.Values.TOP_EDGE;
            if (this.p2.Y > maxY)   code2 |= Consts.Values.BOTTOM_EDGE;
            
            //cały odcinek mieści się w obszarze okna
            if ((code1 | code2) == 0)
                return true;
            //cały odcinek jest poza obszarem rysowania
            else if ((code1 & code2) != 0)
                return false;
            //odcinek przecina się z krawędziami ekranu
            return true;
            //powinno się wyliczyć miejsca przecięcia i tylko tam rysować
            //jednak tutaj to raczej nie jest potrzebne; XNA zrobi to za mnie...
        }

        //this returns the angle between two points in radians
        private float getRotation( float x, float y, float x2, float y2 ) {
            float adj = x - x2;
            float opp = y - y2;
            float tan = opp / adj;
            float res = MathHelper.ToDegrees( ( float )Math.Atan2( opp, adj ) );
            res = ( res - 180 ) % 360;
            if( res < 0 ) { res += 360; }
            res = MathHelper.ToRadians( res );
            return res;
        }
    }
}
