using Xamarin.Forms;

namespace DayTrack.Utils
{
    public static class ContentPageExtensions
    {
        public static void DisplayAlertOnMain<TPage>(this TPage page, string title, string message, string cancel)
            where TPage : ContentPage
        {
            Device.BeginInvokeOnMainThread(async () => await page.DisplayAlert(title, message, cancel));
        }
    }
}
