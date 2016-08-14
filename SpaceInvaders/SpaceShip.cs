using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SpaceInvaders
{
    public abstract class SpaceShip : Sprite, ICollideable
    {
        public event HitEventHandler Hit;

        public SpaceShip(BaseGame i_Game, string i_TexturePath, Color i_Tint)
            : base(i_Game, i_TexturePath, i_Tint)
        {
        }

        public Rectangle Bounds
        {
            get
            {
                return new Rectangle((int)Pos.X, (int)Pos.Y, Width, Height);
            }
        }

        public virtual bool Intersects(ICollideable i_Collideable)
        {
            bool isIntersects = false;

            if(Bounds.Intersects(i_Collideable.Bounds))
            {
                OnHit(i_Collideable);
            }

            return isIntersects;
        }

        protected virtual void OnHit(ICollideable i_Collideable)
        {
            if(Hit != null)
            {
                Hit.Invoke(this, i_Collideable);
            }
        }
    }
}