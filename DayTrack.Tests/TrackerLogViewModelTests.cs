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
    }
}
