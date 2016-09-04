using Infrastructure.Managers;
using Infrastructure.ManagersInterfaces;
using Infrastructure.Models;
using Infrastructure.Models.Controls;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace SpaceInvaders.Models.Screens
{
    class SoundOptions : InvadersMenuScreen
    {
        private ISoundManager m_SoundManager;

        public SoundOptions(Game i_Game) 
            : base(i_Game, eMenuExitOption.Back)
        {
            r_Menu.Width = 600;

            m_SoundManager = (ISoundManager)Game.Services.GetService(typeof(ISoundManager));
        }

        public object ISoundManager { get; private set; }

        protected override void addControls()
        {
            Label title = new Label(Game, "Title", new Text(Game, "Button"));
            title.Text.Content = "Sound Options";
            title.Text.TintColor = Color.White;
            r_Menu.Add(title);

            Picker <int> backgroundMusicVolume = new Picker<int>(Game, "BackgroundMusicVolume", "Background Music Volume", new Text(Game, "Button"));
            Picker<int> soundEffectsVolume = new Picker<int>(Game, "SoundEffectsVolume", "Sound Effects Volume", new Text(Game, "Button"));

            for (int i = 100; i >= 0; i-=10)
            {
                backgroundMusicVolume.Options.Add(new KeyValuePair<string, int>(i.ToString(), i));
                soundEffectsVolume.Options.Add(new KeyValuePair<string, int>(i.ToString(), i));
            }

            backgroundMusicVolume.SetSelectionOption((int)(m_SoundManager.BackgroundMusicVolume * 100));
            backgroundMusicVolume.SelectedOptionChanged += BackgroundMusicVolume_SelectedOptionChanged;
            r_Menu.Add(backgroundMusicVolume);

            soundEffectsVolume.SetSelectionOption((int)(m_SoundManager.SoundsEffectsVolum * 100));
            soundEffectsVolume.SelectedOptionChanged += SoundEffectsVolume_SelectedOptionChanged;
            r_Menu.Add(soundEffectsVolume);

            Picker<bool> toggleSound = new Picker<bool>(Game, "ToggleSound", "Toggle Sound", new Text(Game, "Button"));
            toggleSound.Options.Add(new KeyValuePair<string, bool>("On", true));
            toggleSound.Options.Add(new KeyValuePair<string, bool>("Off", false));
            toggleSound.SetSelectionOption(AppSettings.Instance.AllowResizing);
            toggleSound.SelectedOptionChanged += ToggleSound_SelectedOptionChanged;
            r_Menu.Add(toggleSound);

            base.addControls();
        }

        private void SoundEffectsVolume_SelectedOptionChanged(object sender, PickerEventArgs<int> e)
        {
            m_SoundManager.SoundsEffectsVolum = e.SelectedValue / 100f;
        }

        private void BackgroundMusicVolume_SelectedOptionChanged(object sender, PickerEventArgs<int> e)
        {
            m_SoundManager.SoundsEffectsVolum = e.SelectedValue / 100f;
        }

        private void ToggleSound_SelectedOptionChanged(object sender, PickerEventArgs<bool> e)
        {
            m_SoundManager.ToggleSound = e.SelectedValue;
        }
    }
}
