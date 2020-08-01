using DayTrack.Data;
using DayTrack.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.IO;

namespace DayTrack.Tests
{
    public abstract class AppDatabaseTestBase : IDisposable
    {
        protected readonly AppDatabase _database;
        protected readonly SQLiteAsyncConnection _context;

        protected List<Tracker> Trackers { get; private set; }
        protected List<LoggedDay> LoggedDays { get; private set; }

        public AppDatabaseTestBase(string databasePath)
        {
            if (File.Exists(databasePath))
            {
                File.Delete(databasePath);
            }

            _database = new AppDatabase(databasePath);
            _context = _database.Connection;

            Seed();
        }

        private void Seed()
        {
            Trackers = new List<Tracker>
            {
                new Tracker
                {
                    Name = "T0"
                },
                new Tracker
                {
                    Name = "T1"
                },
                new Tracker
                {
                    Name = "T2"
                },
                new Tracker
                {
                    Name = "T3"
                }
            };

            LoggedDays = new List<LoggedDay>
            {
                new LoggedDay { TrackerId = 1, Date = new DateTime(2020, 1, 1) },
                new LoggedDay { TrackerId = 1, Date = new DateTime(2020, 1, 1) },
                new LoggedDay { TrackerId = 1, Date = new DateTime(2020, 2, 1) },
                new LoggedDay { TrackerId = 1, Date = new DateTime(2020, 2, 2) },

                new LoggedDay { TrackerId = 2, Date = new DateTime(2020, 1, 1) },
                new LoggedDay { TrackerId = 2, Date = new DateTime(2020, 1, 2) },

                new LoggedDay { TrackerId = 4, Date = new DateTime(2020, 1, 1, 0, 0, 0) },
                new LoggedDay { TrackerId = 4, Date = new DateTime(2020, 1, 1, 1, 0, 0) }
            };

            _context.InsertAllAsync(Trackers).Wait();
            _context.InsertAllAsync(LoggedDays).Wait();
        }

        public void Dispose() => _database.Dispose();
    }
}
