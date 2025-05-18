using OpenQA.Selenium.Appium;

namespace UITest.NewsReader;

public class UITest() : UITestBase("Waf.NewsReader_a8txtqew917ny!App",
        Environment.GetEnvironmentVariable("UITestOutputPath") ?? "out/NewsReader.UITest/")
{
    public AppiumElement GetShellWindow() => Driver.FindElement(MobileBy.ClassName("WinUIDesktopWin32WindowClass")) ?? throw new InvalidOperationException("MainWindow not found");
}
