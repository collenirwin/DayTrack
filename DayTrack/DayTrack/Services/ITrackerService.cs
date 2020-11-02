using DayTrack.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DayTrack.Services
{
    public interface ITrackerService
    {
        Task<Tracker> TryAddTrackerAsync(string name);
        Task<Tracker> TryUpdateTrackerNameAsync(int id, string name);
        Task<IEnumerable<Tracker>> TryGetAllTrackersAsync();
        Task<IEnumerable<Tracker>> TryGetRecentTrackersAsync(int count);
        Task<bool> TryDeleteTrackerAsync(int id);
    }
}
