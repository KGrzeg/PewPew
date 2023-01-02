using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace PewPew.Interface {
    public enum EPointer {
        arrow = 0,
        crosshair,
        author  //special
    }
    public class Pointer {
        private Color m_Color;
        private Texture2D m_arrow;
        private Texture2D m_crosshair;

        private Texture2D m_current_tex;
        private PointerHead m_author_pointer;
        private bool m_special;

        public bool Special { get { return this.m_special; } }

        public Pointer() {
            this.m_arrow = Consts.Values.Pointers.tex_Normal;
            this.m_crosshair = Consts.Values.Pointers.tex_InGame;

            this.m_current_tex = this.m_arrow;
            this.m_special = false;
            this.m_Color = Color.White;
        }
        public void Update(GameTime gameTime) { this.m_author_pointer.Update( gameTime ); }
        public void Draw(SpriteBatch spriteBatch, GameTime gameTime) {
            if( this.m_special ) {
                this.m_author_pointer.Draw( spriteBatch, gameTime );
            } else {
                spriteBatch.Draw( this.m_current_tex, Functions.GetMousePosition() - new Vector2( this.m_current_tex.Width / 2, this.m_current_tex.Height / 2 ), this.m_Color );
            }
        }
        public void SetPointer(EPointer point) {
            if( this.m_special && point != EPointer.author ) {
                this.m_special = false;
                this.m_author_pointer = null;
            }
            switch( point ) {
                case EPointer.arrow: {
                        this.m_current_tex = this.m_arrow;
                        break;
                    }
                case EPointer.crosshair: {
                        this.m_current_tex = this.m_crosshair;
                        break;
                    }
                case EPointer.author: {
                        this.m_author_pointer = new PointerHead();
                        this.m_special = true;
                        break;
                    }
            }
        }
        public void SetColor(Color color) { this.m_Color = color; }

    }
    public class PointerHead {
        protected PointerTail[] m_Tail;
        protected Texture2D m_Tex;
        protected Vector2 m_Pos, m_LastPos;
        protected Color m_Color;
        protected float m_Angle, m_Size;

        protected int m_NumOfTailes;
        protected double m_SpawnTailCd;

        public Vector2 Position { get { return this.m_Pos; } }

        public PointerHead() {
            this.m_Pos = Functions.GetMousePosition();
            this.m_Tex = Consts.Values.Pointers.AuthorMenu.tex_Head;
            this.m_Color = Consts.Values.Pointers.AuthorMenu.col_Head;
            this.m_Size = Math.Max( this.m_Tex.Width / 2, this.m_Tex.Height / 2 );
            this.m_NumOfTailes = 0;
            this.m_SpawnTailCd = Consts.Values.Pointers.AuthorMenu.SpawnTime;
            this.m_Tail = new PointerTail[Consts.Values.Pointers.AuthorMenu.Lenght];
            this.m_Angle = 1;
        }
        public void Update(GameTime gameTime) {
            this.Move( gameTime );
            if( this.m_NumOfTailes < this.m_Tail.Count() )
                this.AddTail( gameTime );
            for( int i=0; i < m_NumOfTailes; ++i )
                this.m_Tail[i].Update( gameTime );
        }
        public void Draw(SpriteBatch spriteBatch, GameTime gameTime) {
            spriteBatch.Draw( this.m_Tex, new Rectangle( ( int ) ( this.m_Pos.X ), ( int ) ( this.m_Pos.Y ), this.m_Tex.Width, this.m_Tex.Height ), null, this.m_Color, MathHelper.ToRadians( this.m_Angle ), new Vector2( this.m_Tex.Width / 2, this.m_Tex.Height / 2 ), SpriteEffects.None, 1f );
            for( int i=0; i < m_NumOfTailes; ++i )
                this.m_Tail[i].Draw( spriteBatch, gameTime );
        }
        protected void AddTail(GameTime gameTime) {
            double time = gameTime.ElapsedGameTime.TotalMilliseconds;
            this.m_SpawnTailCd -= time;
            if( this.m_SpawnTailCd < 0 ) {
                this.m_SpawnTailCd = Consts.Values.Enemies.Znake.AddTileTime;
                this.m_Tail[this.m_NumOfTailes++] = new PointerTail( this, ( this.m_NumOfTailes == 1 ? null : this.m_Tail[m_NumOfTailes - 2] ) );
            }
        }
        protected void Move(GameTime gameTime) {
            this.m_Pos = Functions.GetMousePosition();
            if( Vector2.Distance( this.m_Pos, this.m_LastPos ) > 0.35 )
                this.m_Angle = Functions.Angle( this.m_LastPos, this.m_Pos );
            this.m_LastPos = this.m_Pos;
        }
    }
    public class PointerTail {
        protected PointerHead m_Head;
        protected Texture2D m_Tex;
        protected Vector2 m_Pos;
        protected Color m_Color;
        protected float m_Angle, m_Distance;

        protected PointerTail m_NextPointerTail;

        public float Angle { get { return this.m_Angle; } }
        public Vector2 Position { get { return this.m_Pos; } }

        public PointerTail(PointerHead head, PointerTail tail) {
            this.m_Head = head;
            this.m_NextPointerTail = tail; //null, jesli to pierwsza część
            this.m_Pos = head.Position;
            this.m_Tex = Consts.Values.Pointers.AuthorMenu.tex_Tail;
            this.m_Color = Consts.Values.Pointers.AuthorMenu.col_Tail;
            this.m_Distance = Consts.Values.Pointers.AuthorMenu.Distance;
        }
        public void Update(GameTime gameTime) {
            this.Move( gameTime );
        }
        public void Draw(SpriteBatch spriteBatch, GameTime gameTime) { spriteBatch.Draw( this.m_Tex, new Rectangle( ( int ) ( this.m_Pos.X ), ( int ) ( this.m_Pos.Y ), this.m_Tex.Width, this.m_Tex.Height ), null, this.m_Color, MathHelper.ToRadians( this.m_Angle ), new Vector2( this.m_Tex.Width / 2, this.m_Tex.Height / 2 ), SpriteEffects.None, 1f ); }
        protected void Move(GameTime gameTime) {
            this.UpdateRotation();
            Vector2 target = new Vector2();
            if( this.m_NextPointerTail != null )
                target = this.m_NextPointerTail.Position;
            else
                target = this.m_Head.Position;
            Vector2 move = new Vector2();
            Vector2.Lerp( ref this.m_Pos, ref target, this.m_Distance, out move );

            this.m_Pos = move;
        }
        protected void UpdateRotation() {
            if( this.m_NextPointerTail != null )
                this.m_Angle = Functions.Angle( this.Position, this.m_NextPointerTail.Position );
            else
                this.m_Angle = Functions.Angle( this.Position, this.m_Head.Position );
        }
    }
}
