using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DayTrack.Models
{
    /// <summary>
    /// Represents a tracker the user can log days under.
    /// </summary>
    public class Tracker : ITimeStamped
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime Created { get; set; } = DateTime.Now;
        public ICollection<LoggedDay> LoggedDays { get; set; }
    }
}
