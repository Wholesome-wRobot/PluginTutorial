using robotManager.Helpful;
using System;
using System.ComponentModel;
using System.IO;
using wManager.Wow.Helpers;
using wManager.Wow.ObjectManager;

namespace PluginTutorial
{
    public class TutorialPluginSettings : Settings
    {
        private static readonly string _productName = "PluginTutorial";
        private static string GetSettingsPath => AdviserFilePathAndName(_productName, ObjectManager.Me.Name + "." + Usefuls.RealmName);
        public static TutorialPluginSettings CurrentSettings { get; set; }

        [DefaultValue(false)]
        [Category("Settings")]
        [DisplayName("Enable radar")]
        [Description("Shows the enemies around you")]
        public bool EnableRadar { get; set; }

        public TutorialPluginSettings()
        {
            EnableRadar = false;
        }

        public bool Save()
        {
            try
            {
                return Save(GetSettingsPath);
            }
            catch (Exception ex)
            {
                Logging.WriteError($"{_productName} > Save(): " + ex);
                return false;
            }
        }

        public static bool Load()
        {
            try
            {
                if (File.Exists(GetSettingsPath))
                {
                    CurrentSettings = Load<TutorialPluginSettings>(GetSettingsPath);
                    return true;
                }
                CurrentSettings = new TutorialPluginSettings();
            }
            catch (Exception ex)
            {
                Logging.WriteError($"{_productName} > Load(): " + ex);
            }
            return false;
        }
    }
}
