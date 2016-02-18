using System.ComponentModel.Composition;
using Waf.BookLibrary.Reporting.Applications.Reports;

namespace Test.BookLibrary.Reporting.Applications.Reports
{
    [Export(typeof(IBorrowedBooksReport)), PartCreationPolicy(CreationPolicy.NonShared)]
    public class MockBorrowedBooksReport : MockReport, IBorrowedBooksReport
    {
    }
}
