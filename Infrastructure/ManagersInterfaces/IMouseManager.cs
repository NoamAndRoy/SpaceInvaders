using Microsoft.Xna.Framework;
using Infrastructure.Models; 

namespace Infrastructure.ManagersInterfaces
{
    public interface IMouseManager
    {
        void Update(GameTime i_GameTime);

        int X { get; }

        int Y { get; }

        int DeltaX { get; }

        int DeltaY { get; }

        bool IsKeyPressed(eMouseButton i_MouseButton);

        bool IsKeyReleased(eMouseButton i_MouseButton);

        bool IsKeyHeld(eMouseButton i_MouseButton);
    }
}
