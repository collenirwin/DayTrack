using DayTrack.Models;
using DayTrack.Services;
using DayTrack.Utils;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

[assembly: InternalsVisibleTo("DayTrack.Tests")]
namespace DayTrack.ViewModels
{
    /// <summary>
    /// Provides an interface for interaction with <see cref="LoggedDay"/>s.
    /// </summary>
    public class TrackerLogViewModel : ViewModelBase
    {
        private DateTime _dateToLog = DateTime.Now.Date;
        private GroupSortOption _sortOption = GroupSortOption.DateDescending;
        private ObservableCollection<LoggedDay> _allDays = new ObservableCollection<LoggedDay>();
        private ObservableCollection<LoggedDayGroup> _allDayGroups = new ObservableCollection<LoggedDayGroup>();
        private LoggedDayStats _loggedDayStats;
        private readonly Tracker _tracker;
        private readonly ITrackerLogService _logService;

        /// <summary>
        /// Message to subscribe to for notification of database errors during command execution.
        /// </summary>
        public const string DatabaseErrorMessage = "DatabaseError";

        /// <summary>
        /// Date for a new <see cref="LoggedDay"/>.
        /// </summary>
        public DateTime DateToLog
        {
            get => _dateToLog;
            set => SetAndRaiseIfChanged(ref _dateToLog, value);
        }

        /// <summary>
        /// Selected sort option index (converts directly to a <see cref="GroupSortOption"/> value).
        /// </summary>
        public int SortOptionIndex
        {
            get => (int)_sortOption;
            set => SetAndRaiseIfChanged(ref _sortOption, (GroupSortOption)value);
        }

        /// <summary>
        /// All <see cref="LoggedDay"/>s from the database.
        /// </summary>
        public ObservableCollection<LoggedDay> AllDays
        {
            get => _allDays;
            set => SetAndRaiseIfChanged(ref _allDays, value);
        }

        /// <summary>
        /// All <see cref="LoggedDay"/>s represented as <see cref="LoggedDayGroup"/>s.
        /// </summary>
        public ObservableCollection<LoggedDayGroup> AllDayGroups
        {
            get => _allDayGroups;
            set => SetAndRaiseIfChanged(ref _allDayGroups, value);
        }

        /// <summary>
        /// Computed stats for <see cref="AllDays"/>.
        /// </summary>
        public LoggedDayStats LoggedDayStats
        {
            get => _loggedDayStats;
            set => SetAndRaiseIfChanged(ref _loggedDayStats, value);
        }

        /// <summary>
        /// Manages user settings.
        /// </summary>
        public ISettingsViewModel SettingsViewModel { get; }

        /// <summary>
        /// Create a new <see cref="LoggedDay"/> with the current <see cref="DateToLog"/>.
        /// </summary>
        public ICommand LogDayCommand { get; }

        /// <summary>
        /// Delete a logged day (requires the <see cref="LoggedDay"/> to delete).
        /// </summary>
        public ICommand DeleteLoggedDayCommand { get; }

        /// <summary>
        /// Get all <see cref="LoggedDay"/>s from the database, storing them in <see cref="AllDays"/>.
        /// </summary>
        public ICommand PullAllDaysCommand { get; }

        /// <summary>
        /// Get all <see cref="LoggedDay"/>s from the database as <see cref="LoggedDayGroup"/>s,
        /// storing them in <see cref="AllDayGroups"/>.
        /// </summary>
        public ICommand PullAllDayGroupsCommand { get; }

        /// <summary>
        /// Pulls <see cref="AllDays"/>, then computes <see cref="LoggedDayStats"/>.
        /// </summary>
        public ICommand PullStatsCommand { get; }

        public TrackerLogViewModel(Tracker tracker, ITrackerLogService logService, ISettingsViewModel settingsViewModel)
        {
            _tracker = tracker;
            _logService = logService;
            SettingsViewModel = settingsViewModel;

            LogDayCommand = new Command(async () => await LogDayAsync().ExpressLoading(this));
            DeleteLoggedDayCommand = new Command(async day =>
                await DeleteLoggedDayAsync(day as LoggedDay).ExpressLoading(this));
            PullAllDaysCommand = new Command(async () => await PopulateAllDaysAsync().ExpressLoading(this));
            PullAllDayGroupsCommand = new Command(async () => await PopulateAllDayGroupsAsync().ExpressLoading(this));
            PullStatsCommand = new Command(async () => await PopulateStatsAsync().ExpressLoading(this));
        }

        internal async Task<bool> LogDayAsync()
        {
            bool successful = await _logService.TryLogDayAsync(DateToLog, _tracker.Id);

            if (!successful)
            {
                MessagingCenter.Send(this, DatabaseErrorMessage);
                return false;
            }

            return await PopulateAllDayGroupsAsync();
        }

        internal async Task<bool> DeleteLoggedDayAsync(LoggedDay loggedDay)
        {
            int index = AllDays.IndexOf(loggedDay);

            if (index == -1 || loggedDay is null)
            {
                return false;
            }

            AllDays.RemoveAt(index);
            bool successful = await _logService.TryDeleteLoggedDayAsync(loggedDay.Id);

            if (!successful)
            {
                MessagingCenter.Send(this, nameof(DeleteLoggedDayCommand), loggedDay);
                AllDays.Insert(index, loggedDay);
            }

            return successful;
        }

        internal async Task<bool> PopulateAllDaysAsync()
        {
            var allDays = await _logService.TryGetAllLoggedDaysAsync(_tracker.Id);

            if (allDays == null)
            {
                MessagingCenter.Send(this, DatabaseErrorMessage);
                return false;
            }

            AllDays = new ObservableCollection<LoggedDay>(allDays);
            return true;
        }

        internal async Task<bool> PopulateAllDayGroupsAsync()
        {
            await PopulateAllDaysAsync();
            var allDayGroups = _logService.TryGetAllLoggedDayGroups(AllDays, _tracker.Id, _sortOption)?
                .Select(group =>
                {
                    group.DateString = group.Date.ToString(SettingsViewModel.DateFormat);
                    return group;
                });

            if (allDayGroups == null)
            {
                MessagingCenter.Send(this, DatabaseErrorMessage);
                return false;
            }

            AllDayGroups = new ObservableCollection<LoggedDayGroup>(allDayGroups);
            return true;
        }

        internal async Task<bool> PopulateStatsAsync()
        {
            bool successful = await PopulateAllDaysAsync();

            if (!successful || AllDayGroups is null)
            {
                MessagingCenter.Send(this, DatabaseErrorMessage);
                return false;
            }

            if (!AllDayGroups.Any())
            {
                LoggedDayStats = new LoggedDayStats();
                return true;
            }

            var first = AllDays.Last().Date.Date;
            var last = AllDays.First().Date.Date;
            double totalDays = (int)(last - first).TotalDays + 1;
            
            int medianIndex = AllDayGroups.Count / 2;
            if (AllDayGroups.Count % 2 == 0)
            {
                medianIndex--;
            }

            LoggedDayStats = new LoggedDayStats
            {
                Average = AllDays.Count / totalDays,
                Min = AllDayGroups.Min(group => group.Count),
                Max = AllDayGroups.Max(group => group.Count),
                Median = AllDayGroups[medianIndex].Count,
                First = first.ToString(SettingsViewModel.DateFormat),
                Last = first.ToString(SettingsViewModel.DateFormat)
            };

            return true;
        }
    }
}
