using UITest.FeedService;
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
        menuView.AddFeedItem.SafeClick();
        var addEditFeedView = window.AddEditFeedView;
        addEditFeedView.FeedUrlEntry.EnterText("http://localhost:5000/feed/rss");
        addEditFeedView.LoadFeedButton.SafeClick();

        Assert.Equal("FeedTitle", addEditFeedView.FeedNameEntry.Text);
        Assert.Empty(addEditFeedView.FeedErrorLabel.Text);
        addEditFeedView.AddEditButton.SafeClick();

        if (!IsWindows) window.MenuButton.SafeClick();
        menuView = window.MenuView;
        var lastItem = menuView.FeedNavigationItems[^1];
        Assert.Equal("FeedTitle", lastItem.TitleLabel.Text);

        cts.Cancel();
        await serviceTask;
    }
}
