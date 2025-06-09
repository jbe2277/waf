using OpenQA.Selenium.Appium;

namespace UITest.NewsReader.Views;

public record SettingsView(AppiumElement Element)
{
    public AppiumElement InfoTabButton => Element.OnPlatform(
        android: () => AndroidTopTabs.Find(MobileBy.AccessibilityId("Info")),
        iOS: () => throw new NotSupportedException(),
        windows: () => Element.Find("TopNavMenuItemsHost").FindAll("navViewItem")[2]);

    public InfoView InfoView => new(Element.Find("InfoView"));

    private AppiumElement AndroidTopTabs => Element.GetDriver().Find("navigationlayout_toptabs");
}

public record InfoView(AppiumElement Element)
{
    public AppiumElement VersionLabel => Element.Find("VersionLabel");
}
