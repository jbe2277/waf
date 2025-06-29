using OpenQA.Selenium.Appium;

namespace UITest.NewsReader.Views;

public record FeedView(AppiumElement Element)
{
    public IReadOnlyList<FeedItem> FeedItems => Element.Find("FeedItemList").FindAll("FeedItem").Select(x => new FeedItem(x)).ToArray();
}

public record FeedItem(AppiumElement Element)
{
    public AppiumElement NameLabel = Element.Find("NameLabel");
}