using System.Waf.Applications.Services;

namespace Waf.BookLibrary.Library.Presentation.DesignData
{
    public class MockSettingsService : ISettingsService
    {
        public string FileName { get; set; } = @"C:\Users\User1\AppData\Local\Waf Book Library\Settings\Settings.xml";

        public event EventHandler<SettingsErrorEventArgs>? ErrorOccurred;

        public T Get<T>() where T : class, new() => new();

        public void Save() { }

        public void RaiseErrorOccurred(SettingsErrorEventArgs e) => ErrorOccurred?.Invoke(this, e);
    }
}
