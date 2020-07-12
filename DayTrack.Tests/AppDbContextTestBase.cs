using DayTrack.Data;
using DayTrack.Models;
using Microsoft.EntityFrameworkCore;
using System;

namespace DayTrack.Tests
{
    public abstract class AppDbContextTestBase : IDisposable
    {
        protected readonly AppDbContext _context;

        public AppDbContextTestBase()
        {
            _context = new AppDbContext(new DbContextOptionsBuilder<AppDbContext>()
               .UseSqlite("Data source=test.db")
               .Options);

            Seed();
        }

        private void Seed()
        {
            _context.Database.EnsureDeleted();
            _context.Database.EnsureCreated();

            _context.Trackers.Add(new Tracker
            {
                Name = "T0",
                LoggedDays = new[]
                {
                    new LoggedDay { Date = new DateTime(2020, 1, 1) },
                    new LoggedDay { Date = new DateTime(2020, 1, 1) },
                    new LoggedDay { Date = new DateTime(2020, 2, 1) },
                    new LoggedDay { Date = new DateTime(2020, 2, 2) }
                }
            });

            _context.Trackers.Add(new Tracker
            {
                Name = "T1",
                LoggedDays = new[]
                {
                    new LoggedDay { Date = new DateTime(2020, 1, 1) },
                    new LoggedDay { Date = new DateTime(2020, 1, 2) }
                }
            });

            _context.Trackers.Add(new Tracker
            {
                Name = "T2"
            });

            _context.SaveChanges();
        }

        public void Dispose()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }
    }
}
