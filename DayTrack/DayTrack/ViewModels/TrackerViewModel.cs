using DayTrack.Models;
using DayTrack.Services;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace DayTrack.ViewModels
{
    public class TrackerViewModel : ViewModelBase
    {
        private int _id = 0;
        private string _name = "";
        private string _errorMessage = "";
        private bool _hasError = false;
        private readonly TrackerService _trackerService;

        public const string AllTrackersPullFailedMessage = "AllTrackersPullFailed";

        public int Id
        {
            get => _id;
            set => SetAndRaiseIfChanged(ref _id, value);
        }

        public string Name
        {
            get => _name;
            set => SetAndRaiseIfChanged(ref _name, value);
        }

        public string ErrorMessage
        {
            get => _errorMessage;
            set
            {
                SetAndRaiseIfChanged(ref _errorMessage, value);
                HasError = value.Any();
            }
        }

        public bool HasError
        {
            get => _hasError;
            private set => SetAndRaiseIfChanged(ref _hasError, value);
        }

        public ObservableCollection<Tracker> AllTrackers { get; } = new ObservableCollection<Tracker>();
        public Command CreateCommand { get; }
        public Command UpdateCommand { get; }
        public Command DeleteCommand { get; }

        public TrackerViewModel(TrackerService trackerService)
        {
            _trackerService = trackerService;

            CreateCommand = new Command(async () => await CreateAsync());
            UpdateCommand = new Command(async () => await UpdateAsync());
            DeleteCommand = new Command(async tracker => await DeleteAsync((Tracker)tracker));
            _ = PopulateAllTrackersAsync();
        }

        private async Task CreateAsync()
        {
            string name = Name.Trim();

            if (!name.Any())
            {
                ErrorMessage = "Your new tracker will need a name.";
                return;
            }

            bool successful = await _trackerService.TryAddTrackerAsync(name);

            if (!successful)
            {
                ErrorMessage = "Failed to create the tracker. Please make sure the name is unique.";
                return;
            }

            await PopulateAllTrackersAsync();

            // notify all listeners that this command has finished successfully with the name of the new tracker
            MessagingCenter.Send(this, nameof(CreateCommand), Name);
            ResetAllValues();
        }

        private async Task UpdateAsync()
        {
            string name = Name.Trim();

            if (!name.Any())
            {
                ErrorMessage = "Your tracker must have a name.";
                return;
            }

            bool successful = await _trackerService.TryUpdateTrackerNameAsync(Id, name);

            if (!successful)
            {
                ErrorMessage = "Failed to update the tracker. Please make sure the name is unique.";
                return;
            }

            await PopulateAllTrackersAsync();
            MessagingCenter.Send(this, nameof(UpdateCommand), Name);
            ResetAllValues();
        }

        private async Task DeleteAsync(Tracker tracker)
        {
            bool successful = await _trackerService.TryDeleteTrackerAsync(tracker.Id);

            if (!successful)
            {
                MessagingCenter.Send(this, nameof(DeleteCommand));
                return;
            }

            await PopulateAllTrackersAsync();
        }

        private async Task PopulateAllTrackersAsync()
        {
            var allTrackers = await _trackerService.TryGetAllTrackersAsync();

            if (allTrackers == null)
            {
                MessagingCenter.Send(this, AllTrackersPullFailedMessage);
                return;
            }

            AllTrackers.Clear();
            foreach (var tracker in allTrackers)
            {
                AllTrackers.Add(tracker);
            }
        }

        private void ResetAllValues()
        {
            Id = 0;
            Name = "";
            ErrorMessage = "";
        }
    }
}
