using FlaUI.Core;
using FlaUI.Core.AutomationElements;

namespace UITest.BookLibrary.Views;

public class ReportView(FrameworkAutomationElementBase element) : AutomationElement(element)
{
    public Button CreateBookListReportButton => this.Find("CreateBookListReportButton").AsButton();

    public Button CreateBorrowedBooksReportButton => this.Find("CreateBorrowedBooksReportButton").AsButton();

    public Button PrintButton => this.Find("PrintButton").AsButton();
}
