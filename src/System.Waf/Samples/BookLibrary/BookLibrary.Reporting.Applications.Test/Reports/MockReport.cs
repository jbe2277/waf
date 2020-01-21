using Waf.BookLibrary.Reporting.Applications.Reports;

namespace Test.BookLibrary.Reporting.Applications.Reports
{
    public class MockReport : IReport
    {
        public object Report => this;

        public object? ReportData { get; set; }
    }
}
