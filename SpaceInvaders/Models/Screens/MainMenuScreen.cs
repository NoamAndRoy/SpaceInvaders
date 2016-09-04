﻿using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Infrastructure.Models;
using Infrastructure.Models.Controls;

namespace SpaceInvaders.Models.Screens
{
    public class MainMenuScreen : InvadersMenuScreen
    {        
        public MainMenuScreen(Game i_Game) 
            : base(i_Game, eMenuExitOption.Exit)
        {
        }

        protected override void addControls()
        {
            InvadersLabel title = new InvadersLabel(Game, "Title", new Text(Game, "Button"));
            title.Text.Content = "Main Menu";
            title.Text.TintColor = Color.White;
            r_Menu.Add(title);

            InvadersButton soundOptions = new InvadersButton(Game, "SoundOptions", new Text(Game, "Button"));
            soundOptions.Text.Content = "Sound Options";
            soundOptions.Click += soundOptions_Select;
            r_Menu.Add(soundOptions);

            InvadersButton screenOptions = new InvadersButton(Game, "ScreenOptions", new Text(Game, "Button"));
            screenOptions.Text.Content = "Screen Options";
            screenOptions.Click += screenOptions_Select;
            r_Menu.Add(screenOptions);

            InvadersPicker<int> amountOfPlayers = new InvadersPicker<int>(Game, "AmountOfPlayers", "Players", new Text(Game, "Button"));
            amountOfPlayers.Options.Add(new KeyValuePair<string, int>("One", 1));
            amountOfPlayers.Options.Add(new KeyValuePair<string, int>("Two", 2));
            amountOfPlayers.SelectedOptionChanged += amountOfPlayers_SelectedOptionChanged;
            amountOfPlayers.SetSelectionOption(AppSettings.Instance.AmountOfPlayers);
            r_Menu.Add(amountOfPlayers);

            InvadersButton play = new InvadersButton(Game, "Play", new Text(Game, "Button"));
            play.Text.Content = "Play";
            play.Click += play_Select;
            r_Menu.Add(play);

            base.addControls();
        }

        private void amountOfPlayers_SelectedOptionChanged(object sender, PickerEventArgs<int> e)
        {
            AppSettings.Instance.AmountOfPlayers = e.SelectedValue;
        }

        private void quit_Select(object sender, EventArgs e)
        {
            Game.Exit();
        }

        private void play_Select(object sender, EventArgs e)
        {
            ScreensManager.SetCurrentScreen(new LevelTransitionScreen(Game, 1));
            ExitScreen();
        }

        private void screenOptions_Select(object sender, EventArgs e)
        {
            ScreensManager.SetCurrentScreen(new ScreenOptions(Game));
        }

        private void soundOptions_Select(object sender, EventArgs e)
        {
            ScreensManager.SetCurrentScreen(new SoundOptions(Game));
        }
    }
}
