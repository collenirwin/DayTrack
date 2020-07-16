using DayTrack.ViewModels;
using System.Threading.Tasks;
using Xunit;

namespace DayTrack.Tests
{
    public class TrackerViewModelTests
    {
        [Theory]
        [InlineData("")]
        [InlineData("   ")]
        [InlineData("\t")]
        public async Task CreateAsync_NoName_HasError(string name)
        {
            // arrange
            var vm = new TrackerViewModel(new MockTrackerService())
            {
                Name = name
            };

            // act
            await vm.CreateAsync();

            // assert
            Assert.True(vm.HasError);
        }
    }
}
