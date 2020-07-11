using Autofac;
using DayTrack.Utils;
using DayTrack.ViewModels;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DayTrack.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ExportPage : ContentPage
    {
        public ExportPage()
        {
            InitializeComponent();

            using (var scope = App.DependencyContainer.BeginLifetimeScope())
            {
                BindingContext = scope.Resolve<ExportViewModel>();
            }

            MessagingCenter.Subscribe<ExportViewModel>(this, nameof(ExportViewModel.ExportCommand),
                sender => this.DisplayAlertOnMain(title: "Error",
                    message: $"Failed to export the selected tracker.",
                    cancel: "OK"));

            MessagingCenter.Subscribe<ExportViewModel, Exception>(this, nameof(ExportViewModel.ExportCommand),
                (sender, ex) => this.DisplayAlertOnMain(title: "Error",
                    message: $"Error when saving to export file: {ex.Message}",
                    cancel: "OK"));

            MessagingCenter.Subscribe<ExportViewModel, string>(this, nameof(ExportViewModel.ExportCommand),
                (sender, fileName) => this.DisplayAlertOnMain(title: "Success",
                    message: $"Export complete (file name: {fileName}).",
                    cancel: "OK"));
        }
    }
}
