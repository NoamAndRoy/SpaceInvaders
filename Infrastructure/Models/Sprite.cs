using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Infrastructure.Models
{
    public abstract class Sprite : LoadableDrawableComponent
    {
        private const string k_SpritesPath = "Sprites/";

        private Texture2D m_Texture;
        private Vector2 m_Position = Vector2.Zero;
        private Vector2 m_PositionOrigin = Vector2.Zero;
        private Vector2 m_RotationOrigin = Vector2.Zero;
        private Vector2 m_Scales = Vector2.One;
        private Vector2 m_Velocity = Vector2.Zero;
        private float m_WidthBeforeScale;
        private float m_HeightBeforeScale;
        private float m_Rotation = 0;
        private float m_LayerDepth = 0;
        private float m_AngularVelocity = 0;
        private Rectangle m_SourceRectangle;
        private Color m_TintColor = Color.White;
        private SpriteEffects m_SpriteEffects = SpriteEffects.None;
        private SpriteBatch m_SpriteBatch;
        private bool m_UseSharedBatch = true;

        public Texture2D Texture
        {
            get { return m_Texture; }
        }

        public Vector2 TextureCenter
        {
            get
            {
                return new Vector2((float)(m_Texture.Width / 2), (float)(m_Texture.Height / 2));
            }
        }

        public Vector2 Position
        {
            get { return m_Position; }
            set
            {
                if (m_Position != value)
                {
                    m_Position = value;
                    OnPositionChanged();
                }
            }
        }

        public Vector2 PositionOrigin
        {
            get { return m_PositionOrigin; }
            set { m_PositionOrigin = value; }
        }

        public Vector2 RotationOrigin
        {
            get { return m_RotationOrigin; }
            set { m_RotationOrigin = value; }
        }

        private Vector2 PositionForDraw
        {
            get { return this.Position - this.PositionOrigin + this.RotationOrigin; }
        }

        public Vector2 TopLeftPosition
        {
            get { return this.Position - this.PositionOrigin; }
            set { this.Position = value + this.PositionOrigin; }
        }

        public Vector2 Scales
        {
            get { return m_Scales; }
            set
            {
                if (m_Scales != value)
                {
                    m_Scales = value;
                    OnSizeChanged();
                }
            }
        }

        public Vector2 Velocity
        {
            get { return m_Velocity; }
            set { m_Velocity = value; }
        }

        public float Width
        {
            get { return m_WidthBeforeScale * m_Scales.X; }
            set { m_WidthBeforeScale = value / m_Scales.X; }
        }

        public float Height
        {
            get { return m_HeightBeforeScale * m_Scales.Y; }
            set { m_HeightBeforeScale = value / m_Scales.Y; }
        }

        public float WidthBeforeScale
        {
            get { return m_WidthBeforeScale; }
            set { m_WidthBeforeScale = value; }
        }

        public float HeightBeforeScale
        {
            get { return m_HeightBeforeScale; }
            set { m_HeightBeforeScale = value; }
        }

        public float Rotation
        {
            get { return m_Rotation; }
            set
            {
                if(m_Rotation != value)
                {
                    m_Rotation = value;
                    OnRotationChanged();
                }
            }
        }

        public float LayerDepth
        {
            get { return m_LayerDepth; }
            set { m_LayerDepth = value; }
        }

        public float AngularVelocity
        {
            get { return m_AngularVelocity; }
            set { m_AngularVelocity = value; }
        }

        public Rectangle SourceRectangle
        {
            get { return m_SourceRectangle; }
            set { m_SourceRectangle = value; }
        }

        public Color TintColor
        {
            get { return m_TintColor; }
            set { m_TintColor = value; }
        }

        public SpriteEffects SpriteEffects
        {
            get { return m_SpriteEffects; }
            set { m_SpriteEffects = value; }
        }

        public SpriteBatch SpriteBatch
        {
            get { return m_SpriteBatch; }
            set
            {
                m_SpriteBatch = value;
                m_UseSharedBatch = true;
            }
        }

        public float Opacity
        {
            get { return (float)m_TintColor.A / (float)byte.MaxValue; }
            set { m_TintColor.A = (byte)(value * (float)byte.MaxValue); }
        }

        public Sprite(Game i_Game, string i_TexturePath)
            : base(i_Game, i_TexturePath)
        {
        }

        public override void Initialize()
        {
            base.Initialize();

            initBounds();
        }

        protected virtual void initBounds()
        {
            m_WidthBeforeScale = m_Texture.Width;
            m_HeightBeforeScale = m_Texture.Height;
            Position = Vector2.Zero;

            SourceRectangle = new Rectangle(0, 0, (int) m_WidthBeforeScale, (int)m_HeightBeforeScale);
        }

        protected override void LoadContent()
        {
            m_Texture = Game.Content.Load<Texture2D>(k_SpritesPath + AssetName);

            if (m_SpriteBatch == null)
            {
                m_SpriteBatch = Game.Services.GetService(typeof(SpriteBatch)) as SpriteBatch;

                if (m_SpriteBatch == null)
                {
                    m_SpriteBatch = new SpriteBatch(Game.GraphicsDevice);
                    m_UseSharedBatch = false;
                }
            }

            base.LoadContent();
        }

        public override void Update(GameTime i_GameTime)
        {
            float totalSeconds = (float)i_GameTime.ElapsedGameTime.TotalSeconds;

            this.Position += this.Velocity * totalSeconds;
            this.Rotation += this.AngularVelocity * totalSeconds;

            base.Update(i_GameTime);
        }

        public override void Draw(GameTime i_GameTime)
        {
            if (!m_UseSharedBatch)
            {
                m_SpriteBatch.Begin();
            }

            m_SpriteBatch.Draw(m_Texture, this.PositionForDraw, this.SourceRectangle, this.TintColor, this.Rotation, this.RotationOrigin, this.Scales, SpriteEffects.None, this.LayerDepth);

            if (!m_UseSharedBatch)
            {
                m_SpriteBatch.End();
            }

            base.Draw(i_GameTime);
        }

        public virtual void DeleteSprite()
        {
            Visible = false;
            Enabled = false;
            this.Dispose();
            Game.Components.Remove(this);
        }

         public Sprite ShallowClone()
        {
            return (Sprite)this.MemberwiseClone();
        }
    }
}