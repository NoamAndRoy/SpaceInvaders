using System;
using System.Collections.Generic;
using Infrastructure.ManagersInterfaces;
using Infrastructure.Models;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;

namespace Infrastructure.Managers
{
    public class SoundManager : GameService, ISoundManager
    {
        public const string k_SoundsPath = "Sounds/";

        public event EventHandler<EventArgs> BackgroundMusicVolumeChanged;

        public event EventHandler<EventArgs> SoundsEffectsVolumChanged;

        public event EventHandler<EventArgs> ToggleSoundChanged;

        private float m_BackgroundMusicVolume;
        private float m_SoundsEffectsVolume;
        private bool m_ToggleSound;

        private readonly List<SoundEffectInstance> r_Sounds = new List<SoundEffectInstance>();
        private readonly List<SoundEffectInstance> r_BackgroundMusic = new List<SoundEffectInstance>();

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

        public void PlaySound(string i_AssetName, float i_Pitch = 0, float i_Pan = 0)
        {
            if (ToggleSound && SoundEffectsVolume > 0)
            {
                SoundEffect soundEffect = loadSoundEffect(i_AssetName);
                soundEffect.Play(SoundEffectsVolume, i_Pitch, i_Pan);
            }
        }

        public SoundEffectInstance PlayContinuousSound(string i_AssetName, bool i_IsBackgroundMusic = false)
        {
            SoundEffectInstance soundEffectInstance = loadSoundEffect(i_AssetName).CreateInstance();
            soundEffectInstance.Volume = i_IsBackgroundMusic ? BackgroundMusicVolume : SoundEffectsVolume;
            soundEffectInstance.IsLooped = i_IsBackgroundMusic;
            soundEffectInstance.Play();

            if(i_IsBackgroundMusic)
            {
                r_BackgroundMusic.Add(soundEffectInstance);
            }
            else
            {
                r_Sounds.Add(soundEffectInstance);
            }

            return soundEffectInstance;
        }

        private SoundEffect loadSoundEffect(string i_AssetName)
        {
            return Game.Content.Load<SoundEffect>(k_SoundsPath + i_AssetName);
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

        public float RealBackgroundMusicVolume
        {
            get
            {
                return m_BackgroundMusicVolume;
            }
        }

        public float SoundEffectsVolume
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

        public float RealSoundEffectsVolume
        {
            get
            {
                return m_SoundsEffectsVolume;
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

        protected virtual void OnBackgroundMusicVolumeChanged()
        {
            foreach(SoundEffectInstance backgroundMusic in r_BackgroundMusic)
            {
                backgroundMusic.Volume = BackgroundMusicVolume;
            }

            if (BackgroundMusicVolumeChanged != null)
            {
                BackgroundMusicVolumeChanged.Invoke(this, EventArgs.Empty);
            }
        }

        protected virtual void OnSoundsEffectsVolumChanged()
        {
            foreach (SoundEffectInstance soundEffects in r_Sounds)
            {
                soundEffects.Volume = SoundEffectsVolume;
            }

            if (SoundsEffectsVolumChanged != null)
            {
                SoundsEffectsVolumChanged.Invoke(this, EventArgs.Empty);
            }
        }

        protected virtual void OnToggleSoundChanged()
        {
            foreach (SoundEffectInstance soundInstance in r_Sounds)
            {
                soundInstance.Volume = SoundEffectsVolume;
            }

            foreach (SoundEffectInstance soundInstance in r_BackgroundMusic)
            {
                soundInstance.Volume = BackgroundMusicVolume;
            }

            if (ToggleSoundChanged != null)
            {
                ToggleSoundChanged.Invoke(this, EventArgs.Empty);
            }
        }

        protected override void Dispose(bool disposing)
        {
            BackgroundMusicVolumeChanged = null;
            SoundsEffectsVolumChanged = null;
            ToggleSoundChanged = null;

            base.Dispose(disposing);
        }
    }
}
