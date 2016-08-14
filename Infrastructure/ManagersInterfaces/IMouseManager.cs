using Microsoft.Xna.Framework;

namespace Infrastructure.ManagersInterfaces
{
    public enum eMouseButton
    {
        LeftButton,
        RightButton,
        MiddleButton
    }
    
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
