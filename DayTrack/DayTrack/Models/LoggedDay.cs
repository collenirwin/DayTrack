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
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [ForeignKey(nameof(Tracker))]
        public int TrackerId { get; set; }
        public Tracker Tracker { get; set; }

        [DataType(DataType.Date)]
        public DateTime Date { get; set; }
        public DateTime Created { get; set; } = DateTime.Now;
    }
}
