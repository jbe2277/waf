using Waf.BookLibrary.Reporting.Applications.Reports;

namespace Waf.BookLibrary.Reporting.Presentation.Reports;

public partial class BorrowedBooksReport : IBorrowedBooksReport
{
    public BorrowedBooksReport()
    {
        InitializeComponent();
        // Disconnect the flowDocument from the UserControl. The UserControl is only used for DesignTime support.
        Content = null;
    }

    public object Report => flowDocument;

    public object? ReportData
    {
        get;
        set
        {
            field = value;
            flowDocument.DataContext = value;
        }
    }
}
