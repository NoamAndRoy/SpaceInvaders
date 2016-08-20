using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Infrastructure.Models;
using Infrastructure.ManagersInterfaces;

namespace Infrastructure.Managers
{
    public class CollisionManager : GameService, ICollisionManager
    {
        private readonly List<ICollideable> r_Collideables;

        public CollisionManager(Game i_Game)
            : base(i_Game)
        {
            r_Collideables = new List<ICollideable>();

            Game.Components.ComponentRemoved += componentRemoved;
        }

        protected override void registerService()
        {
            this.Game.Services.AddService(typeof(ICollisionManager), this);
        }

        public void AddComponent(ICollideable i_Collideable)
        {
            if (i_Collideable != null && !r_Collideables.Contains(i_Collideable))
            {
                r_Collideables.Add(i_Collideable);
                i_Collideable.PositionChanged += collideable_Changed;
                i_Collideable.VisibleChanged += collideable_Changed;
                i_Collideable.SizeChanged += collideable_Changed;
                i_Collideable.RotationChanged += collideable_Changed;
            }
        }

        private void componentRemoved(object i_Sender, EventArgs i_EventArgs)
        {
            ICollideable collideable = i_Sender as ICollideable;

            if (collideable != null && this.r_Collideables.Contains(collideable))
            {
                r_Collideables.Remove(collideable);
                collideable.PositionChanged -= collideable_Changed;
                collideable.VisibleChanged -= collideable_Changed;
                collideable.SizeChanged -= collideable_Changed;
                collideable.RotationChanged -= collideable_Changed;
            }
        }

        private void collideable_Changed(object i_Sender, EventArgs i_EventArgs)
        {
            List<ICollideable> collided = new List<ICollideable>();
            ICollideable source = i_Sender as ICollideable;

            if (source.Visible)
            {
                foreach (ICollideable collideable in r_Collideables)
                {
                    if (source != collideable && collideable.Visible)
                    {
                        if (!source.PixelBasedCollision && source.CheckRectangleCollision(collideable))
                        {
                            collided.Add(collideable);
                        }
                        else if (source.PixelBasedCollision && source.CheckPixelBasedCollision(collideable))
                        {
                            collided.Add(collideable);
                        }
                    }
                }

                foreach (ICollideable collideable in collided)
                {
                    source.Collided(collideable);
                    collideable.Collided(source);
                }
            }
        }
    }
}
