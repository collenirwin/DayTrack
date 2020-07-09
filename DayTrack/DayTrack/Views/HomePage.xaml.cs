using DayTrack.Views.Models;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DayTrack.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HomePage : ContentPage
    {
        public HomePage()
        {
            InitializeComponent();
        }

        private void OnNewTrackerClick(object sender, EventArgs e) =>
            App.Conductor.NavigateToPage(PageIdentifier.NewTracker);

        private void OnViewTrackersClick(object sender, EventArgs e) =>
            App.Conductor.IsPresented = true;
    }
}
