using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using System.IO;

namespace SpaceInvaders
{
    public sealed class AppSettings
    {
        private const string k_SettingsFile = "AppSettings.xml";

        private static AppSettings s_Instance;

        public static AppSettings Instance
        {
            get
            {
                if (s_Instance == null)
                {
                    s_Instance = loadSettingsOrDefault();
                }

                return s_Instance;
            }
        }

        public bool IsMouseVisible { get; set; }

        public bool AllowResizing { get; set; }

        public bool FullScreen { get; set; }

        public int AmountOfPlayers { get; set; }

        public float SoundEffectsVolume { get; set; }

        public float BackgroundMusicVolume { get; set; }

        public bool ToggleSound { get; set; }

        private AppSettings()
        {
            IsMouseVisible = false;
            AllowResizing = false;
            FullScreen = false;
            AmountOfPlayers = 2;
            SoundEffectsVolume = 1;
            BackgroundMusicVolume = 1;
            ToggleSound = true;
        }

        private static AppSettings loadSettingsOrDefault()
        {
            XmlSerializer serializer = new XmlSerializer(typeof(AppSettings));
            AppSettings appSettings;

            if (File.Exists(k_SettingsFile))
            {
                using (Stream stream = new FileStream(k_SettingsFile, FileMode.Open))
                {
                    appSettings = (AppSettings)serializer.Deserialize(stream);
                }
            }
            else
            {
                appSettings = new AppSettings();
            }

            return appSettings;
        }

        public void SaveSettings()
        {
            XmlSerializer serializer = new XmlSerializer(typeof(AppSettings));

            using (Stream stream = new FileStream(k_SettingsFile, FileMode.Create))
            {
                serializer.Serialize(stream, this);
            }
        }
    }
}
