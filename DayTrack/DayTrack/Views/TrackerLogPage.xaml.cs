using Autofac;
using DayTrack.Models;
using DayTrack.Services;
using DayTrack.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DayTrack.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TrackerLogPage : ContentPage
    {
        public TrackerLogPage()
        {
            InitializeComponent();
        }

        public TrackerLogPage(Tracker tracker) : this()
        {
            Title = tracker.Name;

            using var scope = App.DependencyContainer.BeginLifetimeScope();
            BindingContext = new TrackerLogViewModel(tracker,
                logService: scope.Resolve<TrackerLogService>(),
                populateGroups: true);
        }
    }
}
