using UITest.FeedService;
using UITest.NewsReader.Controls;
using Xunit;

namespace UITest.NewsReader;

[DeviceCollectionTrait(DevicePlatform.Android)] public class AndroidCustomFeedServiceTest : CustomFeedServiceTest { }
[DeviceCollectionTrait(DevicePlatform.Windows)] public class WindowsCustomFeedServiceTest : CustomFeedServiceTest { }

// Android: This works only with the Google Android Emulator.
public abstract class CustomFeedServiceTest : UITest
{
    [Fact]
    public async Task CustomFeedServiceAddAndListTest()
    {
        await using var feedWebApp = FeedWebApp.RunService(new SyndicationData(), null);

        if (IsWindows) Driver.Manage().Window.Maximize();
        var window = GetShellWindow();
        if (!IsWindows) window.MenuButton.SafeClick();
        var menuView = window.MenuView;
        var itemsCount = menuView.FeedNavigationItems.Count;
        menuView.AddFeedItem.SafeClick();
        var addEditFeedView = window.AddEditFeedView;
        var host = IsAndroid ? "10.0.2.2" : "localhost";
        addEditFeedView.FeedUrlEntry.EnterText($"http://{host}:5000/feed/rss");
        addEditFeedView.LoadFeedButton.SafeClick();
        await Task.Delay(1000, CancellationToken.None);

        Assert.Equal("FeedTitle", addEditFeedView.FeedNameEntry.Text);
        Assert.Empty(addEditFeedView.FeedErrorLabel.Text);
        addEditFeedView.AddEditButton.SafeClick();

        if (!IsWindows) window.MenuButton.SafeClick();
        menuView = window.MenuView;
        Assert.Equal(itemsCount + 1, menuView.FeedNavigationItems.Count);
        var lastItem = menuView.FeedNavigationItems[^1];
        Assert.Equal("FeedTitle", lastItem.TitleLabel.Text);

        lastItem.Element.SafeClick();
        if (!IsWindows) window.TapEmptySpace();  // Close menu flyout
        var feedView = window.FeedView;
        var item = feedView.FeedItems.Single();
        Assert.Equal("ItemTitle", item.NameLabel.Text);
        Assert.Equal("ItemContent", item.DescriptionLabel.Text);

        if (!IsWindows) window.MenuButton.SafeClick();
        menuView = window.MenuView;
        lastItem = menuView.FeedNavigationItems[^1];
        if (IsWindows)
        {
            lastItem.Element.SafeClick(isRightButton: true);
            menuView.ContextMenu.RemoveMenuItem.SafeClick();
        }
        else
        {
            lastItem.Element.SwipeRight();
            menuView.SwipeView.RemoveSwipeItem.SafeClick();
        }
        var popup = new YesNoPopup(Driver);
        Assert.Contains("FeedTitle", popup.Message);
        popup.YesButton.SafeClick();
        Assert.Equal(itemsCount, menuView.FeedNavigationItems.Count);
    }
}
