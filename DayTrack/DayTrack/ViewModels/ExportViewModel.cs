using DayTrack.Models;
using DayTrack.Services;
using DayTrack.Utils;
using Plugin.Permissions;
using System;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;
using PermissionStatus = Plugin.Permissions.Abstractions.PermissionStatus;

[assembly: InternalsVisibleTo("DayTrack.Tests")]
namespace DayTrack.ViewModels
{
    public class ExportViewModel : ViewModelBase
    {
        private Tracker _selectedTracker;
        private readonly ITrackerLogService _logService;

        public Tracker SelectedTracker
        {
            get => _selectedTracker;
            set => SetAndRaiseIfChanged(ref _selectedTracker, value);
        }

        public TrackerViewModel TrackerViewModel { get; }
        public ICommand ExportCommand { get; }

        public ExportViewModel(ITrackerLogService logService, TrackerViewModel trackerViewModel)
        {
            _logService = logService;
            TrackerViewModel = trackerViewModel;

            ExportCommand = new Command(async () => await ExportAsync().ExpressLoading(this));
        }

        internal async Task ExportAsync()
        {
            if (SelectedTracker == null)
            {
                return;
            }

            var permissionStatus = await CrossPermissions.Current.CheckPermissionStatusAsync<StoragePermission>();

            if (permissionStatus != PermissionStatus.Granted)
            {
                permissionStatus = await CrossPermissions.Current.RequestPermissionAsync<StoragePermission>();

                if (permissionStatus != PermissionStatus.Granted)
                {
                    return;
                }
            }

            string fileName = $"daytracker_{SelectedTracker.Name}.txt";
            string path = Path.Combine(FileSystem.CacheDirectory, fileName);

            var dates = (await _logService.TryGetAllLoggedDaysAsync(SelectedTracker.Id))?
                .Select(day => day.Date.ToString(DateFormats.ShortYearMonthDay));

            if (dates == null)
            {
                MessagingCenter.Send(this, nameof(ExportCommand));
                return;
            }

            try
            {
                File.WriteAllText(path, string.Join("\n", dates));

                await Share.RequestAsync(new ShareFileRequest
                {
                    Title = $"{SelectedTracker.Name} export",
                    File = new ShareFile(path)
                });
            }
            catch (Exception ex)
            {
                MessagingCenter.Send(this, nameof(ExportCommand), ex);
                return;
            }
            
            ResetAllValues();
        }

        private void ResetAllValues()
        {
            SelectedTracker = null;
        }
    }
}
