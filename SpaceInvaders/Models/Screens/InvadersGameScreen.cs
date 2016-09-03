using Infrastructure.Models.Screens;
using Microsoft.Xna.Framework;

namespace SpaceInvaders.Models.Screens
{
    public class InvadersGameScreen : GameScreen
    {
        public InvadersGameScreen(Game i_Game) : base(i_Game)
        {
            Background = new Background(i_Game);
            Game.IsMouseVisible = AppSettings.Instance.IsMouseVisible;
        }
    }
}
