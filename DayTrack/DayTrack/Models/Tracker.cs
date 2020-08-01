using SQLite;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TableAttribute = SQLite.TableAttribute;

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
        [Key]
        [PrimaryKey, AutoIncrement]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        /// <summary>
        /// The name of this tracker.
        /// </summary>
        [Indexed(Unique = true)]
        public string Name { get; set; }

        /// <summary>
        /// The days logged under this tracker.
        /// </summary>
        public ICollection<LoggedDay> LoggedDays { get; set; }

        /// <summary>
        /// Creation timestamp.
        /// </summary>
        public DateTime Created { get; set; } = DateTime.Now;
    }
}
