using DayTrack.Utils;

namespace DayTrack.Models
{
    /// <summary>
    /// Contains user-chosen application settings.
    /// </summary>
    public class UserSettings
    {
        /// <summary>
        /// Date display format (defaults to <see cref="DateFormats.ShortYearMonthDay"/>).
        /// </summary>
        public string DateFormat { get; set; } = DateFormats.ShortYearMonthDay;
    }
}
