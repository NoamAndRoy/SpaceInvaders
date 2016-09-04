using Infrastructure.Models.Screens;
using Infrastructure.Models;
using Microsoft.Xna.Framework;
using Infrastructure.ManagersInterfaces;

namespace SpaceInvaders.Models.Screens
{
    public class InvadersMenuScreen : MenuScreen
    {
        private const string k_MenuMoveSound = "MenuMove";

        public InvadersMenuScreen(Game i_Game, eMenuExitOption i_ExitOption) : base(i_Game, i_ExitOption)
        {
            Background = new Background(i_Game);
            r_Menu.Width = 300;
            r_Menu.ActiveControlChanged += R_Menu_ActiveControlChanged;
        }

        private void R_Menu_ActiveControlChanged(object sender, System.EventArgs e)
        {
            ((ISoundManager)Game.Services.GetService(typeof(ISoundManager))).PlaySound(k_MenuMoveSound);
        }

        protected override void addControls()
        {
            base.addControls();

            r_Menu.Position = CenterOfViewPort - new Vector2(r_Menu.Width / 2, CenterOfViewPort.Y - 100);
        }
    }
}
