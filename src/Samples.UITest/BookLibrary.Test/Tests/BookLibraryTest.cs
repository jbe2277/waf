using FlaUI.Core.AutomationElements;
using FlaUI.Core.Capturing;
using UITest.SystemViews;
using Xunit;
using Xunit.Abstractions;

namespace UITest.BookLibrary.Tests;

public class BookLibraryTest(ITestOutputHelper log) : UITest(log)
{
    [Fact]
    public void AboutTest()
    {
        Launch();
        var window = GetShellWindow();

        var helpMenu = window.HelpMenu;
        helpMenu.Click();
        helpMenu.AboutMenuItem.Click();

        var messageBox = window.FirstModalWindow().As<MessageBox>();
        Assert.Equal("Waf Book Library", messageBox.Title);
        Log.WriteLine(messageBox.Message);
        Assert.StartsWith("Waf Book Library ", messageBox.Message);
        Capture.Screen().ToFile(GetScreenshotFile("About.png"));
        messageBox.OkButton.Click();

        var dataMenu = window.DataMenu;
        dataMenu.Click();
        dataMenu.ExitMenuItem.Click();
    }
}
