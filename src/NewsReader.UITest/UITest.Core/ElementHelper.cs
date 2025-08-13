using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Enums;

namespace UITest;

public static class ElementHelper
{
    public static T? ToView<T>(this AppiumElement? element, Func<AppiumElement, T> factory) where T : class => element is null ? null : factory(element);

    public static string GetStatusInfo(this AppiumElement element) => element.GetDriver().PlatformName switch
    {
        MobilePlatform.Android => element.GetAttribute("content-desc"),
        MobilePlatform.IOS => element.GetAttribute("label"),
        MobilePlatform.Windows => element.GetDomAttribute("Name"),  // Note: GetAttribute does not work here - but GetDomAttribute.
        _ => throw new NotSupportedException($"Platform not supported: {element.GetDriver().PlatformName}")
    };

    public static void SafeClick(this AppiumElement element)
    {
        if (element.IsWindows())
        {
            Thread.Sleep(100);
            var d = element.GetDriver();
            var windowPos = d.Manage().Window.Position;
            var center = element.Rect.Center();
            // Workaround: Add Window.Position to the coordinates: https://github.com/appium/appium-windows-driver/issues/280
            d.ExecuteScript("windows: hover", new Dictionary<string, object>()
            {
                ["startX"] = windowPos.X + center.X,
                ["startY"] = windowPos.Y + center.Y,
                ["endX"] = windowPos.X + center.X,
                ["endY"] = windowPos.Y + center.Y
            });
            element.Click();
            Thread.Sleep(200);
        }
        else element.Click();
    }

    public static void SafeSendKeys(this AppiumElement element, string text)
    {
        if (element.IsWindows())
        {
            foreach (var x in text)
            {
                element.SendKeys(x.ToString());
                Thread.Sleep(50);
            }
        }
        else element.SendKeys(text);
    }
}
