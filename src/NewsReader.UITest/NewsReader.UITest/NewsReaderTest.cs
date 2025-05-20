using Xunit;

namespace UITest.NewsReader;
public class NewsReaderTest : UITest
{
    [Fact]
    public void InfoViewTest()
    {
        Driver.Manage().Window.Maximize();
        var window = GetShellWindow();
        window.SettingsItem.SafeClick();
        var settingsView = window.SettingsView;
        settingsView.TabItems[2].SafeClick();
        Log.WriteLine("Version: " + settingsView.InfoView.VersionLabel.Text);
        Thread.Sleep(1000);
        CreateScreenshot("About");
    }
}
