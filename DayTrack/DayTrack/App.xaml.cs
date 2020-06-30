using DayTrack.Views;
using Xamarin.Forms;

namespace DayTrack
{
    public partial class App : Application
    {
        public static ConductorPage Conductor => Current.MainPage as ConductorPage;

        public App()
        {
            InitializeComponent();
            MainPage = new ConductorPage();
        }
    }
}
