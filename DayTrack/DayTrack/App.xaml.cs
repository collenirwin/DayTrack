using Autofac;
using DayTrack.Data;
using DayTrack.Services;
using DayTrack.ViewModels;
using DayTrack.Views;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System;
using System.IO;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace DayTrack
{
    public partial class App : Application
    {
        public static ConductorPage Conductor => Current.MainPage as ConductorPage;
        public static IContainer DependencyContainer { get; private set; }

        public App()
        {
            InitializeComponent();
            VersionTracking.Track();

            var builder = new ContainerBuilder();

            var logger = new LoggerConfiguration()
                .WriteTo.File(Path.Combine(FileSystem.AppDataDirectory, "log.txt"),
                    rollingInterval: RollingInterval.Day)
                .CreateLogger();

            string databasePath = Path
                .Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "app.db");

            var context = new AppDbContext(new DbContextOptionsBuilder<AppDbContext>()
                .UseSqlite($"Data source={databasePath}")
                .Options);

            context.Database.Migrate();

            builder.RegisterInstance(logger).As<ILogger>().SingleInstance();
            builder.RegisterInstance(context).SingleInstance();
            builder.RegisterType<TrackerService>().SingleInstance();
            builder.RegisterType<TrackerLogService>().SingleInstance();
            builder.RegisterType<TrackerViewModel>().SingleInstance();
            builder.RegisterType<ImportViewModel>().SingleInstance();
            builder.RegisterType<ExportViewModel>().SingleInstance();

            DependencyContainer = builder.Build();
            MainPage = new ConductorPage();
        }
    }
}
