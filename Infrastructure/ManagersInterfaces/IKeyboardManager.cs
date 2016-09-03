using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Infrastructure.ManagersInterfaces
{
    public interface IKeyboardManager
    {
        KeyboardState CurrentState { get; }

        KeyboardState PrevState { get; }

        bool IsKeyPressed(Keys i_Key);

        bool IsKeyReleased(Keys i_Key);

        bool IsKeyHeld(Keys i_Key);
    }
}
