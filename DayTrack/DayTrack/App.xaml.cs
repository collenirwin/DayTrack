using Autofac;
using DayTrack.Data;
using DayTrack.Services;
using DayTrack.ViewModels;
using DayTrack.Views;
using Microsoft.EntityFrameworkCore;
using Serilog;
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
            MainPage = new ConductorPage();
        }

        protected override void OnStart()
        {
            base.OnStart();

            var builder = new ContainerBuilder();

            var logger = new LoggerConfiguration()
                .WriteTo.File("log.txt", rollingInterval: RollingInterval.Day)
                .CreateLogger();

            var context = new AppDbContext();
            context.Database.Migrate();

            builder.RegisterInstance(logger).As<ILogger>().SingleInstance();
            builder.RegisterInstance(context).SingleInstance();
            builder.RegisterType<TrackerService>().SingleInstance();
            builder.RegisterType<NewTrackerViewModel>().SingleInstance();

            DependencyContainer = builder.Build();
        }
    }
}
