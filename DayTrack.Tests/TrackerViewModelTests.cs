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

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        [InlineData("\t")]
        public async Task UpdateAsync_NoName_HasError(string name)
        {
            // arrange
            var vm = new TrackerViewModel(new MockTrackerService())
            {
                Id = 0,
                Name = name
            };

            // act
            await vm.UpdateAsync();

            // assert
            Assert.True(vm.HasError);
        }

        [Theory]
        [InlineData("Collen")]
        [InlineData(" Col len ")]
        public async Task UpdateAsync_WithName_HasNoError(string name)
        {
            // arrange
            var service = new MockTrackerService();
            service.Trackers.Add(new Tracker { Id = 0, Name = "Bob" });

            var vm = new TrackerViewModel(service)
            {
                Id = 0,
                Name = name
            };

            // act
            await vm.UpdateAsync();

            // assert
            Assert.False(vm.HasError);
        }

        [Fact]
        public async Task UpdateAsync_WithName_UpdatesTracker()
        {
            // arrange
            int id = 0;
            string name = "Collen";

            var service = new MockTrackerService();
            service.Trackers.Add(new Tracker { Id = 0, Name = "Bob" });

            var vm = new TrackerViewModel(service)
            {
                Id = id,
                Name = name
            };

            // act
            await vm.UpdateAsync();

            // assert
            Assert.Single(vm.AllTrackers);
            Assert.Contains(vm.AllTrackers, tracker => tracker.Name == name && tracker.Id == id);
        }

        [Fact]
        public async Task UpdateAsync_WithName_SendsMessageWithTracker()
        {
            // arrange
            int id = 0;
            string name = "Collen";
            
            var service = new MockTrackerService();
            service.Trackers.Add(new Tracker { Id = 0, Name = "Bob" });

            var vm = new TrackerViewModel(service)
            {
                Id = 0,
                Name = name
            };

            Tracker updatedTracker = null;
            MessagingCenter.Subscribe<TrackerViewModel, Tracker>(this, nameof(vm.UpdateCommand),
                (sender, tracker) => updatedTracker = tracker);

            // act
            await vm.UpdateAsync();

            // assert
            Assert.Equal(id, updatedTracker?.Id);
            Assert.Equal(name, updatedTracker?.Name);
        }

        [Fact]
        public async Task UpdateAsync_ServiceCallFailure_HasError()
        {
            // arrange
            string name = "Collen";
            var vm = new TrackerViewModel(new FailingTrackerService())
            {
                Id = 0,
                Name = name
            };

            // act
            await vm.UpdateAsync();

            // assert
            Assert.True(vm.HasError);
        }

        [Theory]
        [InlineData("")]
        [InlineData("Collen")]
        public async Task UpdateAsync_AnyError_DoesNotSendMessage(string name)
        {
            // arrange
            var vm = new TrackerViewModel(new FailingTrackerService())
            {
                Id = 0,
                Name = name
            };

            bool messageSent = false;
            MessagingCenter.Subscribe<TrackerViewModel, Tracker>(this, nameof(vm.UpdateCommand),
                (sender, tracker) => messageSent = true);

            // act
            await vm.UpdateAsync();

            // assert
            Assert.False(messageSent);
        }

        [Fact]
        public async Task PopulateAllTrackersAsync_ServiceCallFailure_SendsMessage()
        {
            // arrange
            var vm = new TrackerViewModel(new FailingTrackerService());

            bool messageSent = false;
            MessagingCenter.Subscribe<TrackerViewModel>(this, TrackerViewModel.AllTrackersPullFailedMessage,
                sender => messageSent = true);

            // act
            await vm.PopulateAllTrackersAsync();

            // assert
            Assert.True(messageSent);
        }

        [Fact]
        public async Task PopulateAllTrackersAsync_ServiceCallFailure_DoesNotClearAllTrackers()
        {
            // arrange
            var vm = new TrackerViewModel(new FailingTrackerService());
            var tracker = new Tracker { Id = 0, Name = "Collen" };
            vm.AllTrackers.Add(tracker);

            // act
            await vm.PopulateAllTrackersAsync();

            // assert
            Assert.Single(vm.AllTrackers, tracker);
        }
    }
}
