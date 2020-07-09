using Autofac;
using DayTrack.Models;
using DayTrack.Services;
using DayTrack.ViewModels;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DayTrack.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EditTrackerLogPage : ContentPage
    {
        private readonly TrackerLogViewModel _viewModel;

        public EditTrackerLogPage()
        {
            InitializeComponent();
        }

        public EditTrackerLogPage(Tracker tracker) : this()
        {
            Title = $"Editing {tracker.Name}";

            using var scope = App.DependencyContainer.BeginLifetimeScope();
            BindingContext = _viewModel =
                new TrackerLogViewModel(tracker, logService: scope.Resolve<TrackerLogService>());
        }

        private async void OnAppearing(object sender, EventArgs e) =>
            await Task.Run(() => _viewModel.PullAllDaysCommand.Execute(null));
    }
}
