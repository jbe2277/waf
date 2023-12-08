using Test.NewsReader.Applications.Views;
using Waf.NewsReader.Applications.ViewModels;
using Xunit;

namespace Test.NewsReader.Applications.Controllers;

public class AppControllerTest : TestBase
{
    [Fact]
    public async Task OpenSettingsView()
    {
        StartApp();
        await Task.Delay(50);  // TODO: Waiting for some time is not the best way to solve this
        var feed = Shell.View.GetCurrentView<MockFeedView, FeedViewModel>();
        Assert.Equal(FeedsController.FeedManager.Feeds.Single(), feed.ViewModel.Feed);

        Shell.ViewModel.SelectedFooterMenu = Shell.ViewModel.FooterMenu[1];
        var settings = Shell.View.GetCurrentView<MockSettingsView, SettingsViewModel>();
        Assert.NotNull(settings.View);
    }
}
