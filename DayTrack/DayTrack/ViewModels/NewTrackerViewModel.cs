using System.Linq;
using Xamarin.Forms;

namespace DayTrack.ViewModels
{
    public class NewTrackerViewModel : ViewModelBase
    {
        private string _name = "";
        private string _errorMessage = "";
        private bool _hasError = false;

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

        public NewTrackerViewModel()
        {
            CreateCommand = new Command(Create);
        }

        private void Create()
        {
            string name = Name.Trim();

            if (!name.Any())
            {
                ErrorMessage = "Your new tracker will need a name.";
                return;
            }

            // TODO: add an entry for this tracker in the db

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
