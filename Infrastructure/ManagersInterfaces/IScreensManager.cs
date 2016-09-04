////*** Guy Ronen © 2008-2011 ***//
using Infrastructure.Models.Screens;

namespace Infrastructure.ManagersInterfaces
{
    public interface IScreensMananger
    {
        GameScreen ActiveScreen { get; }

        void SetCurrentScreen(GameScreen i_NewScreen);

        bool Remove(GameScreen i_Screen);

        void Add(GameScreen i_Screen);

        void Push(GameScreen i_GameScreen);
    }
}
