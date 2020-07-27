﻿namespace DayTrack.Models
{
    /// <summary>
    /// Contains stats regarding the frequency of days logged under a tracker.
    /// </summary>
    public class LoggedDayStats
    {
        /// <summary>
        /// The average logs/day.
        /// </summary>
        public double Average { get; set; }

        /// <summary>
        /// The minimum (non-zero) logs/day.
        /// </summary>
        public int Min { get; set; }

        /// <summary>
        /// The maximum logs/day.
        /// </summary>
        public int Max { get; set; }

        /// <summary>
        /// The median logs/day.
        /// </summary>
        public int Median { get; set; }
    }
}