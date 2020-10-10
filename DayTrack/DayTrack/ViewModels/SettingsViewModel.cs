using DayTrack.Services;
using DayTrack.Utils;
using System.Linq;
using System.Windows.Input;
using Xamarin.Forms;

namespace DayTrack.ViewModels
{
    /// <summary>
    /// Provides an interface for managing application settings.
    /// </summary>
    public class SettingsViewModel : ViewModelBase, ISettingsViewModel
    {
        private readonly SettingsService _settingsService;

        /// <summary>
        /// The current date format setting.
        /// </summary>
        public string DateFormat
        {
            get => _settingsService.UserSettings.DateFormat;
            set
            {
                if (_settingsService.UserSettings.DateFormat != value && AllDateFormats.Contains(value))
                {
                    _settingsService.UserSettings.DateFormat = value;
                    RaiseChange(nameof(DateFormat));
                    SaveUserSettingsCommand.Execute(null);
                }
            }
        }

        /// <summary>
        /// All available date format settings.
        /// </summary>
        public string[] AllDateFormats { get; } = new[]
        {
            DateFormats.ShortYearMonthDay,
            DateFormats.ShortDayMonthYear,
            DateFormats.ShortMonthDayYear,
            DateFormats.LongYearMonthDay,
            DateFormats.LongDayMonthYear,
            DateFormats.LongMonthDayYear
        };

        public ICommand SaveUserSettingsCommand { get; }

        public SettingsViewModel(SettingsService settingsService)
        {
            _settingsService = settingsService;

            SaveUserSettingsCommand = new Command(() => SaveUserSettings());
        }

        internal void SaveUserSettings()
        {
            bool successful = _settingsService.TrySaveUserSettings();

            if (!successful)
            {
                MessagingCenter.Send(this, nameof(SaveUserSettingsCommand));
            }
        }
    }
}
