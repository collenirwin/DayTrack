using DayTrack.Data;
using DayTrack.Services;
using DayTrack.ViewModels;
using DayTrack.Views;
using Microsoft.EntityFrameworkCore;
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
            var context = new AppDbContext(new DbContextOptionsBuilder<AppDbContext>()
                .UseSqlite($"Data source={databasePath}")
                .Options);

            context.Database.Migrate();

            DependencyContainer = new Container();
            DependencyContainer.RegisterSingleton<ILogger>(() => logger);
            DependencyContainer.RegisterSingleton(() => context);
            DependencyContainer.RegisterSingleton<ITrackerService, TrackerService>();
            DependencyContainer.RegisterSingleton<ITrackerLogService, TrackerLogService>();
            DependencyContainer.RegisterSingleton<TrackerViewModel>();
            DependencyContainer.RegisterSingleton<ImportViewModel>();
            DependencyContainer.RegisterSingleton<ExportViewModel>();

            MainPage = new ConductorPage();
        }
    }
}
