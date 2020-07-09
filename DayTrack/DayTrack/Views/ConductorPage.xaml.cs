using DayTrack.Models;
using DayTrack.Views.Models;
using System.Collections.Generic;
using System.ComponentModel;
using Xamarin.Forms;

namespace DayTrack.Views
{
    [DesignTimeVisible(false)]
    public partial class ConductorPage : MasterDetailPage
    {
        private readonly Dictionary<PageIdentifier, NavigationPage> _pages;

        public ConductorPage()
        {
            InitializeComponent();

            _pages = new Dictionary<PageIdentifier, NavigationPage>
            {
                { PageIdentifier.Home, (NavigationPage)Detail }
            };

            MasterBehavior = MasterBehavior.Popover;
        }

        public void NavigateToPage(PageIdentifier pageId)
        {
            if (!_pages.ContainsKey(pageId))
            {
                switch (pageId)
                {
                    case PageIdentifier.NewTracker:
                        _pages.Add(pageId, new NavigationPage(new NewTrackerPage()));
                        break;
                    case PageIdentifier.About:
                        _pages.Add(pageId, new NavigationPage(new AboutPage()));
                        break;
                    case PageIdentifier.Settings:
                        _pages.Add(pageId, new NavigationPage(new SettingsPage()));
                        break;
                }
            }

            var newPage = _pages[pageId];

            if (newPage != null && newPage != Detail)
            {
                Detail = newPage;
            }

            IsPresented = false;
        }

        public void NavigateToTrackerLogPage(Tracker tracker)
        {
            if (tracker != null)
            {
                Detail = new NavigationPage(new TrackerLogPage(tracker));
            }

            IsPresented = false;
        }

        public void NavigateToEditTrackerPage(Tracker tracker)
        {
            if (tracker != null)
            {
                Detail = new NavigationPage(new EditTrackerPage(tracker));
            }

            IsPresented = false;
        }
    }
}
