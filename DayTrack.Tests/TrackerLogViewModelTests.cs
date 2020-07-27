using DayTrack.Models;
using DayTrack.Tests.Mocks;
using DayTrack.ViewModels;
using System;
using System.Collections.ObjectModel;
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

        #region PopulateAllDayGroupsAsync

        [Fact]
        public async Task PopulateAllDayGroupsAsync_ServiceFailure_SendsMessage()
        {
            // arrange
            var tracker = new Tracker { Id = 0 };
            var vm = new TrackerLogViewModel(tracker, new FailingTrackerLogService());

            bool messageSent = false;
            MessagingCenter.Subscribe<TrackerLogViewModel>(this, TrackerLogViewModel.DatabaseErrorMessage,
               sender => messageSent = true);

            // act
            await vm.PopulateAllDayGroupsAsync();

            // assert
            Assert.True(messageSent);
        }

        [Fact]
        public async Task PopulateAllDayGroupsAsync_ServiceFailure_DoesNotChangeState()
        {
            // arrange
            var tracker = new Tracker { Id = 0 };
            var group = new LoggedDayGroup { Date = DateTime.Now.Date, Count = 2 };
            var vm = new TrackerLogViewModel(tracker, new FailingTrackerLogService());
            vm.AllDayGroups.Add(group);

            // act
            await vm.PopulateAllDayGroupsAsync();

            // assert
            Assert.Single(vm.AllDayGroups, expected: group);
        }

        #endregion

        #region PopulateStatsAsync

        [Fact]
        public async Task PopulateStatsAsync_ServiceFailure_SendsMessage()
        {
            // arrange
            var tracker = new Tracker { Id = 0 };
            var vm = new TrackerLogViewModel(tracker, new FailingTrackerLogService());

            bool messageSent = false;
            MessagingCenter.Subscribe<TrackerLogViewModel>(this, TrackerLogViewModel.DatabaseErrorMessage,
               sender => messageSent = true);

            // act
            await vm.PopulateStatsAsync();

            // assert
            Assert.True(messageSent);
        }

        [Fact]
        public async Task PopulateStatsAsync_ServiceFailure_DoesNotComputeStats()
        {
            // arrange
            var tracker = new Tracker { Id = 0 };
            var vm = new TrackerLogViewModel(tracker, new FailingTrackerLogService());

            // act
            await vm.PopulateStatsAsync();

            // assert
            Assert.Null(vm.LoggedDayStats);
        }

        [Fact]
        public async Task PopulateStatsAsync_NoLoggedDays_DoesNotComputeStats()
        {
            // arrange
            var tracker = new Tracker { Id = 0 };
            var vm = new TrackerLogViewModel(tracker, new MockTrackerLogService());

            // act
            await vm.PopulateStatsAsync();

            // assert
            Assert.Null(vm.LoggedDayStats);
        }

        [Fact]
        public async Task PopulateStatsAsync_NullAllDayGroups_DoesNotComputeStats()
        {
            // arrange
            var tracker = new Tracker { Id = 0 };
            var vm = new TrackerLogViewModel(tracker, new MockTrackerLogService())
            {
                AllDayGroups = null
            };

            // act
            await vm.PopulateStatsAsync();

            // assert
            Assert.Null(vm.LoggedDayStats);
        }

        [Fact]
        public async Task PopulateStatsAsync_WithLoggedDays_ComputesCorrectMin()
        {
            // arrange
            var tracker = new Tracker { Id = 0 };
            var service = new MockTrackerLogService();
            service.LoggedDays.Add(new LoggedDay { Date = new DateTime(2002, 1, 1) });
            service.LoggedDays.Add(new LoggedDay { Date = new DateTime(2002, 1, 1) });
            service.LoggedDays.Add(new LoggedDay { Date = new DateTime(2000, 1, 1) });
            var vm = new TrackerLogViewModel(tracker, service);
            await vm.PopulateAllDayGroupsAsync();

            // act
            await vm.PopulateStatsAsync();

            // assert
            Assert.Equal(1, vm.LoggedDayStats.Min);
        }

        [Fact]
        public async Task PopulateStatsAsync_WithLoggedDays_ComputesCorrectMax()
        {
            // arrange
            var tracker = new Tracker { Id = 0 };
            var service = new MockTrackerLogService();
            service.LoggedDays.Add(new LoggedDay { Date = new DateTime(2002, 1, 1) });
            service.LoggedDays.Add(new LoggedDay { Date = new DateTime(2002, 1, 1) });
            service.LoggedDays.Add(new LoggedDay { Date = new DateTime(2000, 1, 1) });
            var vm = new TrackerLogViewModel(tracker, service);
            await vm.PopulateAllDayGroupsAsync();

            // act
            await vm.PopulateStatsAsync();

            // assert
            Assert.Equal(2, vm.LoggedDayStats.Max);
        }

        [Fact]
        public async Task PopulateStatsAsync_OnePerDay_ComputesCorrectAverage()
        {
            // arrange
            var tracker = new Tracker { Id = 0 };
            var service = new MockTrackerLogService();
            service.LoggedDays.Add(new LoggedDay { Date = new DateTime(2000, 1, 1) });
            service.LoggedDays.Add(new LoggedDay { Date = new DateTime(2000, 1, 2) });
            service.LoggedDays.Add(new LoggedDay { Date = new DateTime(2000, 1, 3) });
            var vm = new TrackerLogViewModel(tracker, service);
            await vm.PopulateAllDayGroupsAsync();

            // act
            await vm.PopulateStatsAsync();

            // assert
            Assert.Equal(1.0, vm.LoggedDayStats.Average);
        }

        [Fact]
        public async Task PopulateStatsAsync_HalfOfDays_ComputesCorrectAverage()
        {
            // arrange
            var tracker = new Tracker { Id = 0 };
            var service = new MockTrackerLogService();
            service.LoggedDays.Add(new LoggedDay { Date = new DateTime(2000, 1, 1) });
            service.LoggedDays.Add(new LoggedDay { Date = new DateTime(2000, 1, 4) });
            var vm = new TrackerLogViewModel(tracker, service);
            await vm.PopulateAllDayGroupsAsync();

            // act
            await vm.PopulateStatsAsync();

            // assert
            Assert.Equal(0.5, vm.LoggedDayStats.Average);
        }

        [Fact]
        public async Task PopulateStatsAsync_OneDay_ComputesCorrectAverage()
        {
            // arrange
            var tracker = new Tracker { Id = 0 };
            var service = new MockTrackerLogService();
            service.LoggedDays.Add(new LoggedDay { Date = new DateTime(2000, 1, 1) });
            var vm = new TrackerLogViewModel(tracker, service);
            await vm.PopulateAllDayGroupsAsync();

            // act
            await vm.PopulateStatsAsync();

            // assert
            Assert.Equal(1.0, vm.LoggedDayStats.Average);
        }

        [Fact]
        public async Task PopulateStatsAsync_OddLoggedDays_ComputesCorrectMedian()
        {
            // arrange
            var tracker = new Tracker { Id = 0 };
            var service = new MockTrackerLogService();
            service.LoggedDays.Add(new LoggedDay { Date = new DateTime(2002, 1, 1) });
            service.LoggedDays.Add(new LoggedDay { Date = new DateTime(2002, 1, 1) });
            service.LoggedDays.Add(new LoggedDay { Date = new DateTime(2001, 1, 1) });
            service.LoggedDays.Add(new LoggedDay { Date = new DateTime(2000, 1, 1) });
            service.LoggedDays.Add(new LoggedDay { Date = new DateTime(2000, 1, 1) });
            var vm = new TrackerLogViewModel(tracker, service);
            await vm.PopulateAllDayGroupsAsync();

            // act
            await vm.PopulateStatsAsync();

            // assert
            Assert.Equal(1, vm.LoggedDayStats.Median);
        }

        [Fact]
        public async Task PopulateStatsAsync_EvenLoggedDays_ComputesCorrectMedian()
        {
            // arrange
            var tracker = new Tracker { Id = 0 };
            var service = new MockTrackerLogService();
            service.LoggedDays.Add(new LoggedDay { Date = new DateTime(2000, 1, 1) });
            service.LoggedDays.Add(new LoggedDay { Date = new DateTime(2000, 1, 1) });
            service.LoggedDays.Add(new LoggedDay { Date = new DateTime(2000, 1, 1) });
            service.LoggedDays.Add(new LoggedDay { Date = new DateTime(2000, 1, 1) });

            service.LoggedDays.Add(new LoggedDay { Date = new DateTime(2001, 1, 1) });
            service.LoggedDays.Add(new LoggedDay { Date = new DateTime(2001, 1, 1) });
            service.LoggedDays.Add(new LoggedDay { Date = new DateTime(2001, 1, 1) });

            service.LoggedDays.Add(new LoggedDay { Date = new DateTime(2002, 1, 1) });
            service.LoggedDays.Add(new LoggedDay { Date = new DateTime(2002, 1, 1) });

            service.LoggedDays.Add(new LoggedDay { Date = new DateTime(2003, 1, 1) });
            var vm = new TrackerLogViewModel(tracker, service);
            await vm.PopulateAllDayGroupsAsync();

            // act
            await vm.PopulateStatsAsync();

            // assert
            Assert.Equal(3, vm.LoggedDayStats.Median);
        }

        #endregion
    }
}
