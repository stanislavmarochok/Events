using Plugin.Settings;
using Plugin.Settings.Abstractions;

namespace GoBuhat.Utils
{
    /// <summary>
    /// This is the Settings static class that can be used in your Core solution or in any
    /// of your client applications. All settings are laid out the same exact way with getters
    /// and setters. 
    /// </summary>
    public static class Settings
    {
        private static ISettings AppSettings
        {
            get
            {
                return CrossSettings.Current;
            }
        }

        #region Setting Constants

        private const string UserNameSettingsKey = "username_key";
        private const string UserPasswordSettingsKey = "userpassword_key";
        private const string UserIDSettingsKey = "userid_key";
        private static readonly string SettingsDefault = string.Empty;

        #endregion


        public static string UserName
        {
            get
            {
                return AppSettings.GetValueOrDefault(UserNameSettingsKey, SettingsDefault);
            }
            set
            {
                AppSettings.AddOrUpdateValue(UserNameSettingsKey, value);
            }
        }

        public static string UserPassword
        {
            get
            {
                return AppSettings.GetValueOrDefault(UserPasswordSettingsKey, SettingsDefault);
            }
            set
            {
                AppSettings.AddOrUpdateValue(UserPasswordSettingsKey, value);
            }
        }

        public static string UserID
        {
            get
            {
                return AppSettings.GetValueOrDefault(UserIDSettingsKey, SettingsDefault);
            }
            set
            {
                AppSettings.AddOrUpdateValue(UserIDSettingsKey, value);
            }
        }

    }
}