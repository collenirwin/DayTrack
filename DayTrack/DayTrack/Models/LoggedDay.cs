using SQLite;
using System;

namespace DayTrack.Models
{
    /// <summary>
    /// Represents a day logged under a <see cref="Tracker"/>.
    /// </summary>
    [Table("LoggedDays")]
    public class LoggedDay : ITimeStamped
    {
        /// <summary>
        /// Auto-generated primary key.
        /// </summary>
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        /// <summary>
        /// Id of the <see cref="Tracker"/> this day is logged under.
        /// </summary>
        [Indexed]
        public int TrackerId { get; set; }

        /// <summary>
        /// The day logged.
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// Creation timestamp.
        /// </summary>
        public DateTime Created { get; set; } = DateTime.Now;
    }
}
