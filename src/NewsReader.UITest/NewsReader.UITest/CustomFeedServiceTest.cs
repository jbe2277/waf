using UITest.FeedService;
using UITest.NewsReader.Controls;
using Xunit;

namespace UITest.NewsReader;

[DeviceCollectionTrait(DevicePlatform.Windows)] public class WindowsCustomFeedServiceTest : CustomFeedServiceTest { }

public abstract class CustomFeedServiceTest : UITest
{
    [Fact]
    public async Task CustomFeedServiceAddAndListTest()
    {
        var cts = new CancellationTokenSource();
        var serviceTask = FeedWebApp.RunService(new SyndicationData(), null, cts.Token);

        if (IsWindows) Driver.Manage().Window.Maximize();
        var window = GetShellWindow();
        if (!IsWindows) window.MenuButton.SafeClick();
        var menuView = window.MenuView;
        var itemsCount = menuView.FeedNavigationItems.Count;
        menuView.AddFeedItem.SafeClick();
        var addEditFeedView = window.AddEditFeedView;
        addEditFeedView.FeedUrlEntry.EnterText("http://localhost:5000/feed/rss");
        addEditFeedView.LoadFeedButton.SafeClick();

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

        lastItem.Element.SafeClick(isRightButton: true);
        menuView.ContextMenu.RemoveMenuItem.SafeClick();
        var popup = new YesNoPopup(Driver);
        Assert.Contains("FeedTitle", popup.Message);
        popup.YesButton.SafeClick();
        Assert.Equal(itemsCount, menuView.FeedNavigationItems.Count);

        cts.Cancel();
        await serviceTask;
    }
}
