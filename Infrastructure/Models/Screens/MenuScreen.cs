using System;
using Microsoft.Xna.Framework;
using Infrastructure.Models.Controls;

namespace Infrastructure.Models.Screens
{
    public class MenuScreen : GameScreen
    {
        protected readonly Menu r_Menu;
        private readonly eMenuExitOption r_ExitOption;
        private bool m_IsMouseVisibleBeforeMenu;

        public MenuScreen(Game i_Game, eMenuExitOption i_ExitOption) : base(i_Game)
        {
            r_Menu = new Menu(i_Game);
            r_ExitOption = i_ExitOption;

            m_IsMouseVisibleBeforeMenu = Game.IsMouseVisible;
            this.Add(r_Menu);
        }

        protected override void OnActivated()
        {
            Game.IsMouseVisible = true;
            base.OnActivated();
        }

        protected override void OnDeactivated()
        {
            if (!(ScreensManager.ActiveScreen is MenuScreen))
            {
                Game.IsMouseVisible = m_IsMouseVisibleBeforeMenu;
            }

            r_Menu.CurrentControl = null;
            base.OnDeactivated();
        }

        public override void Initialize()
        {
            addControls();
            base.Initialize();
        }

        protected virtual void addControls()
        {
            string text = r_ExitOption == eMenuExitOption.Back ? "Done" : "Quit";
            Button quitOrBack = new Button(Game, text, new Text(Game, "Button"));
            quitOrBack.Text.Content = text;
            quitOrBack.Click += quitOrBack_Select;

            r_Menu.Add(quitOrBack);
        }

        private void quitOrBack_Select(object sender, EventArgs e)
        {
            if (r_ExitOption == eMenuExitOption.Back)
            {
                ExitScreen();
            }
            else
            {
                Game.Exit();
            }
        }
    }
}
