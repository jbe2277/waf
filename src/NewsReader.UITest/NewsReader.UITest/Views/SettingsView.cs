using OpenQA.Selenium.Appium;

namespace UITest.NewsReader.Views;

public record SettingsView(AppiumElement Element)
{
    public IReadOnlyList<AppiumElement> TabItems => Element.Find("TopNavMenuItemsHost").FindAll("navViewItem");

    public InfoView InfoView => new(Element.Find("InfoView"));
}

public record InfoView(AppiumElement Element)
{
    public AppiumElement VersionLabel => Element.Find("VersionLabel");
}
