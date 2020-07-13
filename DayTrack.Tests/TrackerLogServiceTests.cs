using DayTrack.Services;
using Serilog;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace DayTrack.Tests
{
    public class TrackerLogServiceTests : AppDbContextTestBase
    {
        private readonly ILogger _logger;

        public TrackerLogServiceTests() : base(nameof(TrackerLogServiceTests))
        {
            _logger = new LoggerConfiguration()
                .WriteTo.Debug()
                .CreateLogger();
        }

        [Fact]
        public async Task TryLogDayAsync_NewTrackerId_ReturnsFalse()
        {
            // arrange
            var service = new TrackerLogService(_context, _logger);
            int trackerId = 100;
            var day = DateTime.Now.Date;

            // act
            bool successful = await service.TryLogDayAsync(day, trackerId);

            // assert
            Assert.False(successful);
        }

        [Fact]
        public async Task TryLogDayAsync_DuplicateDate_ReturnsTrue()
        {
            // arrange
            var service = new TrackerLogService(_context, _logger);
            int trackerId = 1;
            var day = new DateTime(2020, 1, 1);

            // act
            bool successful = await service.TryLogDayAsync(day, trackerId);

            // assert
            Assert.True(successful);
        }

        [Fact]
        public async Task TryLogDayAsync_ExistsInDb()
        {
            // arrange
            var service = new TrackerLogService(_context, _logger);
            int trackerId = 1;
            var day = DateTime.Now.Date;

            // act
            await service.TryLogDayAsync(day, trackerId);
            var loggedDay = _context.LoggedDays.FirstOrDefault(d => d.TrackerId == trackerId && d.Date == day);

            // assert
            Assert.NotNull(loggedDay);
        }

        [Fact]
        public async Task TryDeleteLoggedDayAsync_NewId_ReturnsFalse()
        {
            // arrange
            var service = new TrackerLogService(_context, _logger);
            int id = 100;

            // act
            bool successful = await service.TryDeleteLoggedDayAsync(id);

            // assert
            Assert.False(successful);
        }

        [Fact]
        public async Task TryDeleteLoggedDayAsync_ExistingId_ReturnsTrue()
        {
            // arrange
            var service = new TrackerLogService(_context, _logger);
            int id = 1;

            // act
            bool successful = await service.TryDeleteLoggedDayAsync(id);

            // assert
            Assert.True(successful);
        }

        [Fact]
        public async Task TryDeleteLoggedDayAsync_ExistingId_DeletesLoggedDay()
        {
            // arrange
            var service = new TrackerLogService(_context, _logger);
            int id = 1;

            // act
            await service.TryDeleteLoggedDayAsync(id);
            var loggedDay = _context.LoggedDays.FirstOrDefault(day => day.Id == id);

            // assert
            Assert.Null(loggedDay);
        }

        [Fact]
        public async Task TryDeleteLoggedDayAsync_ExistingId_DoesNotDeleteTracker()
        {
            // arrange
            var service = new TrackerLogService(_context, _logger);
            int id = 1;

            // act
            var trackerId = _context.LoggedDays.First(day => day.Id == id).TrackerId;
            await service.TryDeleteLoggedDayAsync(id);
            var tracker = _context.Trackers.FirstOrDefault(t => t.Id == trackerId);

            // assert
            Assert.NotNull(tracker);
        }

        [Fact]
        public async Task TryGetAllLoggedDayGroupsAsync_DateDescending_IsInDateDescendingOrder()
        {
            // arrange
            var service = new TrackerLogService(_context, _logger);
            int trackerId = 1;

            // act
            var actual = await service
                .TryGetAllLoggedDayGroupsAsync(trackerId, TrackerLogService.GroupSortOption.DateDescending);
            var expected = actual.OrderByDescending(group => group.Date);

            // assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task TryGetAllLoggedDayGroupsAsync_DateAscending_IsInDateAscendingOrder()
        {
            // arrange
            var service = new TrackerLogService(_context, _logger);
            int trackerId = 1;

            // act
            var actual = await service
                .TryGetAllLoggedDayGroupsAsync(trackerId, TrackerLogService.GroupSortOption.DateAscending);
            var expected = actual.OrderBy(group => group.Date);

            // assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task TryGetAllLoggedDayGroupsAsync_CountDescending_IsInCountDescendingOrder()
        {
            // arrange
            var service = new TrackerLogService(_context, _logger);
            int trackerId = 1;

            // act
            var actual = await service
                .TryGetAllLoggedDayGroupsAsync(trackerId, TrackerLogService.GroupSortOption.CountDescending);
            var expected = actual.OrderByDescending(group => group.Count);

            // assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task TryGetAllLoggedDayGroupsAsync_GroupsByDay()
        {
            // arrange
            var service = new TrackerLogService(_context, _logger);
            int trackerId = 1;

            // act
            var groups = await service
                .TryGetAllLoggedDayGroupsAsync(trackerId, TrackerLogService.GroupSortOption.DateDescending);
            var duplicateDate = groups.First(group => group.Date == new DateTime(2020, 1, 1));

            // assert
            Assert.Equal(2, duplicateDate.Count);
            Assert.Equal(3, groups.Count()); // 3 items in the list overall because the duplicates should count as 1
        }

        [Fact]
        public async Task TryGetAllLoggedDayGroupsAsync_NewTrackerId_ReturnsEmpty()
        {
            // arrange
            var service = new TrackerLogService(_context, _logger);
            int trackerId = 100;

            // act
            var groups = await service
                .TryGetAllLoggedDayGroupsAsync(trackerId, TrackerLogService.GroupSortOption.DateDescending);

            // assert
            Assert.Empty(groups);
        }

        [Fact]
        public async Task TryGetAllLoggedDaysAsync_NewTrackerId_ReturnsEmpty()
        {
            // arrange
            var service = new TrackerLogService(_context, _logger);
            int trackerId = 100;

            // act
            var loggedDays = await service.TryGetAllLoggedDaysAsync(trackerId);

            // assert
            Assert.Empty(loggedDays);
        }

        [Fact]
        public async Task TryGetAllLoggedDaysAsync_ReturnsAllLoggedDays()
        {
            // arrange
            var service = new TrackerLogService(_context, _logger);
            int trackerId = 1;

            // act
            var loggedDays = await service.TryGetAllLoggedDaysAsync(trackerId);

            // assert
            Assert.Equal(4, loggedDays.Count());
        }
    }
}
