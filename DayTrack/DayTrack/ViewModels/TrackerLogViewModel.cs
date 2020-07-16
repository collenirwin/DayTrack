using DayTrack.Models;
using DayTrack.Services;
using DayTrack.Utils;
using System;
using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

[assembly: InternalsVisibleTo("DayTrack.Tests")]
namespace DayTrack.ViewModels
{
    public class TrackerLogViewModel : ViewModelBase
    {
        private DateTime _dateToLog = DateTime.Now.Date;
        private GroupSortOption _sortOption = GroupSortOption.DateDescending;
        private readonly Tracker _tracker;
        private readonly ITrackerLogService _logService;

        public const string DatabaseErrorMessage = "DatabaseError";

        public DateTime DateToLog
        {
            get => _dateToLog;
            set => SetAndRaiseIfChanged(ref _dateToLog, value);
        }

        public int SortOptionIndex
        {
            get => (int)_sortOption;
            set => SetAndRaiseIfChanged(ref _sortOption, (GroupSortOption)value);
        }

        public ObservableCollection<LoggedDay> AllDays { get; } = new ObservableCollection<LoggedDay>();
        public ObservableCollection<LoggedDayGroup> AllDayGroups { get; } = new ObservableCollection<LoggedDayGroup>();
        public ICommand LogDayCommand { get; }
        public ICommand DeleteLoggedDayCommand { get; }
        public ICommand PullAllDaysCommand { get; }
        public ICommand PullAllDayGroupsCommand { get; }

        public TrackerLogViewModel(Tracker tracker, ITrackerLogService logService)
        {
            _tracker = tracker;
            _logService = logService;

            LogDayCommand = new Command(async () => await LogDayAsync().ExpressLoading(this));
            DeleteLoggedDayCommand = new Command(async day =>
                await DeleteLoggedDayAsync(day as LoggedDay).ExpressLoading(this));
            PullAllDaysCommand = new Command(async () => await PopulateAllDaysAsync().ExpressLoading(this));
            PullAllDayGroupsCommand = new Command(async () => await PopulateAllDayGroupsAsync().ExpressLoading(this));
        }

        internal async Task LogDayAsync()
        {
            bool successful = await _logService.TryLogDayAsync(DateToLog, _tracker.Id);

            if (!successful)
            {
                MessagingCenter.Send(this, DatabaseErrorMessage);
                return;
            }

            await PopulateAllDayGroupsAsync();
        }

        internal async Task DeleteLoggedDayAsync(LoggedDay loggedDay)
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

        internal async Task PopulateAllDaysAsync()
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

        internal async Task PopulateAllDayGroupsAsync()
        {
            var allDayGroups = await _logService.TryGetAllLoggedDayGroupsAsync(_tracker.Id, _sortOption);

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
