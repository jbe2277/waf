using OpenQA.Selenium.Appium;

namespace UITest.NewsReader.Controls;

public record Popup(AppiumDriver Driver)
{
    public AppiumElement Element => Driver.OnPlatform(
        android: () => Driver.Find("action_bar_root"),
        iOS: () => Driver.Find(MobileBy.ClassName("XCUIElementTypeAlert")),
        windows: () => Driver.FindAll(MobileBy.ClassName("Popup"))[1]);

    public string Title => Driver.OnPlatform(
        android: () => Element.Find("alertTitle"),
        iOS: () => Element.Find(MobileBy.ClassName("XCUIElementTypeStaticText")),
        windows: () => Element.Find(MobileBy.ClassName("TextBlock"))).Text;

    public string Message => Driver.OnPlatform(
        android: () => Element.Find("android:id/message"),
        iOS: () => Element.FindAll(MobileBy.ClassName("XCUIElementTypeStaticText"))[1],
        windows: () => Element.FindAll(MobileBy.ClassName("TextBlock"))[1]).Text;
}