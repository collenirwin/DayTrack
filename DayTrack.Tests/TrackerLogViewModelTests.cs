using DayTrack.Models;
using DayTrack.Tests.Mocks;
using DayTrack.ViewModels;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xunit;

namespace DayTrack.Tests
{
    /// <summary>
    /// Tests for <see cref="TrackerLogViewModel"/> methods.
    /// </summary>
    public class TrackerLogViewModelTests
    {
        #region LogDayAsync

        [Fact]
        public async Task LogDayAsync_AddsToAllDayGroups()
        {
            // arrange
            var tracker = new Tracker { Id = 0 };
            var vm = new TrackerLogViewModel(tracker, new MockTrackerLogService());

            // act
            await vm.LogDayAsync();

            // assert
            Assert.Single(vm.AllDayGroups, group => group.Date == vm.DateToLog);
        }

        [Fact]
        public async Task LogDayAsync_ServiceFailure_SendsMessage()
        {
            // arrange
            var tracker = new Tracker();
            var vm = new TrackerLogViewModel(tracker, new FailingTrackerLogService());

            bool messageSent = false;
            MessagingCenter.Subscribe<TrackerLogViewModel>(this, TrackerLogViewModel.DatabaseErrorMessage,
                sender => messageSent = true);

            // act
            await vm.LogDayAsync();

            // assert
            Assert.True(messageSent);
        }

        #endregion

        #region DeleteLoggedDayAsync

        [Fact]
        public async Task DeleteLoggedDayAsync_ExistingDay_DeletesDay()
        {
            // arrange
            var tracker = new Tracker { Id = 0 };
            var day = new LoggedDay { Id = 0, TrackerId = 0 };
            var service = new MockTrackerLogService();
            var vm = new TrackerLogViewModel(tracker, service);
            service.LoggedDays.Add(day);
            vm.AllDays.Add(day);

            // act
            await vm.DeleteLoggedDayAsync(day);

            // assert
            Assert.Empty(vm.AllDays);
            Assert.Empty(service.LoggedDays);
        }

        [Fact]
        public async Task DeleteLoggedDayAsync_NullDay_DoesNothing()
        {
            // arrange
            var tracker = new Tracker { Id = 0 };
            var vm = new TrackerLogViewModel(tracker, new MockTrackerLogService());
            vm.AllDays.Add(null);

            // act
            await vm.DeleteLoggedDayAsync(null);

            // assert
            Assert.Single(vm.AllDays, expected: null);
        }

        [Fact]
        public async Task DeleteLoggedDayAsync_NewDay_DoesNothing()
        {
            // arrange
            var tracker = new Tracker { Id = 0 };
            var newDay = new LoggedDay { Id = 0, TrackerId = 0 };
            var existingDay = new LoggedDay { Id = 5, TrackerId = 0 };
            var vm = new TrackerLogViewModel(tracker, new MockTrackerLogService());
            vm.AllDays.Add(existingDay);

            // act
            await vm.DeleteLoggedDayAsync(newDay);

            // assert
            Assert.Single(vm.AllDays, expected: existingDay);
        }

        [Fact]
        public async Task DeleteLoggedDayAsync_ServiceFailure_DoesNotDeleteFromAllDays()
        {
            // arrange
            var tracker = new Tracker { Id = 0 };
            var day = new LoggedDay { Id = 0, TrackerId = 0 };
            var vm = new TrackerLogViewModel(tracker, new FailingTrackerLogService());
            vm.AllDays.Add(day);

            // act
            await vm.DeleteLoggedDayAsync(day);

            // assert
            Assert.Single(vm.AllDays, expected: day);
        }

        [Fact]
        public async Task DeleteLoggedDayAsync_ServiceFailure_SendsMessageWithDay()
        {
            // arrange
            var tracker = new Tracker { Id = 0 };
            var day = new LoggedDay { Id = 0, TrackerId = 0 };
            var vm = new TrackerLogViewModel(tracker, new FailingTrackerLogService());
            vm.AllDays.Add(day);

            LoggedDay sentDay = null;
            MessagingCenter.Subscribe<TrackerLogViewModel, LoggedDay>(this, nameof(vm.DeleteLoggedDayCommand),
               (sender, loggedDay) => sentDay = loggedDay);

            // act
            await vm.DeleteLoggedDayAsync(day);

            // assert
            Assert.Equal(day, sentDay);
        }

        #endregion

        #region PopulateAllDaysAsync

        [Fact]
        public async Task PopulateAllDaysAsync_ServiceFailure_SendsMessage()
        {
            // arrange
            var tracker = new Tracker { Id = 0 };
            var vm = new TrackerLogViewModel(tracker, new FailingTrackerLogService());

            bool messageSent = false;
            MessagingCenter.Subscribe<TrackerLogViewModel>(this, TrackerLogViewModel.DatabaseErrorMessage,
               sender => messageSent = true);

            // act
            await vm.PopulateAllDaysAsync();

            // assert
            Assert.True(messageSent);
        }

        [Fact]
        public async Task PopulateAllDaysAsync_ServiceFailure_DoesNotChangeState()
        {
            // arrange
            var tracker = new Tracker { Id = 0 };
            var day = new LoggedDay { Id = 0, TrackerId = 0 };
            var vm = new TrackerLogViewModel(tracker, new FailingTrackerLogService());
            vm.AllDays.Add(day);

            // act
            await vm.PopulateAllDaysAsync();

            // assert
            Assert.Single(vm.AllDays, expected: day);
        }

        #endregion
    }
}
