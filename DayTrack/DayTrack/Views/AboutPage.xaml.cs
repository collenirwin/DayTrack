using System;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DayTrack.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AboutPage : ContentPage
    {
        public string VersionNumber { get; }

        public AboutPage()
        {
            InitializeComponent();

            VersionNumber = VersionTracking.CurrentVersion;
            BindingContext = this;
        }

        private async void OnGitHubLinkClick(object sender, EventArgs e) =>
            await Launcher.OpenAsync("https://github.com/collenirwin/DayTrack");
    }
}
