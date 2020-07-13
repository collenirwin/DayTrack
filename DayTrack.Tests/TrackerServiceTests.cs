using DayTrack.Services;
using Newtonsoft.Json;
using Serilog;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace DayTrack.Tests
{
    public class TrackerServiceTests : AppDbContextTestBase
    {
        private readonly ILogger _logger;

        public TrackerServiceTests() : base(nameof(TrackerServiceTests))
        {
            _logger = new LoggerConfiguration()
                .WriteTo.Debug()
                .CreateLogger();
        }

        [Fact]
        public async Task TryAddTrackerAsync_DuplicateName_ReturnsNull()
        {
            // arrange
            var service = new TrackerService(_context, _logger);
            string name = "T0"; // created in seed

            // act
            var tracker = await service.TryAddTrackerAsync(name);

            // assert
            Assert.Null(tracker);
        }

        [Fact]
        public async Task TryAddTrackerAsync_NullName_ReturnsNull()
        {
            // arrange
            var service = new TrackerService(_context, _logger);
            string name = null;

            // act
            var tracker = await service.TryAddTrackerAsync(name);

            // assert
            Assert.Null(tracker);
        }

        [Fact]
        public async Task TryAddTrackerAsync_NameOnly_HasId()
        {
            // arrange
            var service = new TrackerService(_context, _logger);
            string name = "Test";

            // act
            var tracker = await service.TryAddTrackerAsync(name);

            // assert
            Assert.NotEqual(default, tracker.Id);
        }

        [Fact]
        public async Task TryAddTrackerAsync_NameOnly_HasDateCreated()
        {
            // arrange
            var service = new TrackerService(_context, _logger);
            string name = "Test";

            // act
            var tracker = await service.TryAddTrackerAsync(name);

            // assert
            Assert.Equal(DateTime.Now, tracker.Created, precision: TimeSpan.FromSeconds(1));
        }

        [Fact]
        public async Task TryAddTrackerAsync_NameOnly_ExistsInDb()
        {
            // arrange
            var service = new TrackerService(_context, _logger);
            string name = "Test";

            // act
            var tracker = await service.TryAddTrackerAsync(name);
            var fetched = _context.Trackers.FirstOrDefault(t => t.Id == tracker.Id);
            string expected = JsonConvert.SerializeObject(tracker);
            string actual = JsonConvert.SerializeObject(fetched);

            // assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task TryDeleteTrackerAsync_NewId_ReturnsFalse()
        {
            // arrange
            var service = new TrackerService(_context, _logger);
            int id = 100;

            // act
            bool successful = await service.TryDeleteTrackerAsync(id);

            // assert
            Assert.False(successful);
        }

        [Fact]
        public async Task TryDeleteTrackerAsync_ExistingId_ReturnsTrue()
        {
            // arrange
            var service = new TrackerService(_context, _logger);
            int id = 1;

            // act
            bool successful = await service.TryDeleteTrackerAsync(id);

            // assert
            Assert.True(successful);
        }

        [Fact]
        public async Task TryDeleteTrackerAsync_ExistingId_DeletesTracker()
        {
            // arrange
            var service = new TrackerService(_context, _logger);
            int id = 1;

            // act
            await service.TryDeleteTrackerAsync(id);
            var tracker = _context.Trackers.FirstOrDefault(t => t.Id == id);

            // assert
            Assert.Null(tracker);
        }

        [Fact]
        public async Task TryDeleteTrackerAsync_ExistingId_DeletesLoggedDays()
        {
            // arrange
            var service = new TrackerService(_context, _logger);
            int id = 1;

            // act
            await service.TryDeleteTrackerAsync(id);
            var days = _context.LoggedDays.Where(day => day.TrackerId == id);

            // assert
            Assert.Empty(days);
        }

        [Fact]
        public async Task TryUpdateTrackerNameAsync_NullName_ReturnsNull()
        {
            // arrange
            var service = new TrackerService(_context, _logger);
            int id = 1;
            string name = null;

            // act
            var tracker = await service.TryUpdateTrackerNameAsync(id, name);

            // assert
            Assert.Null(tracker);
        }

        [Fact]
        public async Task TryUpdateTrackerNameAsync_NullName_DoesNotUpdateTracker()
        {
            // arrange
            var service = new TrackerService(_context, _logger);
            int id = 1;
            string name = null;

            // act
            await service.TryUpdateTrackerNameAsync(id, name);
            var tracker = _context.Trackers.First(t => t.Id == id);

            // assert
            Assert.NotNull(tracker.Name);
        }

        [Fact]
        public async Task TryUpdateTrackerNameAsync_NewName_ReturnsTracker()
        {
            // arrange
            var service = new TrackerService(_context, _logger);
            int id = 1;
            string name = "Anne";

            // act
            var tracker = await service.TryUpdateTrackerNameAsync(id, name);

            // assert
            Assert.NotNull(tracker);
        }

        [Fact]
        public async Task TryUpdateTrackerNameAsync_NewName_UpdatesTracker()
        {
            // arrange
            var service = new TrackerService(_context, _logger);
            int id = 1;
            string name = "Anne";

            // act
            await service.TryUpdateTrackerNameAsync(id, name);
            var tracker = _context.Trackers.First(t => t.Id == id);

            // assert
            Assert.Equal(name, tracker.Name);
        }

        [Fact]
        public async Task TryUpdateTrackerNameAsync_SameName_ReturnsTracker()
        {
            // arrange
            var service = new TrackerService(_context, _logger);
            int id = 1;
            string name = "T0";

            // act
            var tracker = await service.TryUpdateTrackerNameAsync(id, name);

            // assert
            Assert.NotNull(tracker);
        }

        [Fact]
        public async Task TryGetAllTrackersAsync_ReturnsAllTrackers()
        {
            // arrange
            var service = new TrackerService(_context, _logger);

            // act
            var trackers = await service.TryGetAllTrackersAsync();

            // assert
            Assert.Equal(4, trackers.Count());
        }
    }
}
