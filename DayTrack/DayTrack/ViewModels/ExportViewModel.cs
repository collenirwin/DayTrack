using DayTrack.Models;
using DayTrack.Services;
using DayTrack.Utils;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace DayTrack.ViewModels
{
    public class ExportViewModel : ViewModelBase
    {
        private Tracker _selectedTracker;
        private readonly TrackerLogService _logService;

        public Tracker SelectedTracker
        {
            get => _selectedTracker;
            set => SetAndRaiseIfChanged(ref _selectedTracker, value);
        }

        public TrackerViewModel TrackerViewModel { get; }
        public ICommand ExportCommand { get; }

        public ExportViewModel(TrackerLogService logService, TrackerViewModel trackerViewModel)
        {
            _logService = logService;
            TrackerViewModel = trackerViewModel;

            ExportCommand = new Command(async () => await ExportAsync().ExpressLoading(this));
        }

        private async Task ExportAsync()
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
            string path = Path
                .Combine(DependencyService.Get<IPlatformPathService>().DownloadsFolderPath, fileName);

            var dates = (await _logService.TryGetAllLoggedDaysAsync(SelectedTracker.Id))?
                .Select(day => day.Date.ToShortDateString());

            if (dates == null)
            {
                MessagingCenter.Send(this, nameof(ExportCommand));
                return;
            }

            try
            {
                File.WriteAllText(path, string.Join("\n", dates));
            }
            catch (Exception ex)
            {
                MessagingCenter.Send(this, nameof(ExportCommand), ex);
                return;
            }
            
            MessagingCenter.Send(this, nameof(ExportCommand), fileName);
            ResetAllValues();
        }

        private void ResetAllValues()
        {
            SelectedTracker = null;
        }
    }
}
