using Autofac;
using DayTrack.ViewModels;
using DayTrack.Views.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using Xamarin.Forms;

namespace DayTrack.Views
{
    [DesignTimeVisible(false)]
    public partial class MenuPage : ContentPage
    {
        private readonly List<PageNavigationItem> _menuItems = new List<PageNavigationItem>
        {
            new PageNavigationItem { Id = PageIdentifier.Home, Title = "Home" },
            new PageNavigationItem { Id = PageIdentifier.NewTracker, Title = "Create Tracker" },
        };

        private readonly TrackerViewModel _viewModel;

        public MenuPage()
        {
            InitializeComponent();

            MessagingCenter.Subscribe<TrackerViewModel>(this, TrackerViewModel.AllTrackersPullFailedMessage,
                async sender => await DisplayAlert(title: "Error",
                    message: "Database operation failed. Please try again. " +
                        "If the problem persists, the app's data may be corrupt.",
                    cancel: "OK"));

            MessagingCenter.Subscribe<TrackerViewModel>(this, nameof(TrackerViewModel.DeleteCommand),
                async sender => await DisplayAlert(title: "Error",
                    message: "Failed to delete the tracker.",
                    cancel: "OK"));

            using (var scope = App.DependencyContainer.BeginLifetimeScope())
            {
                BindingContext = _viewModel = scope.Resolve<TrackerViewModel>();
            }

            MenuListView.ItemsSource = _menuItems;
            MenuListView.SelectedItem = _menuItems[0];
            MenuListView.ItemTapped += (sender, e) =>
            {
                if (MenuListView.SelectedItem is PageNavigationItem navItem)
                {
                    App.Conductor.NavigateToPage(navItem.Id);
                }
            };
        }

        private async void OnTrackerDelete(object sender, EventArgs e)
        {
            bool shouldDelete = await DisplayAlert(title: "Delete tracker",
                message: "You are about to delete this tracker and all days logged under it.",
                accept: "DELETE",
                cancel: "CANCEL");

            if (shouldDelete)
            {
                _viewModel.DeleteCommand.Execute((sender as MenuItem).CommandParameter);
            }
        }
    }
}
