using DayTrack.Services;
using Serilog;
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
    }
}
