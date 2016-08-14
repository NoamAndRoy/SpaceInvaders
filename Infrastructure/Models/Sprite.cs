using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Infrastructure.Models
{
    public class Sprite : DrawableGameComponent
    {
        private const string k_SpritesPath = "Sprites/";

        private readonly string r_TexturePath;
        private readonly Color r_Tint;
        
        private Vector2 m_Pos;

        protected readonly BaseGame r_BaseGame;

        protected Texture2D m_Texture;

        public Vector2 Pos
        {
            get { return m_Pos; }
            set
            {
                if (m_Pos != value)
                {
                    m_Pos = value;
                    OnPositionChanged();
                }
            }
        }

        public Vector2 Velocity { get; set; }

        public int Width
        {
            get
            {
                return m_Texture.Width;
            }
        }

        public int Height
        {
            get
            {
                return m_Texture.Height;
            }
        }

        public Rectangle Bounds
        {
            get
            {
                return new Rectangle((int)this.Pos.X, (int)this.Pos.Y, Width, Height);
            }
        }

        public event EventHandler<EventArgs> PositionChanged;

        public Sprite(BaseGame i_Game, string i_TexturePath, Color i_Tint)
            : base(i_Game)
        {
            r_BaseGame = i_Game;
            r_TexturePath = i_TexturePath;
            r_Tint = i_Tint;

            this.Game.Components.Add(this);
        }

        protected override void LoadContent()
        {
            m_Texture = Game.Content.Load<Texture2D>(k_SpritesPath + r_TexturePath);

            base.LoadContent();
        }

        public override void Update(GameTime I_GameTime)
        {
            Pos += Velocity * (float)I_GameTime.ElapsedGameTime.TotalSeconds;
            base.Update(I_GameTime);
        }

        public override void Draw(GameTime i_GameTime)
        {
            r_BaseGame.SpriteBatch.Draw(m_Texture, Pos, r_Tint);

            base.Draw(i_GameTime);
        }

        public virtual void DeleteSprite()
        {
            Visible = false;
            Enabled = false;
            this.Dispose();
            Game.Components.Remove(this);
        }

        protected virtual void OnPositionChanged()
        {
            if(PositionChanged != null)
            {
                PositionChanged.Invoke(this, EventArgs.Empty);
            }
        }
    }
}