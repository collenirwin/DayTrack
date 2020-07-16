using DayTrack.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DayTrack.Services
{
    public interface ITrackerLogService
    {
        Task<bool> TryLogDayAsync(DateTime day, int trackerId);
        Task<bool> TryDeleteLoggedDayAsync(int id);
        Task<IEnumerable<LoggedDay>> TryGetAllLoggedDaysAsync(int trackerId);
        Task<IEnumerable<LoggedDayGroup>> TryGetAllLoggedDayGroupsAsync(int trackerId, GroupSortOption sortOption);
        Task<bool> TryBulkAddEntriesAsync(IEnumerable<DateTime> days, int trackerId);
    }

    /// <summary>
    /// Available sort options for
    /// <see cref="ITrackerLogService.TryGetAllLoggedDayGroupsAsync(int, GroupSortOption)"/> results.
    /// </summary>
    public enum GroupSortOption
    {
        DateDescending,
        DateAscending,
        CountDescending
    }
}
