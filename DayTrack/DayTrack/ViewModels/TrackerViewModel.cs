using DayTrack.Models;
using DayTrack.Services;
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
    /// Provides an interface to create, read, update, and delete <see cref="Tracker"/>s.
    /// </summary>
    public class TrackerViewModel : ViewModelBase
    {
        private int _id = 0;
        private string _name = "";
        private string _errorMessage = "";
        private bool _hasError = false;
        private ObservableCollection<Tracker> _allTrackers = new ObservableCollection<Tracker>();
        private readonly ITrackerService _trackerService;

        /// <summary>
        /// Message to subscribe to for notification of tracker pull failure.
        /// </summary>
        public const string AllTrackersPullFailedMessage = "AllTrackersPullFailed";

        /// <summary>
        /// The <see cref="Tracker.Id"/> of the tracker we're working with.
        /// </summary>
        public int Id
        {
            get => _id;
            set => SetAndRaiseIfChanged(ref _id, value);
        }

        /// <summary>
        /// The <see cref="Tracker.Name"/> of the tracker we're working with.
        /// </summary>
        public string Name
        {
            get => _name;
            set => SetAndRaiseIfChanged(ref _name, value);
        }

        /// <summary>
        /// Message for an error that occured during a command's execution.
        /// </summary>
        public string ErrorMessage
        {
            get => _errorMessage;
            set
            {
                SetAndRaiseIfChanged(ref _errorMessage, value);
                HasError = value.Any();
            }
        }

        /// <summary>
        /// Do we have an <see cref="ErrorMessage"/>?
        /// </summary>
        public bool HasError
        {
            get => _hasError;
            private set => SetAndRaiseIfChanged(ref _hasError, value);
        }

        /// <summary>
        /// All <see cref="Tracker"/>s from the database.
        /// </summary>
        public ObservableCollection<Tracker> AllTrackers
        {
            get => _allTrackers;
            set => SetAndRaiseIfChanged(ref _allTrackers, value);
        }

        /// <summary>
        /// Create and add a tracker to the database with the current <see cref="Name"/>.
        /// </summary>
        public ICommand CreateCommand { get; }

        /// <summary>
        /// Update the tracker with the current <see cref="Id"/> to have the current <see cref="Name"/>.
        /// </summary>
        public ICommand UpdateCommand { get; }

        /// <summary>
        /// Delete a tracker (requires the tracker to delete).
        /// </summary>
        public ICommand DeleteCommand { get; }

        public TrackerViewModel(ITrackerService trackerService)
        {
            _trackerService = trackerService;

            CreateCommand = new Command(async () => await CreateAsync());
            UpdateCommand = new Command(async () => await UpdateAsync());
            DeleteCommand = new Command(async tracker => await DeleteAsync((Tracker)tracker));
            _ = PopulateAllTrackersAsync();
        }

        internal async Task CreateAsync()
        {
            string name = Name?.Trim() ?? "";

            if (!name.Any())
            {
                ErrorMessage = "Your new tracker will need a name.";
                return;
            }

            var tracker = await _trackerService.TryAddTrackerAsync(name);

            if (tracker == null)
            {
                ErrorMessage = "Failed to create the tracker. Please make sure the name is unique.";
                return;
            }

            await PopulateAllTrackersAsync();
            MessagingCenter.Send(this, nameof(CreateCommand), tracker);
            ResetAllValues();
        }

        internal async Task UpdateAsync()
        {
            string name = Name?.Trim() ?? "";

            if (!name.Any())
            {
                ErrorMessage = "Your tracker must have a name.";
                return;
            }

            var tracker = await _trackerService.TryUpdateTrackerNameAsync(Id, name);

            if (tracker == null)
            {
                ErrorMessage = "Failed to update the tracker. Please make sure the name is unique.";
                return;
            }

            await PopulateAllTrackersAsync();
            MessagingCenter.Send(this, nameof(UpdateCommand), tracker);
            ResetAllValues();
        }

        internal async Task DeleteAsync(Tracker tracker)
        {
            if (tracker is null)
            {
                return;
            }

            bool successful = await _trackerService.TryDeleteTrackerAsync(tracker.Id);

            if (!successful)
            {
                MessagingCenter.Send(this, nameof(DeleteCommand));
                return;
            }

            await PopulateAllTrackersAsync();
        }

        internal async Task PopulateAllTrackersAsync()
        {
            var allTrackers = await _trackerService.TryGetAllTrackersAsync();

            if (allTrackers == null)
            {
                MessagingCenter.Send(this, AllTrackersPullFailedMessage);
                return;
            }

            AllTrackers = new ObservableCollection<Tracker>(allTrackers);
        }

        private void ResetAllValues()
        {
            Id = 0;
            Name = "";
            ErrorMessage = "";
        }
    }
}
