using OpenQA.Selenium.Appium;
using System.Runtime.CompilerServices;

namespace UITest;

public static class UIAssert
{
    public static void NotExists(AppiumDriver driver, Func<AppiumElement?> accessElement, [CallerArgumentExpression(nameof(accessElement))] string? argumentExpression = null)
    {
        var element = driver.TryGet(accessElement);
        if (element is null) return;
        if (element.Displayed == false) return;
        throw new InvalidOperationException($"Element found with '{argumentExpression}'");
    }
}