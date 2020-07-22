using DayTrack.Models;
using DayTrack.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DayTrack.Tests.Mocks
{
    /// <summary>
    /// A mock <see cref="ITrackerService"/> implementation in which all Try* methods return their failure value.
    /// </summary>
    public class FailingTrackerService : ITrackerService
    {
        public Task<Tracker> TryAddTrackerAsync(string name) => Task.FromResult<Tracker>(null);

        public Task<bool> TryDeleteTrackerAsync(int id) => Task.FromResult(false);

        public Task<IEnumerable<Tracker>> TryGetAllTrackersAsync() => Task.FromResult<IEnumerable<Tracker>>(null);

        public Task<Tracker> TryUpdateTrackerNameAsync(int id, string name) => Task.FromResult<Tracker>(null);
    }
}
