using System.ComponentModel.Composition;
using Waf.BookLibrary.Reporting.Applications.Reports;

namespace Test.BookLibrary.Reporting.Applications.Reports
{
    [Export(typeof(IBookListReport)), PartCreationPolicy(CreationPolicy.NonShared)]
    public class MockBookListReport : MockReport, IBookListReport
    {
    }
}
