using DayTrack.Data;
using DayTrack.Models;
using DayTrack.Utils;
using Serilog;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DayTrack.Services
{
    /// <summary>
    /// Contains methods for interaction with the <see cref="LoggedDay"/> table.
    /// </summary>
    public class TrackerLogService : ITrackerLogService
    {
        private readonly SQLiteAsyncConnection _context;
        private readonly ILogger _logger;

        public TrackerLogService(AppDatabase database, ILogger logger)
        {
            _context = database.Connection;
            _logger = logger;
        }

        private async Task LogDayAsync(DateTime day, int trackerId) =>
            await _context.InsertAsync(new LoggedDay
            {
                TrackerId = trackerId,
                Date = day
            });

        /// <summary>
        /// Adds a new <see cref="LoggedDay"/> to the database with the given date and under the given tracker.
        /// </summary>
        /// <param name="day">Date to log.</param>
        /// <param name="trackerId">Id of the tracker to log this under.</param>
        /// <returns>true if successful.</returns>
        public async Task<bool> TryLogDayAsync(DateTime day, int trackerId) =>
            await Try.RunAsync(async () => await LogDayAsync(day, trackerId),
                ex => _logger.Error(ex, $"Failed to log day (trackerId: {trackerId}, day: {day})."));

        private async Task DeleteLoggedDayAsync(int id)
        {
            var loggedDay = await _context.Table<LoggedDay>().FirstAsync(day => day.Id == id);
            await _context.DeleteAsync(loggedDay);
        }

        /// <summary>
        /// Deletes the <see cref="LoggedDay"/> with the given id.
        /// </summary>
        /// <param name="id">Id of the logged day to delete.</param>
        /// <returns>true if successful.</returns>
        public async Task<bool> TryDeleteLoggedDayAsync(int id) =>
            await Try.RunAsync(async () => await DeleteLoggedDayAsync(id),
                ex => _logger.Error(ex, $"Failed to delete logged day with id {id}."));

        private async Task<IEnumerable<LoggedDay>> GetAllLoggedDaysAsync(int trackerId) =>
            await _context.Table<LoggedDay>()
                .Where(day => day.TrackerId == trackerId)
                .OrderByDescending(day => day.Date)
                .ToListAsync();

        /// <summary>
        /// Gets all <see cref="LoggedDay"/> with the given tracker id, in descending order by date.
        /// </summary>
        /// <param name="trackerId">Tracker id of the logged days to get.</param>
        /// <returns>All logged days under the given tracker.</returns>
        public async Task<IEnumerable<LoggedDay>> TryGetAllLoggedDaysAsync(int trackerId) =>
            await Try.RunAsync(async () => await GetAllLoggedDaysAsync(trackerId),
                ex => _logger.Error(ex, $"Failed to get all logged days for tracker id {trackerId}."));

        private IEnumerable<LoggedDayGroup> GetAllLoggedDayGroups(IEnumerable<LoggedDay> loggedDays,
            int trackerId, GroupSortOption sortOption)
        {
            loggedDays = loggedDays ?? throw new ArgumentNullException(nameof(loggedDays));

            var groups = loggedDays
                .Where(day => day.TrackerId == trackerId)
                .GroupBy(day => day.Date.Date,
                    (day, group) => new LoggedDayGroup
                    {
                        Date = day,
                        Count = group.Count()
                    });

            groups = sortOption switch
            {
                GroupSortOption.DateDescending => groups.OrderByDescending(group => group.Date),
                GroupSortOption.DateAscending => groups.OrderBy(group => group.Date),
                GroupSortOption.CountDescending => groups.OrderByDescending(group => group.Count),
                _ => throw new NotImplementedException($"Sort option '{sortOption}' not supported.")
            };

            return groups;
        }

        /// <summary>
        /// Gets all <see cref="LoggedDay"/>s as <see cref="LoggedDayGroup"/>s for a given tracker id.
        /// </summary>
        /// <param name="loggedDays">Logged days to group.</param>
        /// <param name="trackerId">Tracker id of the logged days to get.</param>
        /// <param name="sortOption">Specifies the way to sort the results.</param>
        /// <returns>All logged day groups under the given tracker, in the given sort order.</returns>
        public IEnumerable<LoggedDayGroup> TryGetAllLoggedDayGroups(IEnumerable<LoggedDay> loggedDays,
            int trackerId, GroupSortOption sortOption) =>
                Try.Run(() => GetAllLoggedDayGroups(loggedDays, trackerId, sortOption),
                    ex => _logger.Error(ex, $"Failed to get all logged day groups for tracker id {trackerId}."));

        private async Task BulkAddEntriesAsync(IEnumerable<DateTime> days, int trackerId)
        {
            var loggedDays = days.Select(day => new LoggedDay
            {
                TrackerId = trackerId,
                Date = day
            });

            await _context.InsertAllAsync(loggedDays);
        }

        /// <summary>
        /// Adds new <see cref="LoggedDay"/>s to the database with the given dates and under the given tracker.
        /// </summary>
        /// <param name="day">Dates to log.</param>
        /// <param name="trackerId">Id of the tracker to log these under.</param>
        /// <returns>true if successful.</returns>
        public async Task<bool> TryBulkAddEntriesAsync(IEnumerable<DateTime> days, int trackerId) =>
            await Try.RunAsync(async () => await BulkAddEntriesAsync(days, trackerId),
                ex => _logger.Error(ex, $"Bulk insert operation failed (tracker id: {trackerId})."));
    }
}
