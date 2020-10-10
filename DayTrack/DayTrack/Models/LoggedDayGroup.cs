using System;

namespace DayTrack.Models
{
    /// <summary>
    /// Represents a group of <see cref="LoggedDay"/>s with the same <see cref="LoggedDay.Date"/>.
    /// </summary>
    public class LoggedDayGroup
    {
        /// <summary>
        /// The day logged.
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// <see cref="Date"/> in string form.
        /// </summary>
        public string DateString { get; set; }

        /// <summary>
        /// The number of other <see cref="LoggedDay"/>s logged for this day.
        /// </summary>
        public int Count { get; set; }
    }
}
