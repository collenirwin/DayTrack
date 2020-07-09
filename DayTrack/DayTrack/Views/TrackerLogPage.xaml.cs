﻿using Autofac;
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
    public partial class TrackerLogPage : ContentPage
    {
        private readonly Tracker _tracker;
        private readonly TrackerLogViewModel _viewModel;

        public TrackerLogPage()
        {
            InitializeComponent();
        }

        public TrackerLogPage(Tracker tracker) : this()
        {
            _tracker = tracker;
            Title = tracker.Name;

            using var scope = App.DependencyContainer.BeginLifetimeScope();
            BindingContext = _viewModel =
                new TrackerLogViewModel(tracker, logService: scope.Resolve<TrackerLogService>());
        }

        private async void OnEditEntries(object sender, EventArgs e) =>
            await App.Conductor.Detail.Navigation.PushAsync(new EditTrackerLogPage(_tracker));

        private async void OnAppearing(object sender, EventArgs e) =>
            await Task.Run(() => _viewModel.PullAllDayGroupsCommand.Execute(null));
    }
}
