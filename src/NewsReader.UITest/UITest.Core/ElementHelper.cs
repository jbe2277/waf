using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Enums;
using OpenQA.Selenium.Interactions;
using System.Drawing;

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

    public static void SafeClick(this AppiumElement element, bool isRightButton = false)
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
            d.ExecuteScript("windows: click", new Dictionary<string, object>()
            {
                ["x"] = windowPos.X + center.X,
                ["y"] = windowPos.Y + center.Y,
                ["button"] = isRightButton ? "right" : "left"
            });
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
                Thread.Sleep(200);
            }
        }
        else element.SendKeys(text);
    }

    public static void SwipeRight(this AppiumElement element)
    {
        const int xOffset = 40;
        var startPoint = new Point(xOffset + element.Rect.Left, element.Rect.Top + element.Rect.Height / 2);
        var endPoint = new Point(-xOffset + element.Rect.Left + element.Rect.Width, startPoint.Y);
        Log.WriteLine($"Swipe from {startPoint} to {endPoint} for Element (Rect: {element.Rect})");
        var input = new PointerInputDevice(PointerKind.Touch);
        var swipe = new ActionSequence(input);
        swipe.AddAction(input.CreatePointerMove(CoordinateOrigin.Viewport, startPoint.X, startPoint.Y, TimeSpan.Zero));
        swipe.AddAction(input.CreatePointerDown(MouseButton.Left));
        swipe.AddAction(input.CreatePointerMove(CoordinateOrigin.Viewport, endPoint.X, endPoint.Y, TimeSpan.FromSeconds(1)));
        swipe.AddAction(input.CreatePointerUp(MouseButton.Left));
        element.GetDriver().PerformActions([swipe]);
    }
}
