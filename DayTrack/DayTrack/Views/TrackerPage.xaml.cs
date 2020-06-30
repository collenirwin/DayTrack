using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DayTrack.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TrackerPage : ContentPage
    {
        public TrackerPage()
        {
            InitializeComponent();
        }

        public TrackerPage(string name) : this()
        {
            Title = name;
        }
    }
}
