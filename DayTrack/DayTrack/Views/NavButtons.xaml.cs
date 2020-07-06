using DayTrack.Views.Models;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DayTrack.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NavButtons : ContentView
    {
        public NavButtons()
        {
            InitializeComponent();
        }

        private void OnHomeClick(object sender, EventArgs e) => NavigateToPage(PageIdentifier.Home);

        private void OnNewTrackerClick(object sender, EventArgs e) => NavigateToPage(PageIdentifier.NewTracker);

        private void OnAboutClick(object sender, EventArgs e) => NavigateToPage(PageIdentifier.About);

        private void OnSettingsClick(object sender, EventArgs e) => NavigateToPage(PageIdentifier.Settings);

        private void NavigateToPage(PageIdentifier pageId) => App.Conductor.NavigateToPage(pageId);
    }
}
