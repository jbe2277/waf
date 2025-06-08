using OpenQA.Selenium.Appium;

namespace UITest.NewsReader.Views;

public record SettingsView(AppiumElement Element)
{
    public IReadOnlyList<AppiumElement> TabItems => Element.OnPlatform(
        android: () => 
        {
            var container = Element.GetDriver().Find("navigationlayout_toptabs");
            string[] buttons = ["General", "Data Sync", "Info"];
            return [.. buttons.Select(x => container.Find(MobileBy.AccessibilityId(x)))];
        },
        iOS: () => throw new NotSupportedException(),
        windows: () => Element.Find("TopNavMenuItemsHost").FindAll("navViewItem"));

    public InfoView InfoView => new(Element.Find("InfoView"));
}

public record InfoView(AppiumElement Element)
{
    public AppiumElement VersionLabel => Element.Find("VersionLabel");
}
