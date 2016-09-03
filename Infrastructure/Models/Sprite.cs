using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Infrastructure.Models
{
    public class Sprite : Component2D
    {
        public const string k_SpritesPath = "Sprites/";

        private Texture2D m_Texture;
        private Rectangle m_SourceRectangle;

        public Texture2D Texture
        {
            get { return m_Texture; }
            protected set { m_Texture = value; }
        }

        public Vector2 TextureCenter
        {
            get
            {
                return new Vector2(m_Texture.Width / 2f, m_Texture.Height / 2f);
            }
        }

        public Rectangle SourceRectangle
        {
            get { return m_SourceRectangle; }
            set { m_SourceRectangle = value; }
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

        protected override void initBounds()
        {
            base.initBounds();

            WidthBeforeScale = m_Texture.Width;
            HeightBeforeScale = m_Texture.Height;
            SourceRectangle = new Rectangle(0, 0, (int)WidthBeforeScale, (int)HeightBeforeScale);
        }

        protected override void LoadContent()
        {
            m_Texture = Game.Content.Load<Texture2D>(k_SpritesPath + AssetName);

            base.LoadContent();
        }

        public override void Draw(GameTime i_GameTime)
        {
            if (!m_UseSharedBatch)
            {
                SpriteBatch.Begin();
            }

            SpriteBatch.Draw(m_Texture, this.positionForDraw, this.SourceRectangle, this.TintColor, this.Rotation, this.RotationOrigin, this.Scales, this.SpriteEffects, this.LayerDepth);

            if (!m_UseSharedBatch)
            {
                SpriteBatch.End();
            }

            base.Draw(i_GameTime);
        }
    }
}