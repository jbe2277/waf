using System.ComponentModel.Composition;
using System.Waf.UnitTesting.Mocks;
using Waf.BookLibrary.Reporting.Applications.Views;

namespace Test.BookLibrary.Reporting.Applications.Views;

[Export(typeof(IReportView))]
public class MockReportView : MockView, IReportView
{
}
