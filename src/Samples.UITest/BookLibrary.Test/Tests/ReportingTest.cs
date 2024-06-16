﻿using FlaUI.Core.AutomationElements;
using FlaUI.Core.Capturing;
using FlaUI.Core.Definitions;
using FlaUI.Core.Tools;
using UITest.SystemViews;
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
        PrintAsPdf(GetScreenshotFile("BookListReport.pdf"));

        reportView.CreateBorrowedBooksReportButton.Click();
        Capture.Screen().ToFile(GetScreenshotFile("BorrowedBooksReport"));
        PrintAsPdf(GetScreenshotFile("BorrowedBooksReport.pdf"));

        var dataMenu = window.DataMenu;
        dataMenu.Click();
        dataMenu.ExitMenuItem.Click();


        void PrintAsPdf(string fileName)
        {
            if (File.Exists(fileName)) File.Delete(fileName);

            reportView.PrintButton.Click();
            var version = new Version(10, 0, 22621, 0);  // Windows 11 22H2
            if (Environment.OSVersion.Version >= version)
            {
                var printDialog = PrintDialog.GetDialog(Automation);
                printDialog.PrinterSelector.Select(printDialog.PrintToPdf.Name);
                Retry.WhileFalse(() => printDialog.PrintButton.IsEnabled, throwOnTimeout: true);
                printDialog.PrintButton.Invoke();
            }
            else
            {
                var printDialog = window.FirstModalWindow().As<LegacyPrintDialog>();
                printDialog.PrintButton.Click();
                Thread.Sleep(1000);
            }

            var saveFileDialog = window.FirstModalWindow().As<SaveFileDialog>();
            saveFileDialog.SetFileName(fileName);
            saveFileDialog.SaveButton.Click();
        }
    });
}