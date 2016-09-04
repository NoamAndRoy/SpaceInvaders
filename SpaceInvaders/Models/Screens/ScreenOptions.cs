using System.Collections.Generic;
using Infrastructure.Models;
using Infrastructure.Models.Controls;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceInvaders.Models.Screens
{
    public class ScreenOptions : InvadersMenuScreen
    {
        public ScreenOptions(Game i_Game) 
            : base(i_Game, eMenuExitOption.Back)
        {
            r_Menu.Width = 400;
        }

        protected override void addControls()
        {
            InvadersLabel title = new InvadersLabel(Game, "Title", new Text(Game, "Button"));
            title.Text.Content = "Screen Options";
            title.Text.TintColor = Color.White;
            r_Menu.Add(title);

            InvadersPicker<bool> mouseVisability = new InvadersPicker<bool>(Game, "MouseVisability", "Mouse Visability", new Text(Game, "Button"));
            mouseVisability.Options.Add(new KeyValuePair<string, bool>("Visible", true));
            mouseVisability.Options.Add(new KeyValuePair<string, bool>("Invisible", false));
            mouseVisability.SetSelectionOption(AppSettings.Instance.IsMouseVisible);
            mouseVisability.SelectedOptionChanged += mouseVisability_SelectedOptionChanged;
            r_Menu.Add(mouseVisability);

            InvadersPicker<bool> allowResizing = new InvadersPicker<bool>(Game, "AllowResizing", "Allow Resizing", new Text(Game, "Button"));
            allowResizing.Options.Add(new KeyValuePair<string, bool>("On", true));
            allowResizing.Options.Add(new KeyValuePair<string, bool>("Off", false));
            allowResizing.SetSelectionOption(AppSettings.Instance.AllowResizing);
            allowResizing.SelectedOptionChanged += allowResizing_SelectedOptionChanged;
            r_Menu.Add(allowResizing);

            InvadersPicker<bool> fullScreen = new InvadersPicker<bool>(Game, "FullScreen", "Full Screen", new Text(Game, "Button"));
            fullScreen.Options.Add(new KeyValuePair<string, bool>("On", true));
            fullScreen.Options.Add(new KeyValuePair<string, bool>("Off", false));
            fullScreen.SetSelectionOption(AppSettings.Instance.FullScreen);
            fullScreen.SelectedOptionChanged += fullScreen_SelectedOptionChanged;
            r_Menu.Add(fullScreen);

            base.addControls();
        }

        private void fullScreen_SelectedOptionChanged(object sender, PickerEventArgs<bool> e)
        {
            GraphicsDeviceManager graphicsService = (GraphicsDeviceManager)Game.Services.GetService(typeof(IGraphicsDeviceService));
            graphicsService.ToggleFullScreen();
            AppSettings.Instance.FullScreen = graphicsService.IsFullScreen;
        }

        private void allowResizing_SelectedOptionChanged(object sender, PickerEventArgs<bool> e)
        {
            AppSettings.Instance.AllowResizing = e.SelectedValue;
            Game.Window.AllowUserResizing = e.SelectedValue;
        }

        private void mouseVisability_SelectedOptionChanged(object sender, PickerEventArgs<bool> e)
        {
            AppSettings.Instance.IsMouseVisible = e.SelectedValue;
        }
    }
}