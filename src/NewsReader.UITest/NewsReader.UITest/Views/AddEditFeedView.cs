using OpenQA.Selenium.Appium;
using UITest.NewsReader.Controls;

namespace UITest.NewsReader.Views;

public record AddEditFeedView(AppiumElement Element)
{
    public Entry FeedUrlEntry => new(Element.Find("FeedUrlEntry"), Placeholder: "Feed Url");

    public AppiumElement LoadFeedButton => Element.Find("LoadFeedButton");

    public AppiumElement? TryLoadErrorLabel => Element.TryFind("LoadErrorLabel");

    public AppiumElement FeedTitleLabel => Element.Find("FeedTitleLabel");

    public Entry FeedNameEntry => new(Element.Find("FeedNameEntry"), Placeholder: "Feed Name");

    public AppiumElement UseTitleAsNameButton => Element.Find("UseTitleAsNameButton");

    public AppiumElement FeedErrorLabel => Element.Find("FeedErrorLabel");
    public AppiumElement? TryFeedErrorLabel => Element.TryFind("FeedErrorLabel");

    public AppiumElement AddEditButton => Element.Find("AddEditButton");
}
