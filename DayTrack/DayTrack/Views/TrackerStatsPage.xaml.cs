using DayTrack.Models;
using DayTrack.ViewModels;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DayTrack.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TrackerStatsPage : ContentPage
    {
        private readonly TrackerLogViewModel _viewModel;

        public TrackerStatsPage()
        {
            InitializeComponent();
        }

        public TrackerStatsPage(Tracker tracker, TrackerLogViewModel trackerLogViewModel) : this()
        {
            Title = $"{tracker.Name} stats";
            BindingContext = _viewModel = trackerLogViewModel;
        }

        private async void OnAppearing(object sender, EventArgs e) =>
            await Task.Run(() => _viewModel.PullStatsCommand.Execute(null));
    }
}