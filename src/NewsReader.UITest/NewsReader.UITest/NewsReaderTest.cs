using Xunit;

namespace UITest.NewsReader;

[DeviceCollectionTrait(DevicePlatform.Android)] public class AndroidNewsReaderTest : NewsReaderTest { }
[DeviceCollectionTrait(DevicePlatform.IOS)] public class IOSNewsReaderTest : NewsReaderTest { }
[DeviceCollectionTrait(DevicePlatform.Windows)] public class WindowsNewsReaderTest : NewsReaderTest { }


public abstract class NewsReaderTest : UITest
{
    [Fact]
    public void OpenFirstNewsTest()
    {
        if (IsWindows) Driver.Manage().Window.Maximize();
        var window = GetShellWindow();
        if (!IsWindows) window.MenuButton.SafeClick();
        var menuView = window.MenuView;
        var firstItem = menuView.FeedNavigationItems[0];        
        Log.WriteLine(("1. Feed:", firstItem.TitleLabel.Text));
        firstItem.Element.SafeClick();
        if (!IsWindows) window.TapEmptySpace();  // Close menu flyout

        var feedView = window.FeedView;
        Log.WriteLine("Feed items:");
        foreach (var x in feedView.FeedItems) Log.WriteLine("  " + x.NameLabel.Text);
        feedView.FeedItems[0].Element.SafeClick();

        var feedItemView = window.FeedItemView;
        Thread.Sleep(2000);
        CreateScreenshot("FeedView");        

        window.Back();
        feedView.SearchButton.SafeClick();
        feedView.SearchBar.EnterText("DoesNotExist_34jlk534");
        Assert.Equal("DoesNotExist_34jlk534", feedView.SearchBar.Text);
        if (IsWindows) Thread.Sleep(1000);
        Assert.Empty(feedView.FeedItems);
        feedView.SearchBar.Clear();
        Thread.Sleep(1000);
        Assert.NotEmpty(feedView.FeedItems);
    }

    [Fact]
    public void InfoViewTest()
    {
        if (IsWindows) Driver.Manage().Window.Maximize();
        var window = GetShellWindow();
        if (!IsWindows) window.MenuButton.SafeClick();
        window.MenuView.SettingsItem.SafeClick();
        var settingsView = window.SettingsView;
        settingsView.InfoTabButton.SafeClick();
        Log.WriteLine(("Version:", settingsView.InfoView.VersionLabel.Text));
        Thread.Sleep(1000);
        CreateScreenshot("About");
    }
}
