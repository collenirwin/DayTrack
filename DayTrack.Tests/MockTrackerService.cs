using DayTrack.Models;
using DayTrack.Services;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DayTrack.Tests
{
    public class MockTrackerService : ITrackerService
    {
        private readonly List<Tracker> _trackers = new List<Tracker>();
        private int _idCount = 1;

        public Task<Tracker> TryAddTrackerAsync(string name)
        {
            var tracker = new Tracker
            {
                Id = _idCount++,
                Name = name
            };

            _trackers.Add(tracker);
            return Task.FromResult(tracker);
        }


        public Task<bool> TryDeleteTrackerAsync(int id)
        {
            var tracker = _trackers.FirstOrDefault(t => t.Id == id);

            if (tracker is null)
            {
                return Task.FromResult(false);
            }

            _trackers.Remove(tracker);
            return Task.FromResult(true);
        }

        public Task<IEnumerable<Tracker>> TryGetAllTrackersAsync() => Task.FromResult(_trackers.AsEnumerable());

        public Task<Tracker> TryUpdateTrackerNameAsync(int id, string name)
        {
            var tracker = _trackers.FirstOrDefault(t => t.Id == id);

            if (tracker is null)
            {
                return null;
            }

            tracker.Name = name;
            return Task.FromResult(tracker);
        }
    }
}
