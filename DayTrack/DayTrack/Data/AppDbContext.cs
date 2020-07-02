using DayTrack.Models;
using Microsoft.EntityFrameworkCore;
using System.IO;
using Xamarin.Essentials;

namespace DayTrack.Data
{
    /// <summary>
    /// The application's local database context.
    /// </summary>
    public class AppDbContext : DbContext
    {
        private const string _fileName = "app.db";
        private readonly string _path = Path.Combine(FileSystem.AppDataDirectory, _fileName);

        public DbSet<Tracker> Trackers { get; set; }
        public DbSet<LoggedDay> LoggedDays { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) =>
            optionsBuilder.UseSqlite($"Data source={_path}");
    }
}
