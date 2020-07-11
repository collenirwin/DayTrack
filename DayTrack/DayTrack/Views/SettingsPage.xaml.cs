using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DayTrack.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SettingsPage : ContentPage
    {
        public SettingsPage()
        {
            InitializeComponent();
        }

        private async void OnImportClick(object sender, EventArgs e) =>
            await App.Conductor.Detail.Navigation.PushAsync(new ImportPage());

        private async void OnExportClick(object sender, EventArgs e) =>
            await App.Conductor.Detail.Navigation.PushAsync(new ExportPage());
    }
}
