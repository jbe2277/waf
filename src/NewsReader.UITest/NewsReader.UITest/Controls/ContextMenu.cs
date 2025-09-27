using OpenQA.Selenium.Appium;

namespace UITest.NewsReader.Controls;

public record ContextMenu(AppiumDriver Driver)
{
    public AppiumElement Element => Driver.OnPlatform(
        windows: () => Driver.Find(MobileBy.Name("PopupHost")).Find(MobileBy.ClassName("MenuFlyout")));

    public IReadOnlyList<AppiumElement> MenuItems => Element.FindAll(MobileBy.ClassName("MenuFlyoutItem"), 1);
}