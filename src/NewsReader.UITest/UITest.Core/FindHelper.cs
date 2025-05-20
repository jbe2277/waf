using OpenQA.Selenium.Appium.Enums;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium;
using System.Xml.Linq;

namespace UITest;

public static class FindHelper
{
    public static IReadOnlyList<AppiumElement> FindAll(this AppiumDriver driver, string automationId, int atLeast = 0) => WaitFindAll((d, id) => d.FindElements(driver.ById(id)), driver, automationId, atLeast);

    public static IReadOnlyList<AppiumElement> FindAll(this AppiumElement element, string automationId, int atLeast = 0) => WaitFindAll((d, id) => element.FindElements(d.ById(id)), element.GetDriver(), automationId, atLeast, element);

    public static IReadOnlyList<AppiumElement> FindAll(this AppiumDriver driver, By by, int atLeast = 0) => WaitFindAll((d, id) => d.FindElements(by), driver, by.ToString(), atLeast);

    public static IReadOnlyList<AppiumElement> FindAll(this AppiumElement element, By by, int atLeast = 0) => WaitFindAll((d, id) => element.FindElements(by), element.GetDriver(), by.ToString(), atLeast, element);

    public static AppiumElement Find(this AppiumDriver driver, string automationId) => WaitFind((d, id) => d.FindElement(d.ById(id)), driver, automationId);

    public static AppiumElement Find(this AppiumElement element, string automationId) => WaitFind((d, id) => element.FindElement(d.ById(id)), element.GetDriver(), automationId, element);

    public static AppiumElement Find(this AppiumDriver driver, By by) => WaitFind((d, id) => d.FindElement(by), driver, by.ToString());

    public static AppiumElement Find(this AppiumElement element, By by) => WaitFind((d, id) => element.FindElement(by), element.GetDriver(), by.ToString(), element);

    public static AppiumElement? TryFind(this AppiumDriver driver, string automationId) => TryFind(driver, driver.ById(automationId));

    public static AppiumElement? TryFind(this AppiumElement element, string automationId) => TryFind(element, element.GetDriver().ById(automationId));

    public static AppiumElement? TryFind(this AppiumDriver driver, By by)
    {
        try
        {
            return driver.FindElement(by);
        }
        catch (NoSuchElementException)
        {
            return null;
        }
    }

    public static AppiumElement? TryFind(this AppiumElement element, By by)
    {
        try
        {
            return element.FindElement(by);
        }
        catch (NoSuchElementException)
        {
            return null;
        }
    }

    public static By ById(this AppiumDriver driver, string automationId) => driver.PlatformName switch
    {
        MobilePlatform.Android => MobileBy.Id(automationId),
        MobilePlatform.IOS => MobileBy.Id(automationId),
        MobilePlatform.Windows => MobileBy.AccessibilityId(automationId),
        _ => throw new NotSupportedException($"Platform not supported: {driver.PlatformName}")
    };
    
    private static AppiumElement WaitFind(Func<AppiumDriver, string, AppiumElement> func, AppiumDriver driver, string automationId, AppiumElement? owner = null)
    {
        try
        {
            return driver.Wait().Until(x => func(driver, automationId));
        }
        catch (Exception ex) when (ex is NoSuchElementException or WebDriverTimeoutException)
        {
            var ownerInfo = owner is null ? "driver" : GetElementInfo(owner);
            throw new NoSuchElementException($"Element '{automationId}' not found." + Environment.NewLine + Environment.NewLine
                + "Owner: " + ownerInfo + Environment.NewLine + Environment.NewLine
                + FormatXml(driver.PageSource) + Environment.NewLine + Environment.NewLine, ex);
        }
    }

    private static IReadOnlyList<AppiumElement> WaitFindAll(Func<AppiumDriver, string, IReadOnlyList<AppiumElement>> func, AppiumDriver driver, string automationId, int atLeast, AppiumElement? owner = null)
    {
        try
        {
            return driver.Wait().Until(x =>
            {
                var result = func(driver, automationId);
                return (result.Count >= atLeast) ? result : null;
            });
        }
        catch (Exception ex) when (ex is NoSuchElementException or WebDriverTimeoutException)
        {
            var ownerInfo = owner is null ? "driver" : GetElementInfo(owner);
            throw new NoSuchElementException($"Element '{automationId}' not found at least {atLeast} times." + Environment.NewLine + Environment.NewLine
                + "Owner: " + ownerInfo + Environment.NewLine + Environment.NewLine
                + FormatXml(driver.PageSource) + Environment.NewLine + Environment.NewLine, ex);
        }
    }

    private static string GetElementInfo(AppiumElement element)
    {
        return $"AId=\"{GetAutomationId(element)}\" TagName=\"{TryGet(() => element.TagName)}\" Text=\"{TryGet(() => element.Text)}\" Rect=\"{TryGet(element.Rect.ToString)}\" Id=\"{TryGet(() => element.Id)}\"";

        static string? GetAutomationId(AppiumElement element) => element.GetDriver().PlatformName switch
        {
            MobilePlatform.Android => TryGet(() => element.GetAttribute("resource-id")),
            MobilePlatform.IOS => TryGet(() => element.GetAttribute("name")),
            MobilePlatform.Windows => TryGet(() => element.GetAttribute("AutomationId")),
            _ => null
        };

        static string? TryGet(Func<string> func)
        {
            try { return func(); }
            catch { /* Ignore */ }
            return null;
        }
    }

    private static string FormatXml(string xml)
    {
        try { return XDocument.Parse(xml).ToString(); }
        catch { return xml; }
    }
}
