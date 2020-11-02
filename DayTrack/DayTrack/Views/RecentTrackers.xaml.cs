using DayTrack.Models;
using DayTrack.ViewModels;
using System.ComponentModel;
using Xamarin.Forms;

namespace DayTrack.Views
{
    [DesignTimeVisible(false)]
    public partial class RecentTrackers : ContentView
    {
        private readonly TrackerViewModel _viewModel;

        public RecentTrackers()
        {
            InitializeComponent();
            BindingContext = _viewModel = App.DependencyContainer.GetInstance<TrackerViewModel>();
            _viewModel.PullRecentCommand.Execute(null);
        }

        private void OnTrackerSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem is Tracker tracker)
            {
                App.Conductor.NavigateToTrackerLogPage(tracker);
                TrackersListView.SelectedItem = null;
            }
        }
    }
}
