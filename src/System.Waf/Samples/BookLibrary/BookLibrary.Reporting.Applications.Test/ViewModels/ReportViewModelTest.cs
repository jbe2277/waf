using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Waf.UnitTesting;
using Waf.BookLibrary.Reporting.Applications.ViewModels;

namespace Test.BookLibrary.Reporting.Applications.ViewModels;

[TestClass]
public class ReportViewModelTest : ReportingTest
{
    [TestMethod]
    public void PropertiesTest()
    {
        var reportViewModel = Get<ReportViewModel>();
        var report = new object();
        AssertHelper.PropertyChangedEvent(reportViewModel, x => x.Report, () => reportViewModel.Report = report);
        Assert.AreEqual(report, reportViewModel.Report);
    }
}
