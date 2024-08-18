using FlaUI.Core.AutomationElements;
using FlaUI.Core.Capturing;
using UITest.SystemViews;
using Xunit;
using Xunit.Abstractions;

namespace UITest.InformationManager.Tests;

public class GeneralTest(ITestOutputHelper log) : UITest(log)
{
    [Fact]
    public void AboutTest() => Run(() =>
    {
        Launch();
        var window = GetShellWindow();
        window.AboutCommand.Click();

        var messageBox = window.FirstModalWindow().As<MessageBox>();
        Assert.Equal("Waf Information Manager", messageBox.Title);
        Log.WriteLine(messageBox.Message);
        Assert.StartsWith("Waf Information Manager ", messageBox.Message);
        Capture.Screen().ToFile(GetScreenshotFile("About"));
        messageBox.Buttons[0].Click();

        window.ExitCommand.Click();
    });
}
