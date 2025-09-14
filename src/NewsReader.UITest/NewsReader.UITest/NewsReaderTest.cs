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
        var firstFeed = menuView.FeedNavigationItems[0];        
        Log.WriteLine(("1. Feed:", firstFeed.TitleLabel.Text));
        firstFeed.Element.SafeClick();
        if (!IsWindows) window.TapEmptySpace();  // Close menu flyout

        var feedView = window.FeedView;
        Log.WriteLine("Feed items:");
        foreach (var x in feedView.FeedItems) Log.WriteLine("  " + x.NameLabel.Text);
        var item = feedView.FeedItems[0];
        Assert.False(item.MarkAsRead);
        item.Element.SafeClick();

        var feedItemView = window.FeedItemView;
        Thread.Sleep(4500);
        CreateScreenshot("FeedView");        

        window.Back();
        if (IsWindows) Thread.Sleep(1000);
        Assert.True(item.MarkAsRead);
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

    [Fact]
    public void AddFeedValidationTest()
    {
        if (IsWindows) Driver.Manage().Window.Maximize();
        var window = GetShellWindow();
        if (!IsWindows) window.MenuButton.SafeClick();
        var menuView = window.MenuView;
        menuView.AddFeedItem.SafeClick();
        var addEditFeedView = window.AddEditFeedView;
        Assert.False(addEditFeedView.AddEditButton.Enabled);
        Assert.Empty(addEditFeedView.TryLoadErrorLabel?.Text ?? "");
        Assert.True(addEditFeedView.FeedUrlEntry.IsTextEmpty);
        addEditFeedView.FeedUrlEntry.EnterText("wrong");
        addEditFeedView.LoadFeedButton.SafeClick();

        Assert.True(addEditFeedView.FeedNameEntry.IsTextEmpty);
        Assert.NotEmpty(addEditFeedView.FeedErrorLabel.Text);
        addEditFeedView.FeedNameEntry.EnterText("Test");
        Thread.Sleep(1000);
        Assert.Empty(addEditFeedView.FeedErrorLabel.Text);

        Thread.Sleep(5000);
        Assert.NotEmpty(addEditFeedView.TryLoadErrorLabel?.Text ?? "");
        Assert.False(addEditFeedView.AddEditButton.Enabled);
    }
}
