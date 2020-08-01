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
        IEnumerable<LoggedDayGroup> TryGetAllLoggedDayGroups(IEnumerable<LoggedDay> loggedDays,
            int trackerId, GroupSortOption sortOption);
        Task<bool> TryBulkAddEntriesAsync(IEnumerable<DateTime> days, int trackerId);
    }

    /// <summary>
    /// Available sort options for <see cref="ITrackerLogService.TryGetAllLoggedDayGroups"/> results.
    /// </summary>
    public enum GroupSortOption
    {
        DateDescending,
        DateAscending,
        CountDescending
    }
}
