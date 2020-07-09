using DayTrack.Models;
using DayTrack.Services;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
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
        public ObservableCollection<LoggedDayGroup> AllDayGroups { get; } = new ObservableCollection<LoggedDayGroup>();
        public ICommand LogDayCommand { get; }
        public ICommand DeleteLoggedDayCommand { get; }
        public ICommand PullAllDaysCommand { get; }
        public ICommand PullAllDayGroupsCommand { get; }

        public TrackerLogViewModel(Tracker tracker, TrackerLogService logService)
        {
            _tracker = tracker;
            _logService = logService;

            LogDayCommand = new Command(async () => await LogDayAsync());
            DeleteLoggedDayCommand = new Command(async day => await DeleteLoggedDayAsync(day as LoggedDay));
            PullAllDaysCommand = new Command(async () => await PopulateAllDaysAsync());
            PullAllDayGroupsCommand = new Command(async () => await PopulateAllDayGroupsAsync());
        }

        private async Task LogDayAsync()
        {
            bool successful = await _logService.TryLogDayAsync(_tracker.Id, DateToLog);

            if (!successful)
            {
                MessagingCenter.Send(this, DatabaseErrorMessage);
                return;
            }

            await PopulateAllDayGroupsAsync();
        }

        private async Task DeleteLoggedDayAsync(LoggedDay loggedDay)
        {
            int index = AllDays.IndexOf(loggedDay);
            AllDays.RemoveAt(index);
            bool successful = await _logService.TryDeleteLoggedDayAsync(loggedDay.Id);

            if (!successful)
            {
                MessagingCenter.Send(this, nameof(DeleteLoggedDayCommand), loggedDay);
                AllDays.Insert(index, loggedDay);
            }
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

        private async Task PopulateAllDayGroupsAsync()
        {
            var allDayGroups = await _logService.TryGetAllLoggedDayGroupsAsync(_tracker.Id);

            if (allDayGroups == null)
            {
                MessagingCenter.Send(this, DatabaseErrorMessage);
                return;
            }

            AllDayGroups.Clear();
            foreach (var group in allDayGroups)
            {
                AllDayGroups.Add(group);
            }
        }
    }
}
