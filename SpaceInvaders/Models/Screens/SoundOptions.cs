using System.Collections.Generic;
using Infrastructure.ManagersInterfaces;
using Infrastructure.Models;
using Infrastructure.Models.Controls;
using Microsoft.Xna.Framework;

namespace SpaceInvaders.Models.Screens
{
    public class SoundOptions : InvadersMenuScreen
    {
        private ISoundManager m_SoundManager;
        private Label m_Title;
        private Picker<int> m_BackgroundMusicVolume;
        private Picker<int> m_SoundEffectsVolume;
        private Picker<bool> m_ToggleSound;

        public SoundOptions(Game i_Game) 
            : base(i_Game, eMenuExitOption.Back)
        {
            r_Menu.Width = 600;

            m_SoundManager = (ISoundManager)Game.Services.GetService(typeof(ISoundManager));
        }

        public object ISoundManager { get; private set; }

        protected override void addControls()
        {
            m_Title = new Label(this, "Title", new Text(this, "Button"));
            m_Title.Text.Content = "Sound Options";
            m_Title.Text.TintColor = Color.White;
            r_Menu.Add(m_Title);

            m_BackgroundMusicVolume = new Picker<int>(this, "BackgroundMusicVolume", "Background Music Volume", new Text(this, "Button"));
            m_SoundEffectsVolume = new Picker<int>(this, "SoundEffectsVolume", "Sound Effects Volume", new Text(this, "Button"));

            for (int i = 100; i >= 0; i -= 10)
            {
                m_BackgroundMusicVolume.Options.Add(new KeyValuePair<string, int>(i.ToString(), i));
                m_SoundEffectsVolume.Options.Add(new KeyValuePair<string, int>(i.ToString(), i));
            }

            m_BackgroundMusicVolume.SetSelectionOption((int)(m_SoundManager.RealBackgroundMusicVolume * 100));
            m_BackgroundMusicVolume.SelectedOptionChanged += BackgroundMusicVolume_SelectedOptionChanged;
            r_Menu.Add(m_BackgroundMusicVolume);

            m_SoundEffectsVolume.SetSelectionOption((int)(m_SoundManager.RealSoundEffectsVolume * 100));
            m_SoundEffectsVolume.SelectedOptionChanged += SoundEffectsVolume_SelectedOptionChanged;
            r_Menu.Add(m_SoundEffectsVolume);

            m_ToggleSound = new Picker<bool>(this, "ToggleSound", "Toggle Sound", new Text(this, "Button"));
            m_ToggleSound.Options.Add(new KeyValuePair<string, bool>("On", true));
            m_ToggleSound.Options.Add(new KeyValuePair<string, bool>("Off", false));
            m_ToggleSound.SetSelectionOption(m_SoundManager.ToggleSound);
            m_ToggleSound.SelectedOptionChanged += ToggleSound_SelectedOptionChanged;
            r_Menu.Add(m_ToggleSound);


           m_SoundManager.ToggleSoundChanged += SoundManager_ToggleSoundChanged;

            base.addControls();
        }

        private void SoundManager_ToggleSoundChanged(object sender, System.EventArgs e)
        {
            m_ToggleSound.SetSelectionOption(m_SoundManager.ToggleSound);
            AppSettings.Instance.ToggleSound = m_SoundManager.ToggleSound;
        }

        private void SoundEffectsVolume_SelectedOptionChanged(object sender, PickerEventArgs<int> e)
        {
            m_SoundManager.SoundEffectsVolume = e.SelectedValue / 100f;
            AppSettings.Instance.SoundEffectsVolume = m_SoundManager.SoundEffectsVolume;
        }

        private void BackgroundMusicVolume_SelectedOptionChanged(object sender, PickerEventArgs<int> e)
        {
            m_SoundManager.BackgroundMusicVolume = e.SelectedValue / 100f;
            AppSettings.Instance.BackgroundMusicVolume = m_SoundManager.BackgroundMusicVolume;
        }

        private void ToggleSound_SelectedOptionChanged(object sender, PickerEventArgs<bool> e)
        {
            m_SoundManager.ToggleSound = e.SelectedValue;
            AppSettings.Instance.ToggleSound = m_SoundManager.ToggleSound;
        }
    }
}
