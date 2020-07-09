using DayTrack.Models;
using DayTrack.Services;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace DayTrack.ViewModels
{
    public class TrackerLogViewModel : ViewModelBase
    {
        private DateTime _dateToLog = DateTime.Now.Date;
        private readonly Tracker _tracker;
        private readonly TrackerLogService _logService;

        public const string DatabaseErrorMessage = "DatabaseError";

        public DateTime DateToLog
        {
            get => _dateToLog;
            set => SetAndRaiseIfChanged(ref _dateToLog, value);
        }

        public ObservableCollection<LoggedDay> AllDays { get; } = new ObservableCollection<LoggedDay>();
        public Command LogDayCommand { get; }

        public TrackerLogViewModel(Tracker tracker, TrackerLogService logService)
        {
            _tracker = tracker;
            _logService = logService;

            LogDayCommand = new Command(async () => await LogDayAsync());
            _ = PopulateAllDaysAsync();
        }

        private async Task LogDayAsync()
        {
            bool successful = await _logService.TryLogDayAsync(_tracker.Id, DateToLog);

            if (!successful)
            {
                MessagingCenter.Send(this, DatabaseErrorMessage);
                return;
            }

            await PopulateAllDaysAsync();
        }

        private async Task PopulateAllDaysAsync()
        {
            var allDays = await _logService.TryGetAllLoggedDaysAsync(_tracker.Id);

            if (allDays == null)
            {
                MessagingCenter.Send(this, DatabaseErrorMessage);
                return;
            }

            AllDays.Clear();
            foreach (var day in allDays)
            {
                AllDays.Add(day);
            }
        }
    }
}
