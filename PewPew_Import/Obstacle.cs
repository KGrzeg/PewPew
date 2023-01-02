using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PewPew {
    public class Obstacle {
        protected Vector2 m_Pos;  //pozycja skrajnego lewego górnego punktu obiektu
        protected Vector2[] m_Points;
        protected Line[] m_Lines;
        protected Color m_Color;
        protected int m_Thickness;

        public Vector2 Position { get { return this.m_Pos; } }
        public Color Color { get { return this.m_Color; } }

        public Obstacle(Vector2 pos, Vector2[] points, Color color, int thickness) {
            this.m_Pos = pos;
            this.m_Points = points;
            this.m_Color = color;
            this.m_Thickness = thickness;
            this.m_Lines = new Line[points.Count()];
            this.ResetLines();
        }

        protected void ResetLines() {
            for( int i = 0; i < this.m_Points.Count(); ++i )
                if( i + 1 < this.m_Points.Count() )
                    this.m_Lines[i] = new Line( this.m_Points[i] + this.m_Pos, this.m_Points[i + 1] + this.m_Pos, this.m_Thickness, this.m_Color );
                else
                    this.m_Lines[i] = new Line( this.m_Points[i] + this.m_Pos, this.m_Points[0] + this.m_Pos, this.m_Thickness, this.m_Color );
        }
        protected void ApplyCamera(Vector2 cam) {
            for( int i = 0; i < this.m_Lines.Count(); ++i ) {
                this.m_Lines[i].p1 -= cam;
                this.m_Lines[i].p2 -= cam;
                this.m_Lines[i].Update();
            }
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 Camera) {
            this.ResetLines();
            this.ApplyCamera( Camera );
            for( int i = 0; i < this.m_Lines.Count(); ++i )
                m_Lines[i].Draw( spriteBatch );
        }
        public bool Intersect(Vector2 p1, Vector2 p2) {   //sprawdza, czy odcinek rozciągający się między punktami p1 i p2 przecina
            //się z dowolnym z odcinków tworzących przeszkodę
            for( int i = 0; i < this.m_Points.Count(); ++i ) {
                if( i + 1 < this.m_Points.Count() ) {
                    if( Functions.IntersectLines( this.Position + this.m_Points[i], this.Position + this.m_Points[i + 1], p1, p2 ) ) {
                        return true;
                    }
                } else {
                    if( Functions.IntersectLines( this.Position + this.m_Points[i], this.Position + this.m_Points[0], p1, p2 ) ) {
                        return true;
                    }
                }
            }
            return false;
        }
    }
}
