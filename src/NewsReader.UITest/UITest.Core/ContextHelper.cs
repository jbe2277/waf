using OpenQA.Selenium.Appium.Enums;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium;

namespace UITest;

public static class ContextHelper
{
    public static TimeSpan DefaultTimeout { get; set; } = TimeSpan.FromSeconds(10);

    extension(AppiumElement element)
    {
        public AppiumDriver Driver => (AppiumDriver)element.WrappedDriver;

        public bool IsAndroid => element.Driver.IsAndroid;

        public bool IsIOS => element.Driver.IsIOS;

        public bool IsWindows => element.Driver.IsWindows;

        public WebDriverWait Wait() => element.Driver.Wait();

        public T OnPlatform<T>(Func<T>? android = null, Func<T>? iOS = null, Func<T>? windows = null) => element.Driver.OnPlatform(android, iOS, windows);
    }

    extension(AppiumDriver driver)
    {
        public bool IsAndroid => driver.PlatformName == MobilePlatform.Android;

        public bool IsIOS => driver.PlatformName == MobilePlatform.IOS;

        public bool IsWindows => driver.PlatformName == MobilePlatform.Windows;

        public WebDriverWait Wait()
        {
            var w = new WebDriverWait(driver, DefaultTimeout);
            w.IgnoreExceptionTypes(typeof(StaleElementReferenceException));
            if (driver.IsWindows)
            {
                w.IgnoreExceptionTypes(typeof(InvalidOperationException));  // Workaround for GetElementId "The specified element ID is either null or the empty string."
            }
            return w;
        }

        public T OnPlatform<T>(Func<T>? android = null, Func<T>? iOS = null, Func<T>? windows = null)
        {
            if (driver.IsAndroid && android is not null) return android();
            else if (driver.IsIOS && iOS is not null) return iOS();
            else if (driver.IsWindows && windows is not null) return windows();
            throw new NotSupportedException($"Platform '{driver.PlatformName}' is not supported");
        }
    }
}
