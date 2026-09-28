using OpenQA.Selenium;
using OpenQA.Selenium.Appium;
using System.Runtime.CompilerServices;

namespace UITest;

public static class UIAssert
{
    public static void NotExists(AppiumDriver driver, Func<AppiumElement?> accessElement, [CallerArgumentExpression(nameof(accessElement))] string? argumentExpression = null)
    {
        var defaultTimeout = driver.DefaultTimeout;
        driver.DefaultTimeout = TimeSpan.Zero;
        try
        {
            var element = accessElement();
            if (element is null) return;
            if (element.Displayed == false) return;
        }
        catch (NoSuchElementException)
        {
            return;
        }
        catch (StaleElementReferenceException)
        {
            return;
        }
        finally
        {
            driver.DefaultTimeout = defaultTimeout;
        }
        throw new InvalidOperationException($"Element found with '{argumentExpression}'");
    }
}