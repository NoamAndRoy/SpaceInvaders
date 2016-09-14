using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Infrastructure.Models.Animators;
using Infrastructure.Models.Screens;

namespace Infrastructure.Models
{
    public abstract class Component2D : LoadableDrawableComponent
    {
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
        private Color m_TintColor = Color.White;
        private SpriteEffects m_SpriteEffects = SpriteEffects.None;
        protected SpriteBatch m_SpriteBatch;
        protected bool m_UseSharedBatch = true;
        protected CompositeAnimator m_Animations;

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

        protected Vector2 positionForDraw
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
            get { return WidthBeforeScale * m_Scales.X; }
            set { WidthBeforeScale = value / m_Scales.X; }
        }

        public float Height
        {
            get { return HeightBeforeScale * m_Scales.Y; }
            set { HeightBeforeScale = value / m_Scales.Y; }
        }

        public float WidthBeforeScale
        {
            get { return m_WidthBeforeScale; }
            set
            {
                if (m_WidthBeforeScale != value)
                {
                    m_WidthBeforeScale = value;
                    OnSizeChanged();
                }
            }
        }

        public float HeightBeforeScale
        {
            get { return m_HeightBeforeScale; }
            set
            {
                if (m_HeightBeforeScale != value)
                {
                    m_HeightBeforeScale = value;
                    OnSizeChanged();
                }
            }
        }

        public float Rotation
        {
            get { return m_Rotation; }
            set
            {
                if (m_Rotation != value)
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

        public CompositeAnimator Animations
        {
            get { return m_Animations; }
            set { m_Animations = value; }
        }

        public float Opacity
        {
            get { return (float)m_TintColor.A / (float)byte.MaxValue; }
            set { m_TintColor.A = (byte)(value * (float)byte.MaxValue); }
        }

        public Component2D(GameScreen i_GameScreen, string i_TexturePath)
            : base(i_GameScreen, i_TexturePath)
        {
        }

        public override void Initialize()
        {
            base.Initialize();

            m_Animations = new CompositeAnimator(this);
            initBounds();
        }

        protected virtual void initBounds()
        {
        }

        protected override void LoadContent()
        {
            if (m_SpriteBatch == null)
            {
                m_SpriteBatch = new SpriteBatch(Game.GraphicsDevice);
                m_UseSharedBatch = false;
            }

            base.LoadContent();
        }

        public override void Update(GameTime i_GameTime)
        {
            float totalSeconds = (float)i_GameTime.ElapsedGameTime.TotalSeconds;
            this.Position += this.Velocity * totalSeconds;
            this.Rotation += this.AngularVelocity * totalSeconds;

            base.Update(i_GameTime);

            this.Animations.Update(i_GameTime);
        }

        protected override void Dispose(bool disposing)
        {
            Visible = false;
            Enabled = false;
            base.Dispose(disposing);
        }

        public Component2D ShallowClone()
        {
            return (Component2D)this.MemberwiseClone();
        }
    }
}