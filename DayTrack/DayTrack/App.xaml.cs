using DayTrack.Data;
using DayTrack.Services;
using DayTrack.ViewModels;
using DayTrack.Views;
using Serilog;
using SimpleInjector;
using System.IO;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace DayTrack
{
    public partial class App : Application
    {
        public static ConductorPage Conductor => Current.MainPage as ConductorPage;
        public static Container DependencyContainer { get; private set; }

        public App()
        {
            InitializeComponent();
            VersionTracking.Track();

            var logger = new LoggerConfiguration()
                .WriteTo.File(Path.Combine(FileSystem.AppDataDirectory, "log.txt"),
                    rollingInterval: RollingInterval.Day)
                .CreateLogger();

            string databasePath = Path.Combine(FileSystem.AppDataDirectory, "app.db");
            var database = new AppDatabase(databasePath);

            string userSettingsPath = Path.Combine(FileSystem.AppDataDirectory, "user_settings.json");
            var settingsService = new SettingsService(userSettingsPath, logger);

            DependencyContainer = new Container();
            DependencyContainer.RegisterSingleton<ILogger>(() => logger);
            DependencyContainer.RegisterSingleton(() => database);
            DependencyContainer.RegisterSingleton(() => settingsService);
            DependencyContainer.RegisterSingleton<ITrackerService, TrackerService>();
            DependencyContainer.RegisterSingleton<ITrackerLogService, TrackerLogService>();
            DependencyContainer.RegisterSingleton<TrackerViewModel>();
            DependencyContainer.RegisterSingleton<ImportViewModel>();
            DependencyContainer.RegisterSingleton<ExportViewModel>();
            DependencyContainer.RegisterSingleton<SettingsViewModel>();

            MainPage = new ConductorPage();
        }
    }
}
