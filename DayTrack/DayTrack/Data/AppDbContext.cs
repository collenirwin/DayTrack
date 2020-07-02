using DayTrack.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.IO;

namespace DayTrack.Data
{
    /// <summary>
    /// The application's local database context.
    /// </summary>
    public class AppDbContext : DbContext
    {
        private const string _fileName = "app.db";
        private readonly string _path = Path
            .Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), _fileName);

        public DbSet<Tracker> Trackers { get; set; }
        public DbSet<LoggedDay> LoggedDays { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) =>
            optionsBuilder.UseSqlite($"Data source={_path}");

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Tracker>()
                .HasIndex(tracker => tracker.Name)
                .IsUnique();
        }
    }
}
