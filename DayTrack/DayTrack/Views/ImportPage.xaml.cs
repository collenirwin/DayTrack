using Autofac;
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
        }
    }
}
