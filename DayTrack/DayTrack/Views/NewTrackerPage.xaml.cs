using DayTrack.ViewModels;
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
        }
    }
}
