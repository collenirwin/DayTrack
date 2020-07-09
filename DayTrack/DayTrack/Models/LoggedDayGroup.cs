using System;

namespace DayTrack.Models
{
    /// <summary>
    /// Represents a group of <see cref="LoggedDay"/>s with the same <see cref="LoggedDay.Date"/>.
    /// </summary>
    public class LoggedDayGroup
    {
        public DateTime Date { get; set; }
        public int Count { get; set; }
    }
}
