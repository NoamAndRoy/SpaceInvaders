using System;
using Microsoft.Xna.Framework;

namespace Infrastructure.ManagersInterfaces
{
    public delegate void CollidedEventHandler(ICollideable i_Source, ICollideable i_Collided);

    public interface ICollideable
    {
        event EventHandler<EventArgs> PositionChanged;

        event EventHandler<EventArgs> VisibleChanged;

        event EventHandler<EventArgs> SizeChanged;

        event EventHandler<EventArgs> RotationChanged;

        event CollidedEventHandler CollidedAction;

        bool CheckRectangleCollision(ICollideable i_Collideable);

        bool CheckPixelBasedCollision(ICollideable i_Collideable);

        void Collided(ICollideable i_Collideable);

        Rectangle Bounds { get; }

        bool PixelBasedCollision { get; set; }

        Color[] PixelsMatrix { get; }

        bool Visible { get; }
    }

    public interface ICollisionManager
    {
        void AddComponent(ICollideable i_Collideable);
    }
}