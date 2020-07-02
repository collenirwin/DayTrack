using Autofac;
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

            using (var scope = App.DependencyContainer.BeginLifetimeScope())
            {
                BindingContext = scope.Resolve<TrackerViewModel>();
            }

            MessagingCenter.Subscribe<TrackerViewModel, string>(this, nameof(TrackerViewModel.CreateCommand),
                (sender, name) => App.Conductor.NavigateToTrackerPage(name));
        }
    }
}
