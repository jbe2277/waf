using Microsoft.VisualStudio.TestTools.UnitTesting;
using Test.BookLibrary.Reporting.Applications.Reports;
using Waf.BookLibrary.Library.Applications.Services;
using Waf.BookLibrary.Reporting.Applications.Controllers;
using Waf.BookLibrary.Reporting.Applications.DataModels;
using Waf.BookLibrary.Reporting.Applications.ViewModels;
using Waf.BookLibrary.Reporting.Applications.Views;

namespace Test.BookLibrary.Reporting.Applications.Controllers
{
    [TestClass]
    public class ModuleControllerTest : ReportingTest
    {
        private ModuleController InitializeModuleController()
        {
            var moduleController = Container.GetExportedValue<ModuleController>();
            moduleController.Initialize();
            moduleController.Run();

            var shellService = Container.GetExportedValue<IShellService>();
            Assert.IsTrue(shellService.IsReportingEnabled);

            var reportView = Container.GetExportedValue<IReportView>();
            Assert.AreEqual(reportView, shellService.LazyReportingView.Value);

            return moduleController;
        }
        

        [TestMethod]
        public void CreateBookListReportTest()
        {
            var moduleController = InitializeModuleController();

            var reportViewModel = Container.GetExportedValue<ReportViewModel>();
            reportViewModel.CreateBookListReportCommand.Execute(null);

            var bookListReport = (MockBookListReport)reportViewModel.Report;
            var bookListReportDataModel = (BookListReportDataModel)bookListReport.ReportData;

            Assert.IsNotNull(bookListReportDataModel.Books);
            Assert.AreEqual(0, bookListReportDataModel.BookCount);

            moduleController.Shutdown();
        }

        [TestMethod]
        public void CreateBorrowedBooksReportTest()
        {
            var moduleController = InitializeModuleController();

            var reportViewModel = Container.GetExportedValue<ReportViewModel>();
            reportViewModel.CreateBorrowedBooksReportCommand.Execute(null);

            var bookListReport = (MockBorrowedBooksReport)reportViewModel.Report;
            var bookListReportDataModel = (BorrowedBooksReportDataModel)bookListReport.ReportData;

            Assert.IsNotNull(bookListReportDataModel.GroupedBooks);
            Assert.AreEqual(0, bookListReportDataModel.GroupedBooks.Count);

            moduleController.Shutdown();
        }
    }
}
