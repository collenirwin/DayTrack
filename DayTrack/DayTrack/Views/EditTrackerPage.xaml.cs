using DayTrack.Models;
using DayTrack.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DayTrack.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EditTrackerPage : ContentPage
    {
        public EditTrackerPage(Tracker tracker)
        {
            InitializeComponent();

            TrackerViewModel viewModel;
            BindingContext = viewModel = App.DependencyContainer.GetInstance<TrackerViewModel>();

            viewModel.Id = tracker.Id;
            viewModel.Name = tracker.Name;

            MessagingCenter.Subscribe<TrackerViewModel, Tracker>(this, nameof(TrackerViewModel.UpdateCommand),
                (sender, tracker) => App.Conductor.NavigateToTrackerLogPage(tracker));
        }
    }
}
