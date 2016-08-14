using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Infrastructure.ManagersInterfaces
{
    public interface IKeyboardManager
    {
        void Update(GameTime i_GameTime);

        bool IsKeyPressed(Keys i_Key);

        bool IsKeyReleased(Keys i_Key);

        bool IsKeyHeld(Keys i_Key);
    }
}
