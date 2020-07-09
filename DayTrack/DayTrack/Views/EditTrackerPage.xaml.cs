using Autofac;
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
            using (var scope = App.DependencyContainer.BeginLifetimeScope())
            {
                BindingContext = viewModel = scope.Resolve<TrackerViewModel>();
            }

            viewModel.Id = tracker.Id;
            viewModel.Name = tracker.Name;

            MessagingCenter.Subscribe<TrackerViewModel, string>(this, nameof(TrackerViewModel.UpdateCommand),
                (sender, name) => App.Conductor.NavigateToTrackerPage(name));
        }
    }
}
