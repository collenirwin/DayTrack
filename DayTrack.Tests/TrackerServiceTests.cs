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

        public TrackerServiceTests()
        {
            _logger = new LoggerConfiguration()
                .WriteTo.Console()
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
    }
}
