using DayTrack.Models;
using DayTrack.Services;
using Newtonsoft.Json;
using Serilog;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace DayTrack.Tests
{
    /// <summary>
    /// Tests for <see cref="TrackerService"/> methods.
    /// </summary>
    public class TrackerServiceTests : AppDatabaseTestBase
    {
        private readonly ILogger _logger;

        public TrackerServiceTests() : base(databasePath: $"{nameof(TrackerServiceTests)}.db")
        {
            _logger = new LoggerConfiguration()
                .WriteTo.Debug()
                .CreateLogger();
        }

        #region TryAddTrackerAsync

        [Fact]
        public async Task TryAddTrackerAsync_DuplicateName_ReturnsNull()
        {
            // arrange
            var service = new TrackerService(_database, _logger);
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
            var service = new TrackerService(_database, _logger);
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
            var service = new TrackerService(_database, _logger);
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
            var service = new TrackerService(_database, _logger);
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
            var service = new TrackerService(_database, _logger);
            string name = "Test";

            // act
            var expected = await service.TryAddTrackerAsync(name);
            var actual = await _context.Table<Tracker>().FirstOrDefaultAsync(t => t.Id == expected.Id);

            // assert
            Assert.Equal(expected.Id, actual.Id);
            Assert.Equal(expected.Name, actual.Name);
        }

        #endregion

        #region TryDeleteTrackerAsync

        [Fact]
        public async Task TryDeleteTrackerAsync_NewId_ReturnsFalse()
        {
            // arrange
            var service = new TrackerService(_database, _logger);
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
            var service = new TrackerService(_database, _logger);
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
            var service = new TrackerService(_database, _logger);
            int id = 1;

            // act
            await service.TryDeleteTrackerAsync(id);
            var tracker = await _context.Table<Tracker>().FirstOrDefaultAsync(t => t.Id == id);

            // assert
            Assert.Null(tracker);
        }

        [Fact]
        public async Task TryDeleteTrackerAsync_ExistingId_DeletesLoggedDays()
        {
            // arrange
            var service = new TrackerService(_database, _logger);
            int id = 1;

            // act
            await service.TryDeleteTrackerAsync(id);
            var days = await _context.Table<LoggedDay>().Where(day => day.TrackerId == id).ToListAsync();

            // assert
            Assert.Empty(days);
        }

        #endregion

        #region TryUpdateTrackerNameAsync

        [Fact]
        public async Task TryUpdateTrackerNameAsync_NullName_ReturnsNull()
        {
            // arrange
            var service = new TrackerService(_database, _logger);
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
            var service = new TrackerService(_database, _logger);
            int id = 1;
            string name = null;

            // act
            await service.TryUpdateTrackerNameAsync(id, name);
            var tracker = await _context.Table<Tracker>().FirstAsync(t => t.Id == id);

            // assert
            Assert.NotNull(tracker.Name);
        }

        [Fact]
        public async Task TryUpdateTrackerNameAsync_NewName_ReturnsTracker()
        {
            // arrange
            var service = new TrackerService(_database, _logger);
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
            var service = new TrackerService(_database, _logger);
            int id = 1;
            string name = "Anne";

            // act
            await service.TryUpdateTrackerNameAsync(id, name);
            var tracker = await _context.Table<Tracker>().FirstAsync(t => t.Id == id);

            // assert
            Assert.Equal(name, tracker.Name);
        }

        [Fact]
        public async Task TryUpdateTrackerNameAsync_SameName_ReturnsTracker()
        {
            // arrange
            var service = new TrackerService(_database, _logger);
            int id = 1;
            string name = "T0";

            // act
            var tracker = await service.TryUpdateTrackerNameAsync(id, name);

            // assert
            Assert.NotNull(tracker);
        }

        #endregion

        #region TryGetAllTrackersAsync

        [Fact]
        public async Task TryGetAllTrackersAsync_ReturnsAllTrackers()
        {
            // arrange
            var service = new TrackerService(_database, _logger);

            // act
            var trackers = await service.TryGetAllTrackersAsync();

            // assert
            Assert.Equal(Trackers.Count, trackers.Count());
        }

        #endregion

        #region TryGetRecentTrackersAsync

        [Fact]
        public async Task TryGetRecentTrackersAsync_2_Gets2MostRecentTrackers()
        {
            // arrange
            var service = new TrackerService(_database, _logger);

            // act
            var trackers = await service.TryGetRecentTrackersAsync(count: 2);

            // assert
            Assert.Equal(2, trackers.Count());
            Assert.NotNull(trackers.First());
            Assert.NotNull(trackers.Last());
            Assert.Equal(1, trackers.First().Id);
            Assert.Equal(2, trackers.Last().Id);
        }

        [Fact]
        public async Task TryGetRecentTrackersAsync_1_GetsMostRecentTracker()
        {
            // arrange
            var service = new TrackerService(_database, _logger);

            // act
            var trackers = await service.TryGetRecentTrackersAsync(count: 1);

            // assert
            Assert.Single(trackers);
            Assert.NotNull(trackers.First());
            Assert.Equal(1, trackers.First().Id);
        }

        [Fact]
        public async Task TryGetRecentTrackersAsync_0_GetsEmptyResultSet()
        {
            // arrange
            var service = new TrackerService(_database, _logger);

            // act
            var trackers = await service.TryGetRecentTrackersAsync(count: 0);

            // assert
            Assert.Empty(trackers);
        }

        [Fact]
        public async Task TryGetRecentTrackersAsync_Negative_ReturnsNull()
        {
            // arrange
            var service = new TrackerService(_database, _logger);

            // act
            var trackers = await service.TryGetRecentTrackersAsync(count: -1);

            // assert
            Assert.Null(trackers);
        }

        #endregion
    }
}
