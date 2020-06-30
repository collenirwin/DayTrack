using DayTrack.ViewModels;
using DayTrack.Views.Models;
using System.Security.Cryptography.X509Certificates;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DayTrack.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NewTrackerPage : ContentPage
    {
        public NewTrackerPage()
        {
            InitializeComponent();
            BindingContext = new NewTrackerViewModel();

            MessagingCenter.Subscribe<NewTrackerViewModel, string>(this, nameof(NewTrackerViewModel.CreateCommand),
                (sender, name) => App.Conductor.NavigateToTrackerPage(name));
        }
    }
}
