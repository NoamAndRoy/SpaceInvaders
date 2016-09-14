using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Infrastructure.Models;
using Infrastructure.ManagersInterfaces;
using Infrastructure.Models.Screens;

namespace Infrastructure.Managers
{
    public class CollisionManager : GameService, ICollisionManager
    {
        private readonly Dictionary<GameScreen, List<ICollideable>> r_CollideablesScreens;

        public CollisionManager(Game i_Game)
            : base(i_Game)
        {
            r_CollideablesScreens = new Dictionary<GameScreen, List<ICollideable>>();
        }

        protected override void registerService()
        {
            this.Game.Services.AddService(typeof(ICollisionManager), this);
        }

        public void AddComponent(ICollideable i_Collideable)
        {
            if (i_Collideable != null)
            {
                List<ICollideable> collideables;

                if (r_CollideablesScreens.ContainsKey(i_Collideable.GameScreen))
                {
                    collideables = r_CollideablesScreens[i_Collideable.GameScreen];
                }
                else
                {
                    collideables = new List<ICollideable>();
                    r_CollideablesScreens[i_Collideable.GameScreen] = collideables;
                }

                if (!collideables.Contains(i_Collideable))
                {
                    collideables.Add(i_Collideable);
                    i_Collideable.PositionChanged += collideable_Changed;
                    i_Collideable.VisibleChanged += collideable_Changed;
                    i_Collideable.SizeChanged += collideable_Changed;
                    i_Collideable.RotationChanged += collideable_Changed;

                    i_Collideable.Disposed += collideable_Disposed;
                }
            }
        }

        private void collideable_Disposed(object i_Sender, EventArgs e)
        {
            ICollideable collideable = i_Sender as ICollideable;

            r_CollideablesScreens[collideable.GameScreen].Remove(collideable);
            collideable.PositionChanged -= collideable_Changed;
            collideable.VisibleChanged -= collideable_Changed;
            collideable.SizeChanged -= collideable_Changed;
            collideable.RotationChanged -= collideable_Changed;

            collideable.Disposed -= collideable_Disposed;
        }

        private void collideable_Changed(object i_Sender, EventArgs i_EventArgs)
        {
            List<ICollideable> collided = new List<ICollideable>();
            ICollideable source = i_Sender as ICollideable;

            if (source.Visible)
            {
                foreach (ICollideable collideable in r_CollideablesScreens[source.GameScreen])
                {
                    if (source != collideable && collideable.Visible)
                    {
                        if ((!source.PixelBasedCollision || !collideable.PixelBasedCollision) && source.CheckRectangleCollision(collideable))
                        {
                            collided.Add(collideable);
                        }
                        else if (source.PixelBasedCollision && collideable.PixelBasedCollision && source.CheckPixelBasedCollision(collideable))
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
