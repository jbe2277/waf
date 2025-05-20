using OpenQA.Selenium.Appium;

namespace UITest.NewsReader.Views;

public record ShellWindow(AppiumDriver Driver)
{
    public AppiumElement SettingsItem => Driver.Find("SettingsItem");

    public SettingsView SettingsView => new(Driver.Find("SettingsView"));
}
