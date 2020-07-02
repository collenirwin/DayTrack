using System;

namespace DayTrack.Models
{
    public interface ITimeStamped
    {
        DateTime Created { get; set; }
    }
}
