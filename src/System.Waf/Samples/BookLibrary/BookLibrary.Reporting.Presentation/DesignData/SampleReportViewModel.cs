using Waf.BookLibrary.Reporting.Applications.ViewModels;
using Waf.BookLibrary.Reporting.Applications.Views;

namespace Waf.BookLibrary.Reporting.Presentation.DesignData
{
    public class SampleReportViewModel : ReportViewModel
    {
        public SampleReportViewModel() : base(new MockReportView())
        {
        }


        private class MockReportView : IReportView
        {
            public object DataContext { get; set; }
        }
    }
}
