using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Android;
using OpenQA.Selenium.Appium.Android.Enums;
using OpenQA.Selenium.Interactions;
using System.Drawing;
using PointerInputDevice = OpenQA.Selenium.Appium.Interactions.PointerInputDevice;

namespace UITest.NewsReader.Views;

public record ShellWindow(AppiumDriver Driver)
{
    public AppiumElement MenuButton => Driver.OnPlatform(
        android: () => Driver.Find(MobileBy.AccessibilityId("Open navigation drawer")),
        iOS: () => Driver.Find("Menu"),
        windows: () => throw new NotSupportedException()
    );

    public MenuView MenuView => new(Driver.Find("MenuView"));
    
    public FeedView FeedView => new(Driver.Find("FeedView"));

    public FeedItemView FeedItemView => new(Driver.Find("FeedItemView"));

    public SettingsView SettingsView => new(Driver.Find("SettingsView"));

    public void Back()
    {
        if (Driver.IsAndroid()) ((AndroidDriver)Driver).PressKeyCode(AndroidKeyCode.Back);
        else if (Driver.IsIOS()) Driver.Navigate().Back();
        else if (Driver.IsWindows()) Driver.Find("NavigationViewBackButton").SafeClick();
        else throw new NotSupportedException();
    }

    public void TapEmptySpace()
    {
        var container = Driver.Manage().Window;
        var emptySpace = new Point((int)(container.Size.Width * 0.9), (int)(container.Size.Height * 0.5));

        var input = new PointerInputDevice(PointerKind.Touch);
        var tap = new ActionSequence(input);
        tap.AddAction(input.CreatePointerMove(CoordinateOrigin.Viewport, emptySpace.X, emptySpace.Y, TimeSpan.Zero));
        tap.AddAction(input.CreatePointerDown(MouseButton.Touch));
        tap.AddAction(input.CreatePointerUp(MouseButton.Touch));
        Driver.PerformActions([tap]);
    }
}

public record MenuView(AppiumElement Element)
{
    public IReadOnlyList<FeedNavigationItem> FeedNavigationItems => Element.Find("FeedList").FindAll("Feed").Select(x => new FeedNavigationItem(x)).ToArray();

    public AppiumElement SettingsItem => Element.Find("SettingsItem");
}

public record FeedNavigationItem(AppiumElement Element)
{
    public AppiumElement TitleLabel => Element.Find("TitleLabel");
}