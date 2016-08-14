using Microsoft.Xna.Framework;
using Infrastructure.ManagersInterfaces;
using Infrastructure.Managers;

namespace Infrastructure.Models
{
    public abstract class CollideableSprite : Sprite, ICollideable
    {
        public event CollidedEventHandler CollidedAction;

        private Color[] m_PixelsMatrix;

        public Color[] PixelsMatrix
        {
            get
            {
                return m_PixelsMatrix;
            }
        }

        public Rectangle Bounds
        {
            get
            {
                return new Rectangle(
                    (int)TopLeftPosition.X,
                    (int)TopLeftPosition.Y,
                    (int)this.Width,
                    (int)this.Height);
            }
        }

        public Rectangle BoundsBeforeScale
        {
            get
            {
                return new Rectangle(
                    (int)TopLeftPosition.X,
                    (int)TopLeftPosition.Y,
                    (int)this.WidthBeforeScale,
                    (int)this.HeightBeforeScale);
            }
        }

        public CollideableSprite(Game i_Game, string i_TexturePath)
            : base(i_Game, i_TexturePath)
        {
        }

        protected override void LoadContent()
        {
            base.LoadContent();

            m_PixelsMatrix = new Color[Texture.Height * Texture.Width];

            Texture.GetData<Color>(m_PixelsMatrix);
        }

        public override void Initialize()
        {
            base.Initialize();
        }

        public virtual bool CheckCollision(ICollideable i_Collideable)
        {
            bool intersects = false;

            if(this.Bounds.Intersects(i_Collideable.Bounds))
            {
                intersects = true;
            }

            return intersects;
        }

        public virtual bool CheckPixelBasedCollision(ICollideable i_Collideable)
        {
            bool intersects = false;

            if (CheckCollision(i_Collideable))
            {
                intersects = true;
            }
            else if(m_PixelsMatrix == null)
            { 
                intersects = true;
            }

            return intersects;
        }

        public virtual void Collided(ICollideable i_Collideable)
        {
            DeleteSprite();

            OnCollide(i_Collideable);
        }

        protected virtual void OnCollide(ICollideable i_Collideable)
        {
            if (CollidedAction != null)
            {
                CollidedAction.Invoke(this, i_Collideable);
            }
        }

        private bool PixelsMatricesIntersects(Color[] i_MatrixOne, Rectangle i_MatrixOneBounds, Color[] i_MatrixTwo, Rectangle i_MatrixTwoBounds)
        {
            bool intersects = false;

            for (int i = 0; i < i_MatrixOneBounds.Height; i++)
            {
                for (int j = 0; j < i_MatrixOneBounds.Width; j++)
                {
                    for (int k = 0; k < i_MatrixTwoBounds.Height; k++)
                    {
                        for (int l = 0; l < i_MatrixTwoBounds.Width; l++)
                        {
                            if (i_MatrixOne[i * i_MatrixOneBounds.Width + j].A != 0 && i_MatrixTwo[k * i_MatrixTwoBounds.Width + l].A != 0 && i_MatrixOneBounds.X + i == 0) { }
                        }
                    }
                }
            }

            return intersects; 
        }
    }
}