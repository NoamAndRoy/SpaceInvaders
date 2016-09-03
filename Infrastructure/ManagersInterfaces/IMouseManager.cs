using Infrastructure.Models;

namespace Infrastructure.ManagersInterfaces
{
    public interface IMouseManager
    {
        int X { get; }

        int PrevX { get; }

        int Y { get; }

        int PrevY { get; }

        int MouseWheel { get; }

        int DeltaX { get; }

        int DeltaY { get; }

        int MouseWheelDelta { get; }

        bool IsKeyPressed(eMouseButton i_MouseButton);

        bool IsKeyReleased(eMouseButton i_MouseButton);

        bool IsKeyHeld(eMouseButton i_MouseButton);
    }
}
