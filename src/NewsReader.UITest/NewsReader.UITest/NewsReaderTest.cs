using Xunit;

namespace UITest.NewsReader;

[DeviceCollectionTrait(DevicePlatform.Windows)] public class WindowsNewsReaderTest : NewsReaderTest { }
[DeviceCollectionTrait(DevicePlatform.Android)] public class AndroidNewsReaderTest : NewsReaderTest { }

public abstract class NewsReaderTest : UITest
{
    [Fact]
    public void OpenFirstNewsTest()
    {
        if (IsWindows) Driver.Manage().Window.Maximize();
        var window = GetShellWindow();
        if (IsAndroid) window.MenuButton.SafeClick();
        var menuView = window.MenuView;
        var firstItem = menuView.FeedNavigationItems[0];        
        Log.WriteLine(("1. Feed:", firstItem.TitleLabel.Text));
        firstItem.Element.SafeClick();
        if (IsAndroid) window.TapEmptySpace();  // Close menu flyout

        var feedView = window.FeedView;
        Log.WriteLine("Feed items:");
        foreach (var x in feedView.FeedItems) Log.WriteLine("  " + x.NameLabel.Text);
        feedView.FeedItems[0].Element.SafeClick();

        var feedItemView = window.FeedItemView;
        Thread.Sleep(1000);
        CreateScreenshot("FeedView");        

        window.Back();
    }

    [Fact]
    public void InfoViewTest()
    {
        if (IsWindows) Driver.Manage().Window.Maximize();
        var window = GetShellWindow();
        if (IsAndroid) window.MenuButton.SafeClick();
        window.MenuView.SettingsItem.SafeClick();
        var settingsView = window.SettingsView;
        settingsView.InfoTabButton.SafeClick();
        Log.WriteLine(("Version:", settingsView.InfoView.VersionLabel.Text));
        Thread.Sleep(1000);
        CreateScreenshot("About");
    }
}
