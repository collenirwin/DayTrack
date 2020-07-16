using DayTrack.Models;
using DayTrack.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DayTrack.Tests
{
    public class MockTrackerService : ITrackerService
    {
        public Task<Tracker> TryAddTrackerAsync(string name) =>
            Task.FromResult(new Tracker
            {
                Id = 0,
                Name = name
            });

        public Task<bool> TryDeleteTrackerAsync(int id) => Task.FromResult(true);

        public Task<IEnumerable<Tracker>> TryGetAllTrackersAsync() =>
            Task.FromResult(new[]
            {
                new Tracker
                {
                    Id = 0,
                    Name = "1"
                },
                new Tracker
                {
                    Id = 1,
                    Name = "2"
                }
            }.AsEnumerable());

        public Task<Tracker> TryUpdateTrackerNameAsync(int id, string name) =>
            Task.FromResult(new Tracker
            {
                Id = id,
                Name = name
            });

    }
}
