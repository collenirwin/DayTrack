using DayTrack.Models;
using DayTrack.Services;
using DayTrack.Utils;
using Newtonsoft.Json;
using Serilog;
using System.IO;
using Xunit;

namespace DayTrack.Tests
{
    public class SettingsServiceTests
    {
        private readonly ILogger _logger;
        private const string _userSettingsPath = "user_settings.json";

        public SettingsServiceTests()
        {
            _logger = new LoggerConfiguration()
                .WriteTo.Debug()
                .CreateLogger();

            if (File.Exists(_userSettingsPath))
            {
                File.Delete(_userSettingsPath);
            }
        }

        [Fact]
        public void UserSettings_NoSettingsFile_IsDefault()
        {
            // arrange
            var service = new SettingsService(_userSettingsPath, _logger);
            var defaultSettings = new UserSettings();

            // act
            string expected = JsonConvert.SerializeObject(defaultSettings);
            string actual = JsonConvert.SerializeObject(service.UserSettings);

            // assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void UserSettings_WithSettingsFile_UsesValuesFromFile()
        {
            // arrange
            var service = new SettingsService(_userSettingsPath, _logger);
            string settingsJson = JsonConvert.SerializeObject(new UserSettings
            {
                DateFormat = DateFormats.LongDayMonthYear
            });

            // act
            File.WriteAllText(_userSettingsPath, settingsJson);
            string actual = JsonConvert.SerializeObject(service.UserSettings);

            // assert
            Assert.Equal(settingsJson, actual);
        }

        [Fact]
        public void TrySaveUserSettings_NoSettingsFile_CreatesFile()
        {
            // arrange
            var service = new SettingsService(_userSettingsPath, _logger);
            string expected = JsonConvert.SerializeObject(service.UserSettings);

            // act
            bool successful = service.TrySaveUserSettings();
            string actual = File.ReadAllText(_userSettingsPath);

            // assert
            Assert.True(successful);
            Assert.Equal(expected, actual);
        }
    }
}
