using System;
using Infrastructure.Models.Screens;
using Infrastructure.Models;
using Microsoft.Xna.Framework;
using Infrastructure.ManagersInterfaces;
using Infrastructure.Models.Controls;
using Infrastructure.Models.Animators;

namespace SpaceInvaders.Models.Screens
{
    public class InvadersMenuScreen : MenuScreen
    {
        private const string k_MenuMoveSound = "MenuMove";

        public InvadersMenuScreen(Game i_Game, eMenuExitOption i_ExitOption) : base(i_Game, i_ExitOption)
        {
            Background = new Background(this);
            r_Menu.Width = 300;
            r_Menu.ActiveControlChanged += menu_ActiveControlChanged;

            r_Menu.ComponentAdded += menu_ComponentAdded;
        }

        private void menu_ActiveControlChanged(object sender, System.EventArgs e)
        {
            ((ISoundManager)Game.Services.GetService(typeof(ISoundManager))).PlaySound(k_MenuMoveSound);
        }

        protected override void addControls()
        {
            base.addControls();

            r_Menu.Position = CenterOfViewPort - new Vector2(r_Menu.Width / 2, CenterOfViewPort.Y - 100);
        }

        private void menu_ComponentAdded(object sender, GameComponentEventArgs<Control> e)
        {
            Control control = e.GameComponent;

            if (control.IsActiveable)
            {
                control.EnabledChanged += Control_EnabledChanged;
            }
        }

        private void Control_EnabledChanged(object sender, EventArgs e)
        {
            Control control = sender as Control;

            if (control.Animations["PulseAnimator"] == null)
            {
                Animator2D pulseAnimator = new PulseAnimator(TimeSpan.Zero, TimeSpan.FromSeconds(7), control.Scales, control.Scales * 0.6f);
                control.Animations.Add(pulseAnimator);
                //control.Animations.Resume();
            }

            if(!control.Enabled)
            {
                control.Animations.Reset();
            }
        }
    }
}
