using DayTrack.Views.Models;
using System.Collections.Generic;
using System.ComponentModel;
using Xamarin.Forms;

namespace DayTrack.Views
{
    [DesignTimeVisible(false)]
    public partial class MenuPage : ContentPage
    {
        private readonly List<PageNavigationItem> menuItems = new List<PageNavigationItem>
        {
            new PageNavigationItem { Id = PageIdentifier.Home, Title = "Home" },
            new PageNavigationItem { Id = PageIdentifier.NewTracker, Title = "Create Tracker" },
        };

        private ConductorPage Conductor => Application.Current.MainPage as ConductorPage;

        public MenuPage()
        {
            InitializeComponent();

            MenuListView.ItemsSource = menuItems;
            MenuListView.SelectedItem = menuItems[0];
            MenuListView.ItemTapped += (sender, e) =>
            {
                if (MenuListView.SelectedItem is PageNavigationItem navItem)
                {
                    Conductor.NavigateToPage(navItem.Id);
                }
            };
        }
    }
}