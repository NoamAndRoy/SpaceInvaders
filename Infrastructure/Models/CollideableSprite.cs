using Microsoft.Xna.Framework;
using Infrastructure.ManagersInterfaces;
using Infrastructure.Managers;

namespace Infrastructure.Models
{
    public abstract class CollideableSprite : Sprite, ICollideable
    {
        public event CollidedEventHandler CollidedAction;

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
    }
}