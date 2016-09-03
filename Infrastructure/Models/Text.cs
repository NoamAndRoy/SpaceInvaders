using Infrastructure.Models.Screens;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Infrastructure.Models
{
    public class Text : Component2D
    {
        public const string k_FontsPath = "Fonts/";

        private SpriteFont m_Font;
        private string m_Content = string.Empty;

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

                if (m_Font != null)
                {
                    initBounds();
                }
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

        public override void Draw(GameTime i_GameTime)
        {
            if (!m_UseSharedBatch)
            {
                m_SpriteBatch.Begin();
            }

            SpriteBatch.DrawString(m_Font, m_Content, this.positionForDraw, this.TintColor, this.Rotation, this.RotationOrigin, this.Scales, this.SpriteEffects, this.LayerDepth);

            if (!m_UseSharedBatch || AlphaBlending)
            {
                m_SpriteBatch.End();
            }

            base.Draw(i_GameTime);
        }
    }
}