using System.Waf.Applications;
using System.Waf.UnitTesting;
using System.Windows.Input;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Waf.BookLibrary.Reporting.Applications.ViewModels;

namespace Test.BookLibrary.Reporting.Applications.ViewModels
{
    [TestClass]
    public class ReportViewModelTest : ReportingTest
    {
        [TestMethod]
        public void PropertiesTest()
        {
            ReportViewModel reportViewModel = Container.GetExportedValue<ReportViewModel>();

            var report = new object();
            AssertHelper.PropertyChangedEvent(reportViewModel, x => x.Report, () => reportViewModel.Report = report);
            Assert.AreEqual(report, reportViewModel.Report);

            ICommand emptyCommand = new DelegateCommand(() => { });
            AssertHelper.PropertyChangedEvent(reportViewModel, x => x.CreateBookListReportCommand, 
                () => reportViewModel.CreateBookListReportCommand = emptyCommand);
            Assert.AreEqual(emptyCommand, reportViewModel.CreateBookListReportCommand);

            AssertHelper.PropertyChangedEvent(reportViewModel, x => x.CreateBorrowedBooksReportCommand,
                () => reportViewModel.CreateBorrowedBooksReportCommand = emptyCommand);
            Assert.AreEqual(emptyCommand, reportViewModel.CreateBorrowedBooksReportCommand);
        }
    }
}
