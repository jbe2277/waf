using Waf.BookLibrary.Reporting.Applications.Reports;
using System.ComponentModel.Composition;

namespace Waf.BookLibrary.Reporting.Presentation.Reports
{
    [Export(typeof(IBorrowedBooksReport)), PartCreationPolicy(CreationPolicy.NonShared)]
    public partial class BorrowedBooksReport : IBorrowedBooksReport
    {
        private object? reportData;

        public BorrowedBooksReport()
        {
            InitializeComponent();
            // Disconnect the flowDocument from the UserControl. The UserControl is only used for DesignTime support.
            Content = null;
        }

        public object Report => flowDocument;

        public object? ReportData
        {
            get => reportData;
            set
            {
                reportData = value;
                flowDocument.DataContext = value;
            }
        }
    }
}
