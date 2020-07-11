using Autofac;
using DayTrack.Models;
using DayTrack.Utils;
using DayTrack.ViewModels;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DayTrack.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ImportPage : ContentPage
    {
        public ImportPage()
        {
            InitializeComponent();

            using (var scope = App.DependencyContainer.BeginLifetimeScope())
            {
                BindingContext = scope.Resolve<ImportViewModel>();
            }

            MessagingCenter.Subscribe<ImportViewModel, Exception>(this, nameof(ImportViewModel.SelectFileCommand),
                (sender, ex) => this.DisplayAlertOnMain(title: "Error",
                    message: $"Failed to select the file (details: {ex.Message}).",
                    cancel: "OK"));

            MessagingCenter.Subscribe<ImportViewModel, Exception>(this, nameof(ImportViewModel.ImportCommand),
                (sender, ex) => this.DisplayAlertOnMain(title: "Error",
                    message: $"Error when parsing selected file: {ex.Message}.",
                    cancel: "OK"));

            MessagingCenter.Subscribe<ImportViewModel>(this, nameof(ImportViewModel.ImportCommand),
                sender => this.DisplayAlertOnMain(title: "Error",
                    message: $"Import operation failed.",
                    cancel: "OK"));

            MessagingCenter.Subscribe<ImportViewModel, Tracker>(this, nameof(ImportViewModel.ImportCommand),
                (sender, tracker) => App.Conductor.NavigateToTrackerLogPage(tracker));
        }
    }
}
