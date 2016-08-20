using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Infrastructure.Models
{
    public abstract class Text : Component2D
    {
        public const string k_FontsPath = "Fonts/";

        private SpriteFont m_Font;
        private string m_Content;

        public SpriteFont Font
        {
            get { return m_Font; }
            protected set { m_Font = value; }
        }

        public Vector2 TextureCenter
        {
            get
            {
                return new Vector2(WidthBeforeScale / 2, HeightBeforeScale / 2);
            }
        }

        public string Content
        {
            get
            {
                return m_Content;
            }

            set
            {
                m_Content = value;
            }
        }

        public Text(Game i_Game, string i_FontPath)
            : base(i_Game, i_FontPath)
        {
        }

        public override void Initialize()
        {
            base.Initialize();

            initBounds();
        }

        protected override void initBounds()
        {
            base.initBounds();

            WidthBeforeScale = m_Font.MeasureString(m_Content).X;
            HeightBeforeScale = m_Font.MeasureString(m_Content).Y;
        }

        protected override void LoadContent()
        {
            m_Font = Game.Content.Load<SpriteFont>(k_FontsPath + AssetName);

            base.LoadContent();
        }

        protected override void DrawComponent2D(GameTime i_GameTime)
        {
            SpriteBatch.DrawString(m_Font, m_Content, this.PositionForDraw, this.TintColor, this.Rotation, this.RotationOrigin, this.Scales, SpriteEffects.None, this.LayerDepth);

            base.DrawComponent2D(i_GameTime);
        }
    }
}