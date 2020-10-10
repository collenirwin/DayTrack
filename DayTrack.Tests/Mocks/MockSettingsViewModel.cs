using DayTrack.ViewModels;

namespace DayTrack.Tests.Mocks
{
    public class MockSettingsViewModel : ISettingsViewModel
    {
        public string DateFormat => "yyyy/MM/dd";
    }
}
