using Microsoft.Xna.Framework;
using Infrastructure.ManagersInterfaces;
using Infrastructure.Managers;

namespace Infrastructure.Models
{
    public abstract class CollideableSprite : Sprite, ICollideable
    {
        public event CollidedEventHandler CollidedAction;

        public CollideableSprite(BaseGame i_Game, string i_TexturePath, Color i_Tint)
            : base(i_Game, i_TexturePath, i_Tint)
        {
        }

        public override void Initialize()
        {
            base.Initialize();
            (r_BaseGame.Services.GetService(typeof(CollisionManager)) as CollisionManager).AddComponent(this);
        }

        public virtual bool CheckCollisions(ICollideable i_Collideable)
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
            Visible = false;
            Enabled = false;
            this.Dispose();
            Game.Components.Remove(this);

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