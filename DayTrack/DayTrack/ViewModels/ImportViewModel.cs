using DayTrack.Services;
using Plugin.FilePicker;
using Plugin.FilePicker.Abstractions;
using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace DayTrack.ViewModels
{
    public class ImportViewModel : ViewModelBase
    {
        private FileData _selectedFile;
        private readonly TrackerLogService _logService;

        public FileData SelectedFile
        {
            get => _selectedFile;
            set => SetAndRaiseIfChanged(ref _selectedFile, value);
        }

        public ICommand SelectFileCommand { get; }

        public ImportViewModel(TrackerLogService logService)
        {
            _logService = logService;

            SelectFileCommand = new Command(async () => await SelectFileAsync());
        }

        private async Task SelectFileAsync()
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
    }
}
