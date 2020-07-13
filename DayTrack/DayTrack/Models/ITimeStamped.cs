using System;

namespace DayTrack.Models
{
    /// <summary>
    /// Has a <see cref="Created"/> property representing the date the model was created.
    /// </summary>
    public interface ITimeStamped
    {
        DateTime Created { get; set; }
    }
}
