using Android.OS;
using DayTrack.Services;
using Xamarin.Forms;

[assembly: Dependency(typeof(DayTrack.Droid.Services.AndroidPathService))]
namespace DayTrack.Droid.Services
{
    public class AndroidPathService : IPlatformPathService
    {
        public string DownloadsFolderPath =>
            Environment.GetExternalStoragePublicDirectory(Environment.DirectoryDownloads).AbsolutePath;
    }
}
