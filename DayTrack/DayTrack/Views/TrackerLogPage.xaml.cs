using Autofac;
using DayTrack.Models;
using DayTrack.Services;
using DayTrack.Utils;
using DayTrack.ViewModels;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DayTrack.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TrackerLogPage : ContentPage
    {
        private readonly Tracker _tracker;
        private readonly TrackerLogViewModel _viewModel;

        public TrackerLogPage()
        {
            InitializeComponent();

            MessagingCenter.Subscribe<TrackerLogViewModel>(this, TrackerLogViewModel.DatabaseErrorMessage,
               sender => this.DisplayAlertOnMain(title: "Error",
                   message: "A database error has occured.",
                   cancel: "OK"));
        }

        public TrackerLogPage(Tracker tracker) : this()
        {
            _tracker = tracker;
            Title = tracker.Name;

            using var scope = App.DependencyContainer.BeginLifetimeScope();
            BindingContext = _viewModel =
                new TrackerLogViewModel(tracker, logService: scope.Resolve<TrackerLogService>());

            SortOptionPicker.SelectedIndex = 0;
            SortOptionPicker.SelectedIndexChanged += OnSortChange;
        }

        private async void OnEditEntries(object sender, EventArgs e) =>
            await App.Conductor.Detail.Navigation.PushAsync(new EditTrackerLogPage(_tracker));

        private async void OnAppearing(object sender, EventArgs e) =>
            await Task.Run(() => _viewModel.PullAllDayGroupsCommand.Execute(null));

        private async void OnSortChange(object sender, EventArgs e)
        {
            if (SortOptionPicker.SelectedIndex != -1)
            {
                _viewModel.SortOptionIndex = SortOptionPicker.SelectedIndex;
                await Task.Run(() => _viewModel.PullAllDayGroupsCommand.Execute(null));
            }
        }
    }
}
