using DayTrack.Models;
using SQLite;

namespace DayTrack.Data
{
    /// <summary>
    /// Exposes a connection to the application's local database.
    /// </summary>
    public class AppDatabase
    {
        public SQLiteAsyncConnection Connection { get; }

        public AppDatabase(string databasePath)
        {
            Connection = new SQLiteAsyncConnection(databasePath);
            Connection.CreateTableAsync<Tracker>().Wait();
            Connection.CreateTableAsync<LoggedDay>().Wait();
        }
    }
}
