using OpenQA.Selenium.Appium.Enums;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium;

namespace UITest;

public static class ContextHelper
{
    public static TimeSpan DefaultTimeout { get; set; } = TimeSpan.FromSeconds(10);

    public static AppiumDriver GetDriver(this AppiumElement element) => (AppiumDriver)element.WrappedDriver;

    public static WebDriverWait Wait(this IWebDriver driver)
    {
        var w = new WebDriverWait(driver, DefaultTimeout);
        w.IgnoreExceptionTypes(typeof(StaleElementReferenceException));
        return w;
    }

    public static WebDriverWait Wait(this AppiumElement element) => element.WrappedDriver.Wait();

    public static bool IsAndroid(this AppiumDriver driver) => driver.PlatformName == MobilePlatform.Android;

    public static bool IsAndroid(this AppiumElement element) => IsAndroid(element.GetDriver());

    public static bool IsIOS(this AppiumDriver driver) => driver.PlatformName == MobilePlatform.IOS;

    public static bool IsIOS(this AppiumElement element) => IsIOS(element.GetDriver());

    public static bool IsWindows(this AppiumDriver driver) => driver.PlatformName == MobilePlatform.Windows;

    public static bool IsWindows(this AppiumElement element) => IsWindows(element.GetDriver());

    public static T OnPlatform<T>(this AppiumElement element, Func<T>? android = null, Func<T>? iOS = null, Func<T>? windows = null) => OnPlatform(element.GetDriver(), android, iOS, windows);

    public static T OnPlatform<T>(this AppiumDriver driver, Func<T>? android = null, Func<T>? iOS = null, Func<T>? windows = null)
    {
        if (driver.IsAndroid() && android is not null) return android();
        else if (driver.IsIOS() && iOS is not null) return iOS();
        else if (driver.IsWindows() && windows is not null) return windows();
        throw new NotSupportedException($"Platform '{driver.PlatformName}' is not supported");
    }
}
