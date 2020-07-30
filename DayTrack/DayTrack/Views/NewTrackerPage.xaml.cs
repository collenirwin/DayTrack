using DayTrack.Models;
using DayTrack.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DayTrack.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NewTrackerPage : ContentPage
    {
        public NewTrackerPage()
        {
            InitializeComponent();
            BindingContext = App.DependencyContainer.GetInstance<TrackerViewModel>();

            MessagingCenter.Subscribe<TrackerViewModel, Tracker>(this, nameof(TrackerViewModel.CreateCommand),
                (sender, tracker) => App.Conductor.NavigateToTrackerLogPage(tracker));
        }
    }
}
