using FlaUI.Core.Capturing;
using FlaUI.Core.Definitions;
using Xunit;
using Xunit.Abstractions;

namespace UITest.BookLibrary.Tests;

public class ReportingTest(ITestOutputHelper log) : UITest(log)
{
    [Fact]
    public void CreateReportTest() => Run(() =>
    {
        Launch();
        var window = GetShellWindow();
        window.SetState(WindowVisualState.Maximized);
        window.TabControl.ReportingTabItem.Select();
        var reportView = window.TabControl.ReportingTabItem.ReportView;

        Assert.False(reportView.PrintButton.IsEnabled);  // No report available
        
        reportView.CreateBookListReportButton.Click();
        Assert.True(reportView.PrintButton.IsEnabled);
        Capture.Screen().ToFile(GetScreenshotFile("BookListReport"));

        reportView.CreateBorrowedBooksReportButton.Click();
        Capture.Screen().ToFile(GetScreenshotFile("BorrowedBooksReport"));

        window.Close();
    });
}