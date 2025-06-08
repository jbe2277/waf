using Xunit;

namespace UITest.NewsReader;

[DeviceCollectionTrait(DevicePlatform.Windows)] public class WindowsNewsReaderTest : NewsReaderTest { }
[DeviceCollectionTrait(DevicePlatform.Android)] public class AndroidNewsReaderTest : NewsReaderTest { }

public abstract class NewsReaderTest : UITest
{
    [Fact]
    public void InfoViewTest()
    {
        if (IsWindows) Driver.Manage().Window.Maximize();
        var window = GetShellWindow();
        if (IsAndroid) window.MenuButton.SafeClick();
        window.SettingsItem.SafeClick();
        var settingsView = window.SettingsView;
        settingsView.TabItems[2].SafeClick();
        Log.WriteLine(("Version:", settingsView.InfoView.VersionLabel.Text));
        Thread.Sleep(1000);
        CreateScreenshot("About");
    }
}
