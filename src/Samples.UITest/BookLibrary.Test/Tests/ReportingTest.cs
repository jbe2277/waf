using FlaUI.Core.AutomationElements;
using FlaUI.Core.Capturing;
using FlaUI.Core.Definitions;
using FlaUI.Core.Tools;
using UITest.SystemViews;
using Xunit;

namespace UITest.BookLibrary.Tests;

public class ReportingTest() : UITest()
{
    [Fact]
    public void CreateReportTest()
    {
        Launch();
        var window = GetShellWindow();
        window.SetState(WindowVisualState.Maximized);
        window.TabControl.ReportingTabItem.Select();
        var reportView = window.TabControl.ReportingTabItem.ReportView;

        Assert.False(reportView.PrintButton.IsEnabled);  // No report available
        
        reportView.CreateBookListReportButton.Click();
        Thread.Sleep(500);
        Assert.True(reportView.PrintButton.IsEnabled);
        Capture.Screen().ToFile(GetScreenshotFile("BookListReport"));
        PrintAsPdf(GetScreenshotFile("BookListReport.pdf"));

        reportView.CreateBorrowedBooksReportButton.Click();
        Thread.Sleep(500);
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
            bool printCompleted = false;
            if (Environment.OSVersion.Version >= version)
            {
                var printDialog = PrintDialog.TryGetDialog(Automation);
                if (printDialog is not null)  // Windows 2025 Server uses legacy print dialog
                {
                    printDialog.PrinterSelector.Select(printDialog.PrintToPdf.Name);
                    Retry.WhileFalse(() => printDialog.PrintButton.IsEnabled, throwOnTimeout: true);
                    printDialog.PrintButton.Invoke();
                    printCompleted = true;
                }
            }
            if (!printCompleted)
            {
                var printDialog = window.FirstModalWindow().As<LegacyPrintDialog>();
                Log.WriteLine("Printers:");
                foreach (var x in printDialog.PrinterList.Items) Log.WriteLine($"- {x.Text}");
                printDialog.PrinterList.Select(printDialog.PrintToPdf.Text);
                printDialog.PrintButton.Click();
            }

            var saveFileDialog = window.FirstModalWindow().As<SaveFileDialog>();
            saveFileDialog.SetFileName(fileName);
            saveFileDialog.SaveButton.Click();

            // Wait until the button is enabled again -> indication that the PDF print is completed
            Retry.WhileFalse(() => reportView.PrintButton.IsEnabled, throwOnTimeout: true);
        }
    }
}