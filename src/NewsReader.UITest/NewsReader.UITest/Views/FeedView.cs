using OpenQA.Selenium.Appium;
using UITest.NewsReader.Controls;

namespace UITest.NewsReader.Views;

public record FeedView(AppiumElement Element)
{
    public AppiumElement SearchButton => Element.OnPlatform(
        android: () => Element.GetDriver().Find(MobileBy.AccessibilityId("SearchButton")),
        windows: () => Element.GetDriver().Find("SearchButton"),
        iOS: () => Element.GetDriver().Find("SearchButton"));

    public SearchBar SearchBar => new(Element.Find("SearchBar"));

    public IReadOnlyList<FeedItem> FeedItems => Element.Find("FeedItemList").FindAll("FeedItem").Select(x => new FeedItem(x)).ToArray();
}

public record FeedItem(AppiumElement Element)
{
    public AppiumElement NameLabel => Element.Find("NameLabel");

    public AppiumElement DescriptionLabel => Element.Find("DescriptionLabel");
}