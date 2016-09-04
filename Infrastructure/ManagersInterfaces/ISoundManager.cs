using System;

namespace Infrastructure.ManagersInterfaces
{
    public interface ISoundManager
    {
        float BackgroundMusicVolume { get; set; }

        float SoundsEffectsVolum { get; set; }

        bool ToggleSound { get; set; }

        void PlaySound(string i_AssetName, bool i_IsLooped = false, bool i_IsBackgroundMusic = false);

        event EventHandler<EventArgs> BackgroundMusicVolumeChanged;

        event EventHandler<EventArgs> SoundsEffectsVolumChanged;

        event EventHandler<EventArgs> ToggleSoundChanged;
    }
}