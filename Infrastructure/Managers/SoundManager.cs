using Infrastructure.ManagersInterfaces;
using Infrastructure.Models;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using System;

namespace Infrastructure.Managers
{
    public class SoundManager : GameService, ISoundManager
    {
        public const string k_SoundsPath = "Sounds/";

        public event EventHandler<EventArgs> BackgroundMusicVolumeChanged;
        public event EventHandler<EventArgs> SoundsEffectsVolumChanged;
        public event EventHandler<EventArgs> ToggleSoundChanged;

        float m_BackgroundMusicVolume;
        float m_SoundsEffectsVolume;
        bool m_ToggleSound;

        public SoundManager(Game i_Game) : base(i_Game)
        {
            m_BackgroundMusicVolume = 1;
            m_SoundsEffectsVolume = 1;
            m_ToggleSound = true;
        }

        protected override void registerService()
        {
            this.Game.Services.AddService(typeof(ISoundManager), this);
        }

        public void PlaySound(string i_AssetName, bool i_IsLooped = false, bool i_IsBackgroundMusic = false)
        {
            SoundEffectInstance soundEffectInstance = Game.Content.Load<SoundEffect>(k_SoundsPath + i_AssetName).CreateInstance();
            soundEffectInstance.Volume = 1;// i_IsBackgroundMusic ? BackgroundMusicVolume : SoundsEffectsVolum;
            soundEffectInstance.IsLooped = true;// i_IsLooped;
            soundEffectInstance.Play();
        }

        public float BackgroundMusicVolume
        {
            get
            {
                return m_ToggleSound ? m_BackgroundMusicVolume : 0;
            }

            set
            {
                if(m_BackgroundMusicVolume != value &&
                    m_BackgroundMusicVolume <= 1 &&
                    m_BackgroundMusicVolume >= 0)
                {
                    m_BackgroundMusicVolume = value;

                    OnBackgroundMusicVolumeChanged();
                }
            }
        }

        public float SoundsEffectsVolum
        {
            get
            {
                return m_ToggleSound ? m_SoundsEffectsVolume : 0;
            }

            set
            {
                if (m_SoundsEffectsVolume != value &&
                    m_SoundsEffectsVolume <= 1 &&
                    m_SoundsEffectsVolume >= 0)
                {
                    m_SoundsEffectsVolume = value;

                    OnSoundsEffectsVolumChanged();
                }
            }
        }

        public bool ToggleSound
        {
            get
            {
                return m_ToggleSound;
            }

            set
            {
                m_ToggleSound = value;

                OnToggleSoundChanged();
            }
        }

        private void OnBackgroundMusicVolumeChanged()
        {
            if (BackgroundMusicVolumeChanged != null)
            {
                BackgroundMusicVolumeChanged.Invoke(this, EventArgs.Empty);
            }
        }

        private void OnSoundsEffectsVolumChanged()
        {
            if (SoundsEffectsVolumChanged != null)
            {
                SoundsEffectsVolumChanged.Invoke(this, EventArgs.Empty);
            }
        }

        private void OnToggleSoundChanged()
        {
            if (ToggleSoundChanged != null)
            {
                ToggleSoundChanged.Invoke(this, EventArgs.Empty);
            }
        }
    }
}
