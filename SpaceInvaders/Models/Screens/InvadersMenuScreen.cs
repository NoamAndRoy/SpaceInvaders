using Infrastructure.Models.Screens;
using Infrastructure.Models;
using Microsoft.Xna.Framework;

namespace SpaceInvaders.Models.Screens
{
    public class InvadersMenuScreen : MenuScreen
    {
        public InvadersMenuScreen(Game i_Game, eMenuExitOption i_ExitOption) : base(i_Game, i_ExitOption)
        {
            Background = new Background(i_Game);
            r_Menu.Width = 300;
        }

        protected override void addControls()
        {
            base.addControls();
            r_Menu.Position = CenterOfViewPort - new Vector2(r_Menu.Width / 2, CenterOfViewPort.Y - 100);
        }
    }
}
