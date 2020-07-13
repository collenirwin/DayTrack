using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DayTrack.Models
{
    /// <summary>
    /// Represents a day logged under a <see cref="Tracker"/>.
    /// </summary>
    public class LoggedDay : ITimeStamped
    {
        /// <summary>
        /// Auto-generated primary key.
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        /// <summary>
        /// Id of the <see cref="Tracker"/> this day is logged under.
        /// </summary>
        [ForeignKey(nameof(Tracker))]
        public int TrackerId { get; set; }

        /// <summary>
        /// Tracker this day is logged under.
        /// </summary>
        public Tracker Tracker { get; set; }

        /// <summary>
        /// The day logged.
        /// </summary>
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }

        /// <summary>
        /// Creation timestamp.
        /// </summary>
        public DateTime Created { get; set; } = DateTime.Now;
    }
}
