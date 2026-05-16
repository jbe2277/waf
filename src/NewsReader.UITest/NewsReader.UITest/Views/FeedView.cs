using OpenQA.Selenium.Appium;
using UITest.NewsReader.Controls;

namespace UITest.NewsReader.Views;

public record FeedView(AppiumElement Element)
{
    public AppiumElement SearchButton => Element.OnPlatform(
        android: () => Element.Driver.Find(MobileBy.AccessibilityId("SearchButton")),
        windows: () => Element.Driver.Find("SearchButton"),
        iOS: () => Element.Driver.Find("SearchButton"));

    public SearchBar SearchBar => new(Element.Find("SearchBar"));

    public IReadOnlyList<FeedItem> FeedItems => Element.Find("FeedItemList").FindAll("FeedItem").Select(x => new FeedItem(x, Element)).ToArray();
}

public record FeedItem(AppiumElement Element, AppiumElement Parent)
{
    public AppiumElement NameLabel => Element.Find("NameLabel");

    public AppiumElement DescriptionLabel => Element.Find("DescriptionLabel");

    public bool MarkAsRead => Element.GetStatusInfo() switch
    {
        "MarkAsReadFalse" => false,
        "MarkAsReadTrue" => true,
        var x => throw new InvalidOperationException($"Status: {x} is not supported")
    };

    public FeedItemContextMenu ContextMenu => new(Element.Driver);

    public FeedItemSwipeView SwipeView => new(Parent);
}

public record FeedItemContextMenu(AppiumDriver Driver) : ContextMenu(Driver)
{
    public AppiumElement ReadUnreadMenuItem => MenuItems[0];
}

public record FeedItemSwipeView(AppiumElement Element)
{
    public AppiumElement ReadUnreadSwipeItem => Element.OnPlatform(
        android: () => Element.Find(MobileBy.AccessibilityId("ReadUnreadSwipeItem")),
        iOS: () => Element.Find(MobileBy.ClassName("XCUIElementTypeButton")));
}