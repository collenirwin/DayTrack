using DayTrack.Models;
using DayTrack.Services;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DayTrack.Tests.Mocks
{
    /// <summary>
    /// A mock <see cref="ITrackerService"/> implementation backed by a list of trackers.
    /// </summary>
    public class MockTrackerService : ITrackerService
    {
        private int _idCount = 1;

        public List<Tracker> Trackers { get; } = new List<Tracker>();

        public Task<Tracker> TryAddTrackerAsync(string name)
        {
            var tracker = new Tracker
            {
                Id = _idCount++,
                Name = name
            };

            Trackers.Add(tracker);
            return Task.FromResult(tracker);
        }
        
        public Task<bool> TryDeleteTrackerAsync(int id)
        {
            var tracker = Trackers.FirstOrDefault(t => t.Id == id);

            if (tracker is null)
            {
                return Task.FromResult(false);
            }

            Trackers.Remove(tracker);
            return Task.FromResult(true);
        }

        public Task<IEnumerable<Tracker>> TryGetAllTrackersAsync() => Task.FromResult(Trackers.AsEnumerable());

        public Task<Tracker> TryUpdateTrackerNameAsync(int id, string name)
        {
            var tracker = Trackers.FirstOrDefault(t => t.Id == id);

            if (tracker is null)
            {
                return null;
            }

            tracker.Name = name;
            return Task.FromResult(tracker);
        }
    }
}
