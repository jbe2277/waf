using OpenQA.Selenium.Appium;

namespace UITest.NewsReader.Views;

public record ShellWindow(AppiumDriver Driver)
{
    public AppiumElement MenuButton => Driver.OnPlatform(
        android: () => Driver.Find(MobileBy.AccessibilityId("Open navigation drawer")),
        iOS: () => throw new NotSupportedException(),
        windows: () => throw new NotSupportedException()
    );
    
    public AppiumElement SettingsItem => Driver.Find("SettingsItem");

    public SettingsView SettingsView => new(Driver.Find("SettingsView"));
}
