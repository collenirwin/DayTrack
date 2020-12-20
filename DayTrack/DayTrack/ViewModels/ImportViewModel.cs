using DayTrack.Models;
using DayTrack.Services;
using DayTrack.Utils;
using Plugin.FilePicker;
using Plugin.FilePicker.Abstractions;
using System;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

[assembly: InternalsVisibleTo("DayTrack.Tests")]
namespace DayTrack.ViewModels
{
    public class ImportViewModel : ViewModelBase
    {
        private FileData _selectedFile;
        private Tracker _selectedTracker;
        private readonly ITrackerLogService _logService;

        public FileData SelectedFile
        {
            get => _selectedFile;
            set => SetAndRaiseIfChanged(ref _selectedFile, value);
        }

        public Tracker SelectedTracker
        {
            get => _selectedTracker;
            set => SetAndRaiseIfChanged(ref _selectedTracker, value);
        }

        public TrackerViewModel TrackerViewModel { get; }
        public ICommand SelectFileCommand { get; }
        public ICommand ImportCommand { get; }

        public ImportViewModel(ITrackerLogService logService, TrackerViewModel trackerViewModel)
        {
            _logService = logService;
            TrackerViewModel = trackerViewModel;

            SelectFileCommand = new Command(async () => await SelectFileAsync());
            ImportCommand = new Command(async () => await ImportAsync().ExpressLoading(this));
        }

        internal async Task SelectFileAsync()
        {
            try
            {
                var file = await CrossFilePicker.Current.PickFile(new[] { "text/plain" });

                // user cancelled selection?
                if (file == null)
                {
                    return;
                }

                SelectedFile = file;
            }
            catch (Exception ex)
            {
                MessagingCenter.Send(this, nameof(SelectFileCommand), ex);
            }
        }

        internal async Task ImportAsync()
        {
            if (SelectedFile == null || SelectedTracker == null)
            {
                return;
            }

            try
            {
                var days = Encoding.UTF8.GetString(SelectedFile.DataArray)
                    .Trim()
                    .Split('\n')
                    .Select(line =>
                        DateTime.ParseExact(line.Trim(), DateFormats.ShortYearMonthDay, CultureInfo.InvariantCulture));

                bool successful = await _logService.TryBulkAddEntriesAsync(days, SelectedTracker.Id);

                if (!successful)
                {
                    MessagingCenter.Send(this, nameof(ImportCommand));
                    return;
                }

                MessagingCenter.Send(this, nameof(ImportCommand), SelectedTracker);
                ResetAllValues();
            }
            catch (Exception ex)
            {
                MessagingCenter.Send(this, nameof(ImportCommand), ex);
            }
        }

        private void ResetAllValues()
        {
            SelectedFile = null;
            SelectedTracker = null;
        }
    }
}
