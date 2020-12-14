using DayTrack.Models;
using DayTrack.ViewModels;
using Microcharts;
using SkiaSharp;
using System;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DayTrack.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TrackerStatsPage : ContentPage
    {
        private readonly TrackerLogViewModel _viewModel;

        public TrackerStatsPage()
        {
            InitializeComponent();
        }

        public TrackerStatsPage(Tracker tracker, TrackerLogViewModel trackerLogViewModel) : this()
        {
            Title = $"{tracker.Name} stats";
            BindingContext = _viewModel = trackerLogViewModel;
        }

        private async void OnAppearing(object sender, EventArgs e)
        {
            await _viewModel.PopulateStatsAsync();

            if (_viewModel.AllDayGroups is null)
            {
                return;
            }

            // get a list of the past 14 days, match that against AllDayGroups to get counts on each day
            var pastEntries = Enumerable.Range(start: -13, count: 14)
                .Select(delta => DateTime.Now.Date.AddDays(delta))
                .Select(date => new
                {
                    Date = date,
                    Count = _viewModel.AllDayGroups.FirstOrDefault(group => group.Date == date)?.Count ?? 0
                });

            var lineColor = SKColor.Parse("#009688");
            ChartView.Chart = new PointChart
            {
                Entries = pastEntries.Select(entry => new ChartEntry(entry.Count)
                {
                    Label = entry.Date.DayOfWeek.ToString().Substring(0, 3).ToUpper(),
                    ValueLabel = entry.Count == 0 ? " " : entry.Count.ToString(),
                    Color = lineColor
                }),
                PointMode = PointMode.Circle,
                PointSize = 30,
                ValueLabelOrientation = Orientation.Horizontal,
                LabelOrientation = Orientation.Vertical,
                LabelTextSize = 35,
                BackgroundColor = SKColors.Transparent
            };
        }
    }
}
