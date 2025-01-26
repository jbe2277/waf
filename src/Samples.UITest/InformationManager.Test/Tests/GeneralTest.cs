using FlaUI.Core.AutomationElements;
using FlaUI.Core.Capturing;
using UITest.SystemViews;
using Xunit;

namespace UITest.InformationManager.Tests;

public class GeneralTest() : UITest()
{
    [Fact]
    public void AboutTest()
    {
        Launch();
        var window = GetShellWindow();
        window.AboutButton.Click();

        var messageBox = window.FirstModalWindow().As<MessageBox>();
        Assert.Equal("Waf Information Manager", messageBox.Title);
        Log.WriteLine(messageBox.Message);
        Assert.StartsWith("Waf Information Manager ", messageBox.Message);
        Capture.Screen().ToFile(GetScreenshotFile("About"));
        messageBox.Buttons[0].Click();

        window.ExitButton.Click();
    }
}
