using DayTrack.Models;
using DayTrack.ViewModels;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xunit;

namespace DayTrack.Tests
{
    public class TrackerViewModelTests
    {
        [Theory]
        [InlineData(null)]
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

        [Theory]
        [InlineData("Collen")]
        [InlineData(" Col len ")]
        public async Task CreateAsync_WithName_HasNoError(string name)
        {
            // arrange
            var vm = new TrackerViewModel(new MockTrackerService())
            {
                Name = name
            };

            // act
            await vm.CreateAsync();

            // assert
            Assert.False(vm.HasError);
        }

        [Fact]
        public async Task CreateAsync_WithName_AddsToAllTrackers()
        {
            // arrange
            string name = "Collen";
            var vm = new TrackerViewModel(new MockTrackerService())
            {
                Name = name
            };

            // act
            await vm.CreateAsync();

            // assert
            Assert.Contains(vm.AllTrackers, tracker => tracker.Name == name);
        }

        [Fact]
        public async Task CreateAsync_WithName_SendsMessageWithTracker()
        {
            // arrange
            string name = "Collen";
            var vm = new TrackerViewModel(new MockTrackerService())
            {
                Name = name
            };

            Tracker createdTracker = null;
            MessagingCenter.Subscribe<TrackerViewModel, Tracker>(this, nameof(vm.CreateCommand),
                (sender, tracker) => createdTracker = tracker);

            // act
            await vm.CreateAsync();

            // assert
            Assert.Equal(name, createdTracker?.Name);
        }

        [Fact]
        public async Task CreateAsync_ServiceCallFailure_HasError()
        {
            // arrange
            string name = "Collen";
            var vm = new TrackerViewModel(new FailingTrackerService())
            {
                Name = name
            };

            // act
            await vm.CreateAsync();

            // assert
            Assert.True(vm.HasError);
        }

        [Theory]
        [InlineData("")]
        [InlineData("Collen")]
        public async Task CreateAsync_AnyError_DoesNotSendMessage(string name)
        {
            // arrange
            var vm = new TrackerViewModel(new FailingTrackerService())
            {
                Name = name
            };

            bool messageSent = false;
            MessagingCenter.Subscribe<TrackerViewModel, Tracker>(this, nameof(vm.CreateCommand),
                (sender, tracker) => messageSent = true);

            // act
            await vm.CreateAsync();

            // assert
            Assert.False(messageSent);
        }
    }
}
