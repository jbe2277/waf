using OpenQA.Selenium;
using OpenQA.Selenium.Appium;

namespace UITest.NewsReader.Views;

public record SettingsView(AppiumElement Element)
{
    public AppiumElement InfoTabButton => Element.OnPlatform(
        android: () => AndroidTopTabs.Find(MobileBy.AccessibilityId("Info")),
        iOS: () => Element.Find(By.XPath("""//XCUIElementTypeButton[@name="InfoView"]""")),
        windows: () => Element.Find("TopNavMenuItemsHost").FindAll("navViewItem")[2]);

    public InfoView InfoView => new(Element.OnPlatform(
        android: () => Element.Find("InfoView"),
        iOS: () => Element.Find(By.XPath("""//XCUIElementTypeOther[@name="InfoView"]""")),
        windows: () => Element.Find("InfoView")));

    private AppiumElement AndroidTopTabs => Element.GetDriver().Find("navigationlayout_toptabs");
}

public record InfoView(AppiumElement Element)
{
    public AppiumElement VersionLabel => Element.Find("VersionLabel");
}
