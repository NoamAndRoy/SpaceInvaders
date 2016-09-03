using System;
using Microsoft.Xna.Framework;
using Infrastructure.ManagersInterfaces;
using Infrastructure.Models.Screens;

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

        public bool PixelBasedCollision { get; set; }

        public CollideableSprite(Game i_Game, string i_TexturePath)
            : base(i_Game, i_TexturePath)
        {
            PixelBasedCollision = false;
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

        public virtual bool CheckRectangleCollision(ICollideable i_Collideable)
        {
            bool areIntersects = false;

            if(this.Bounds.Intersects(i_Collideable.Bounds))
            {
                areIntersects = true;
            }

            return areIntersects;
        }

        public virtual bool CheckPixelBasedCollision(ICollideable i_Collideable)
        {
            bool areIntersects = false;

            if (CheckRectangleCollision(i_Collideable) &&
                pixelsMatricesIntersects(m_PixelsMatrix, Bounds, i_Collideable.PixelsMatrix, i_Collideable.Bounds))
            {
                areIntersects = true;
            }

            return areIntersects;
        }

        public virtual void Collided(ICollideable i_Collideable)
        {
            OnCollide(i_Collideable);
        }

        protected virtual void OnCollide(ICollideable i_Collideable)
        {
            if (CollidedAction != null)
            {
                CollidedAction.Invoke(this, i_Collideable);
            }
        }

        protected bool pixelsMatricesIntersects(Color[] i_MatrixOne, Rectangle i_MatrixOneBounds, Color[] i_MatrixTwo, Rectangle i_MatrixTwoBounds)
        {
            bool areIntersects = false;

            int matrixOneInnerX,
                matrixOneInnerY,
                matrixTwoInnerX,
                matrixTwoInnerY,
                minX = Math.Max(i_MatrixOneBounds.X, i_MatrixTwoBounds.X),
                minY = Math.Max(i_MatrixOneBounds.Y, i_MatrixTwoBounds.Y),
                maxX = Math.Min(i_MatrixOneBounds.X + i_MatrixOneBounds.Width - 1, i_MatrixTwoBounds.X + i_MatrixTwoBounds.Width - 1),
                maxY = Math.Min(i_MatrixOneBounds.Y + i_MatrixOneBounds.Height - 1, i_MatrixTwoBounds.Y + i_MatrixTwoBounds.Height - 1);

            for (int i = minX; !areIntersects && i <= maxX; i++)
            {
                for (int j = minY; !areIntersects && j <= maxY; j++)
                {
                    matrixOneInnerX = i - i_MatrixOneBounds.X;
                    matrixOneInnerY = j - i_MatrixOneBounds.Y;
                    matrixTwoInnerX = i - i_MatrixTwoBounds.X;
                    matrixTwoInnerY = j - i_MatrixTwoBounds.Y;

                    if (i_MatrixOne[(matrixOneInnerY * i_MatrixOneBounds.Width) + matrixOneInnerX].A != 0 && 
                        i_MatrixTwo[(matrixTwoInnerY * i_MatrixTwoBounds.Width) + matrixTwoInnerX].A != 0)
                    {
                        areIntersects = true;
                    }
                }
            }

            return areIntersects; 
        }
    }
}