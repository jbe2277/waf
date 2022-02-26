using Waf.BookLibrary.Reporting.Applications.ViewModels;
using Waf.BookLibrary.Reporting.Applications.Views;
using Waf.BookLibrary.Reporting.Presentation.Reports;

namespace Waf.BookLibrary.Reporting.Presentation.DesignData;

public class SampleReportViewModel : ReportViewModel
{
    public SampleReportViewModel() : base(new MockReportView())
    {
        var reportControl = new BookListReport
        {
            ReportData = new SampleBookListReportDataModel()
        };
        Report = reportControl.Report;
    }


    private class MockReportView : IReportView
    {
        public object? DataContext { get; set; }
    }
}
