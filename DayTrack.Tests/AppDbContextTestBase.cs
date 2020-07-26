using DayTrack.Data;
using DayTrack.Models;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace DayTrack.Tests
{
    public abstract class AppDbContextTestBase : IDisposable
    {
        private readonly SqliteConnection _connection;
        protected readonly AppDbContext _context;

        protected int TrackerCount { get; private set; }

        public AppDbContextTestBase()
        {
            _connection = new SqliteConnection("Data source=:memory:");
            _connection.Open();

            _context = new AppDbContext(new DbContextOptionsBuilder<AppDbContext>()
               .UseSqlite(_connection)
               .Options);

            Seed();
        }

        private void Seed()
        {
            _context.Database.EnsureCreated();

            var trackers = new List<Tracker>
            {
                new Tracker
                {
                    Name = "T0",
                    LoggedDays = new List<LoggedDay>
                    {
                        new LoggedDay { Date = new DateTime(2020, 1, 1) },
                        new LoggedDay { Date = new DateTime(2020, 1, 1) },
                        new LoggedDay { Date = new DateTime(2020, 2, 1) },
                        new LoggedDay { Date = new DateTime(2020, 2, 2) }
                    }
                },
                new Tracker
                {
                    Name = "T1",
                    LoggedDays = new List<LoggedDay>
                    {
                        new LoggedDay { Date = new DateTime(2020, 1, 1) },
                        new LoggedDay { Date = new DateTime(2020, 1, 2) }
                    }
                },
                new Tracker
                {
                    Name = "T2"
                },
                new Tracker
                {
                    Name = "T3",
                    LoggedDays = new List<LoggedDay>
                    {
                        new LoggedDay { Date = new DateTime(2020, 1, 1, 0, 0, 0) },
                        new LoggedDay { Date = new DateTime(2020, 1, 1, 1, 0, 0) }
                    }
                }
            };

            TrackerCount = trackers.Count;
            _context.Trackers.AddRange(trackers);
            _context.SaveChanges();
        }

        public void Dispose()
        {
            _context.Dispose();
            _connection.Dispose();
        }
    }
}
