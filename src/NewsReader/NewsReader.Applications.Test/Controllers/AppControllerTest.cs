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
        Assert.Equal(FeedsController.FeedManager.Feeds.Single(), feed.ViewModel.Feed);

        Shell.ViewModel.SelectedFooterMenu = Shell.ViewModel.FooterMenu[1];
        var settings = Shell.View.GetCurrentView<MockSettingsView, SettingsViewModel>();
        Assert.NotNull(settings.View);
    }
}
