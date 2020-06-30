using DayTrack.Views;
using Xamarin.Forms;

namespace DayTrack
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            MainPage = new ConductorPage();
        }
    }
}
