using OpenQA.Selenium.Appium.Enums;
using OpenQA.Selenium.Appium;

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
        if (element.IsWindows()) Thread.Sleep(100);
        element.Click();
        if (element.IsWindows()) Thread.Sleep(200);
    }
}
