using DayTrack.Models;
using Microsoft.EntityFrameworkCore;

namespace DayTrack.Data
{
    /// <summary>
    /// The application's local database context.
    /// </summary>
    public class AppDbContext : DbContext
    {
        public DbSet<Tracker> Trackers { get; set; }
        public DbSet<LoggedDay> LoggedDays { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Tracker>()
                .HasIndex(tracker => tracker.Name)
                .IsUnique();

            modelBuilder.Entity<Tracker>()
                .HasMany(tracker => tracker.LoggedDays)
                .WithOne(day => day.Tracker)
                .IsRequired();
        }
    }
}
