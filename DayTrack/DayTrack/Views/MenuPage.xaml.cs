﻿using DayTrack.Models;
using DayTrack.Utils;
using DayTrack.ViewModels;
using DayTrack.Views.Models;
using System;
using System.ComponentModel;
using Xamarin.Forms;

namespace DayTrack.Views
{
    [DesignTimeVisible(false)]
    public partial class MenuPage : ContentPage
    {
        private readonly TrackerViewModel _viewModel;

        public MenuPage()
        {
            InitializeComponent();

            MessagingCenter.Subscribe<TrackerViewModel>(this, TrackerViewModel.AllTrackersPullFailedMessage,
                sender => this.DisplayAlertOnMain(title: "Error",
                    message: "Database operation failed. Please try again. " +
                        "If the problem persists, the app's data may be corrupt.",
                    cancel: "OK"));

            MessagingCenter.Subscribe<TrackerViewModel>(this, nameof(TrackerViewModel.DeleteCommand),
                sender => this.DisplayAlertOnMain(title: "Error",
                    message: "Failed to delete the tracker.",
                    cancel: "OK"));

            BindingContext = _viewModel = App.DependencyContainer.GetInstance<TrackerViewModel>();
        }

        private async void OnTrackerDelete(object sender, EventArgs e)
        {
            bool shouldDelete = await DisplayAlert(title: "Delete tracker",
                message: "You are about to delete this tracker and all days logged under it.",
                accept: "DELETE",
                cancel: "CANCEL");

            if (shouldDelete)
            {
                App.Conductor.NavigateToPage(PageIdentifier.Home, keepMenuOpen: true);
                _viewModel.DeleteCommand.Execute((sender as MenuItem).CommandParameter);
            }
        }

        private void OnTrackerEdit(object sender, EventArgs e) =>
            App.Conductor.NavigateToEditTrackerPage((sender as MenuItem).CommandParameter as Tracker);

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
