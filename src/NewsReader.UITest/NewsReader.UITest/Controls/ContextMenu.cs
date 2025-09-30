using OpenQA.Selenium.Appium;

namespace UITest.NewsReader.Controls;

public record ContextMenu(AppiumDriver Driver)
{
    public IReadOnlyList<AppiumElement> MenuItems => Driver.FindAll(MobileBy.ClassName("MenuFlyoutItem"), 1);
}