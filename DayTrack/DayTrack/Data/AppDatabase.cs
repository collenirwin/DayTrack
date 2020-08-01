using DayTrack.Models;
using SQLite;
using System;

namespace DayTrack.Data
{
    /// <summary>
    /// Exposes a connection to the application's local database.
    /// </summary>
    public class AppDatabase : IDisposable
    {
        public SQLiteAsyncConnection Connection { get; }

        public AppDatabase(string databasePath)
        {
            Connection = new SQLiteAsyncConnection(databasePath);
            Connection.CreateTableAsync<Tracker>().Wait();
            Connection.CreateTableAsync<LoggedDay>().Wait();
        }

        public void Dispose() => Connection?.CloseAsync().Wait();
    }
}
