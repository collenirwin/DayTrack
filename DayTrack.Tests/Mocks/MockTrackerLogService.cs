using DayTrack.Models;
using DayTrack.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DayTrack.Tests.Mocks
{
    public class MockTrackerLogService : ITrackerLogService
    {
        private int _idCount = 1;

        public List<LoggedDay> LoggedDays { get; } = new List<LoggedDay>();
        public GroupSortOption SortOption { get; private set; }

        public Task<bool> TryBulkAddEntriesAsync(IEnumerable<DateTime> days, int trackerId)
        {
            LoggedDays.AddRange(days.Select(day => new LoggedDay
            {
                Id = _idCount++,
                Date = day,
                TrackerId = trackerId
            }));

            return Task.FromResult(true);
        }

        public Task<bool> TryDeleteLoggedDayAsync(int id)
        {
            var day = LoggedDays.FirstOrDefault(d => d.Id == id);

            if (day is null)
            {
                return Task.FromResult(false);
            }

            LoggedDays.Remove(day);
            return Task.FromResult(true);
        }

        public Task<IEnumerable<LoggedDayGroup>> TryGetAllLoggedDayGroupsAsync(int trackerId,
            GroupSortOption sortOption)
        {
            SortOption = sortOption;
            return Task.FromResult(LoggedDays.Where(day => day.TrackerId == trackerId)
                .GroupBy(day => day.Date.Date,
                    (day, group) => new LoggedDayGroup
                    {
                        Date = day,
                        Count = group.Count()
                    }));
        }

        public Task<IEnumerable<LoggedDay>> TryGetAllLoggedDaysAsync(int trackerId) =>
            Task.FromResult(LoggedDays.OrderByDescending(day => day.Date).AsEnumerable());

        public Task<bool> TryLogDayAsync(DateTime day, int trackerId)
        {
            LoggedDays.Add(new LoggedDay
            {
                Id = _idCount++,
                Date = day,
                TrackerId = trackerId
            });

            return Task.FromResult(true);
        }
    }
}
