﻿using Autofac;
using DayTrack.Services;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace DayTrack.ViewModels
{
    public class NewTrackerViewModel : ViewModelBase
    {
        private string _name = "";
        private string _errorMessage = "";
        private bool _hasError = false;
        private readonly TrackerService _trackerService;

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

        public Command CreateCommand { get; }

        public NewTrackerViewModel(TrackerService trackerService)
        {
            _trackerService = trackerService;

            CreateCommand = new Command(async () => await Create());
        }

        private async Task Create()
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

            // notify all listeners that this command has finished successfully with the name of the new tracker
            MessagingCenter.Send(this, nameof(CreateCommand), Name);
            ResetAllValues();
        }

        private void ResetAllValues()
        {
            Name = "";
            ErrorMessage = "";
        }
    }
}
