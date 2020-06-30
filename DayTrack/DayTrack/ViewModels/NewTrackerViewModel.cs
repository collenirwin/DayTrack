using System.Linq;
using Xamarin.Forms;

namespace DayTrack.ViewModels
{
    public class NewTrackerViewModel : ViewModelBase
    {
        private string _name = "";

        public string Name
        {
            get => _name;
            set => SetAndRaiseIfChanged(ref _name, value);
        }

        public Command CreateCommand { get; }

        public NewTrackerViewModel()
        {
            CreateCommand = new Command(execute: () =>
            {
                // TODO: add an entry for this tracker in the db
                // possibly use the message center to notify the view of success or failure
            },
            canExecute: () => Name.Trim().Any());
        }
    }
}
