namespace DayTrack.Views.Models
{
    /// <summary>
    /// Contains a <see cref="PageIdentifier"/> and a title for displaying a navigation link to a page.
    /// </summary>
    public class PageNavigationItem
    {
        public PageIdentifier Id { get; set; }
        public string Title { get; set; }
    }
}
