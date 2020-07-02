using Autofac;
using DayTrack.ViewModels;
using DayTrack.Views.Models;
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

        public MenuPage()
        {
            InitializeComponent();

            using (var scope = App.DependencyContainer.BeginLifetimeScope())
            {
                BindingContext = scope.Resolve<TrackerViewModel>();
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
    }
}