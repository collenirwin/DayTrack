using DayTrack.Data;
using DayTrack.Models;
using DayTrack.Utils;
using EFCore.BulkExtensions;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DayTrack.Services
{
    public class TrackerLogService
    {
        public enum GroupSortOption
        {
            DateDescending,
            DateAscending,
            CountDescending
        }

        private readonly AppDbContext _context;
        private readonly ILogger _logger;

        public TrackerLogService(AppDbContext context, ILogger logger)
        {
            _context = context;
            _logger = logger;
        }

        private async Task LogDayAsync(int trackerId, DateTime day)
        {
            _context.LoggedDays.Add(new LoggedDay
            {
                TrackerId = trackerId,
                Date = day
            });

            await _context.SaveChangesAsync();
        }

        public async Task<bool> TryLogDayAsync(int trackerId, DateTime day) =>
            await Try.RunAsync(async () => await LogDayAsync(trackerId, day),
                ex => _logger.Error(ex, $"Failed to log day (trackerId: {trackerId}, day: {day})."));

        private async Task DeleteLoggedDayAsync(int id)
        {
            var loggedDay = await _context.LoggedDays.FirstAsync(day => day.Id == id);
            _context.LoggedDays.Remove(loggedDay);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> TryDeleteLoggedDayAsync(int id) =>
            await Try.RunAsync(async () => await DeleteLoggedDayAsync(id),
                ex => _logger.Error(ex, $"Failed to delete logged day with id {id}."));

        private async Task<IEnumerable<LoggedDay>> GetAllLoggedDaysAsync(int trackerId) =>
            await _context.LoggedDays
                .Where(day => day.TrackerId == trackerId)
                .OrderByDescending(day => day.Date)
                .ToListAsync();

        public async Task<IEnumerable<LoggedDay>> TryGetAllLoggedDaysAsync(int trackerId) =>
            await Try.RunAsync(async () => await GetAllLoggedDaysAsync(trackerId),
                ex => _logger.Error(ex, $"Failed to get all logged days for tracker id {trackerId}."));

        private async Task<IEnumerable<LoggedDayGroup>> GetAllLoggedDayGroupsAsync(int trackerId,
            GroupSortOption sortOption)
        {
            var query = _context.LoggedDays
                .Where(day => day.TrackerId == trackerId)
                .GroupBy(day => day.Date,
                    (day, group) => new LoggedDayGroup
                    {
                        Date = day,
                        Count = group.Count()
                    });

            switch (sortOption)
            {
                case GroupSortOption.DateDescending:
                    query = query.OrderByDescending(group => group.Date);
                    break;
                case GroupSortOption.DateAscending:
                    query = query.OrderBy(group => group.Date);
                    break;
                case GroupSortOption.CountDescending:
                    query = query.OrderByDescending(group => group.Count);
                    break;
            }

            return await query.ToListAsync();
        }

        public async Task<IEnumerable<LoggedDayGroup>> TryGetAllLoggedDayGroupsAsync(int trackerId,
            GroupSortOption sortOption) =>
                await Try.RunAsync(async () => await GetAllLoggedDayGroupsAsync(trackerId, sortOption),
                    ex => _logger.Error(ex, $"Failed to get all logged day groups for tracker id {trackerId}."));

        private async Task BulkAddEntriesAsync(IEnumerable<DateTime> days, int trackerId)
        {
            var loggedDays = days.Select(day => new LoggedDay
            {
                TrackerId = trackerId,
                Date = day
            }).ToList();

            await _context.BulkInsertAsync(loggedDays);
        }

        public async Task<bool> TryBulkAddEntriesAsync(IEnumerable<DateTime> days, int trackerId) =>
            await Try.RunAsync(async () => await BulkAddEntriesAsync(days, trackerId),
                ex => _logger.Error(ex, $"Bulk insert operation failed (tracker id: {trackerId})."));
    }
}
