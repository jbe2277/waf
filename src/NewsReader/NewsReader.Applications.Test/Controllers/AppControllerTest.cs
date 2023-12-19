using Test.NewsReader.Applications.Views;
using Waf.NewsReader.Applications.ViewModels;
using Xunit;

namespace Test.NewsReader.Applications.Controllers;

public class AppControllerTest : TestBase
{
    [Fact]
    public void OpenSettingsView()
    {
        StartApp();
        var feed = Context.WaitForNotNull(Shell.View.GetCurrentViewOrNull<MockFeedView, FeedViewModel>);
        var feed1 = FeedsController.FeedManager.Feeds.Single();
        Assert.Equal(feed1, feed.ViewModel.Feed);
        Assert.Equal(feed1, Shell.ViewModel.SelectedFeed);
        Assert.Null(Shell.ViewModel.SelectedFooterMenu);

        Shell.ViewModel.SelectedFooterMenu = Shell.ViewModel.FooterMenu[1];
        var settings = Shell.View.GetCurrentView<MockSettingsView, SettingsViewModel>();
        Assert.NotNull(settings.View);
        Assert.Null(Shell.ViewModel.SelectedFeed);
    }
}
