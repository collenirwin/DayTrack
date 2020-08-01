using SQLite;
using System;

namespace DayTrack.Models
{
    /// <summary>
    /// Represents a tracker the user can log days under.
    /// </summary>
    [Table("Trackers")]
    public class Tracker : ITimeStamped
    {
        /// <summary>
        /// Auto-generated primary key.
        /// </summary>
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        /// <summary>
        /// The name of this tracker.
        /// </summary>
        [Indexed(Unique = true)]
        public string Name { get; set; }

        /// <summary>
        /// Creation timestamp.
        /// </summary>
        public DateTime Created { get; set; } = DateTime.Now;
    }
}
