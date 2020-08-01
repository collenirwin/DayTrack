using DayTrack.Models;
using DayTrack.Utils;
using Newtonsoft.Json;
using Serilog;
using System.IO;

namespace DayTrack.Services
{
    /// <summary>
    /// Manages application settings.
    /// </summary>
    public class SettingsService
    {
        private UserSettings _userSettings;
        private readonly ILogger _logger;
        private readonly string _userSettingsPath;

        /// <summary>
        /// Loaded or defaulted user application settings.
        /// </summary>
        public UserSettings UserSettings => _userSettings ??= (GetSavedUserSettings() ?? new UserSettings());

        public SettingsService(string userSettingsPath, ILogger logger)
        {
            _userSettingsPath = userSettingsPath;
            _logger = logger;
        }

        private void SaveUserSettings()
        {
            string settingsJson = JsonConvert.SerializeObject(UserSettings);
            File.WriteAllText(_userSettingsPath, settingsJson);
        }

        /// <summary>
        /// Serializes and saves <see cref="UserSettings"/>.
        /// </summary>
        /// <returns>true if successful</returns>
        public bool TrySaveUserSettings() =>
            Try.Run(() => SaveUserSettings(),
                ex => _logger.Error("Failed to save user settings.", ex));

        private UserSettings GetSavedUserSettings()
        {
            if (File.Exists(_userSettingsPath))
            {
                string settingsJson = Try.Run(() => File.ReadAllText(_userSettingsPath),
                    ex => _logger.Error("Failed to open user settings file.", ex));

                if (!string.IsNullOrEmpty(settingsJson))
                {
                    return Try.Run(() => JsonConvert.DeserializeObject<UserSettings>(settingsJson),
                        ex => _logger.Error("Failed to deserialize user settings file.", ex));
                }
            }

            return null;
        }
    }
}
