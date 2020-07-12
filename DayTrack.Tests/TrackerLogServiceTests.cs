using DayTrack.Services;
using Serilog;
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
                .WriteTo.Console()
                .CreateLogger();
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
    }
}
