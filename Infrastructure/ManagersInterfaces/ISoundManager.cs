using System;
using Microsoft.Xna.Framework.Audio;

namespace Infrastructure.ManagersInterfaces
{
    public interface ISoundManager
    {
        float BackgroundMusicVolume { get; set; }

        float RealBackgroundMusicVolume { get; }

        float SoundEffectsVolume { get; set; }

        float RealSoundEffectsVolume { get; }

        bool ToggleSound { get; set; }

        void PlaySound(string i_AssetName, float i_Pitch = 0, float i_Pan = 0);

        SoundEffectInstance PlayContinuousSound(string i_AssetName, bool i_IsBackgroundMusic = false);

        event EventHandler<EventArgs> BackgroundMusicVolumeChanged;

        event EventHandler<EventArgs> SoundsEffectsVolumChanged;

        event EventHandler<EventArgs> ToggleSoundChanged;
    }
}