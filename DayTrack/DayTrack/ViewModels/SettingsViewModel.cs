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
        private string _dateFormat;
        private readonly SettingsService _settingsService;

        /// <summary>
        /// The current date format setting.
        /// </summary>
        public string DateFormat
        {
            get => _dateFormat;
            set
            {
                if (_dateFormat != value && AllDateFormats.Contains(value))
                {
                    SetAndRaiseIfChanged(ref _dateFormat, value);
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
            _dateFormat = settingsService.UserSettings.DateFormat;

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
