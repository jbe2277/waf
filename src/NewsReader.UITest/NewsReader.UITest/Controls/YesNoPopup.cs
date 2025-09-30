using OpenQA.Selenium.Appium;

namespace UITest.NewsReader.Controls;

public record YesNoPopup(AppiumDriver Driver) : Popup(Driver)
{
    public AppiumElement YesButton => Driver.OnPlatform(
        android: () => Element.Find("android:id/button1"),
        iOS: () => Element.FindAll(MobileBy.ClassName("XCUIElementTypeButton"))[1],
        windows: () => Element.Find("PrimaryButton"));

    public AppiumElement CloseButton => Driver.OnPlatform(
        android: () => Element.Find("android:id/button2"),
        iOS: () => Element.Find(MobileBy.ClassName("XCUIElementTypeButton")),
        windows: () => Element.Find("SecondaryButton"));
}
