using SQLite;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TableAttribute = SQLite.TableAttribute;

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
        [Key]
        [PrimaryKey, AutoIncrement]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        /// <summary>
        /// Id of the <see cref="Tracker"/> this day is logged under.
        /// </summary>
        [ForeignKey(nameof(Tracker))]
        [Indexed]
        public int TrackerId { get; set; }

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
