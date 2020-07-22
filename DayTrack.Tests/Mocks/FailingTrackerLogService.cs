using DayTrack.Models;
using DayTrack.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DayTrack.Tests.Mocks
{
    /// <summary>
    /// A mock <see cref="ITrackerLogService"/> implementation in which all Try* methods return their failure value.
    /// </summary>
    public class FailingTrackerLogService : ITrackerLogService
    {
        public Task<bool> TryBulkAddEntriesAsync(IEnumerable<DateTime> days, int trackerId) => Task.FromResult(false);

        public Task<bool> TryDeleteLoggedDayAsync(int id) => Task.FromResult(false);

        public Task<IEnumerable<LoggedDayGroup>> TryGetAllLoggedDayGroupsAsync(int trackerId,
            GroupSortOption sortOption) =>
                Task.FromResult<IEnumerable<LoggedDayGroup>>(null);
        
        public Task<IEnumerable<LoggedDay>> TryGetAllLoggedDaysAsync(int trackerId) =>
            Task.FromResult<IEnumerable<LoggedDay>>(null);

        public Task<bool> TryLogDayAsync(DateTime day, int trackerId) => Task.FromResult(false);
    }
}
