using System.Waf.Applications;
using Waf.BookLibrary.Library.Applications.Services;
using Waf.BookLibrary.Reporting.Applications.DataModels;
using Waf.BookLibrary.Reporting.Applications.Reports;
using Waf.BookLibrary.Reporting.Applications.ViewModels;

namespace Waf.BookLibrary.Reporting.Applications.Controllers;

internal class ModuleController : IModuleController
{
    private readonly IShellService shellService;
    private readonly IEntityService entityService;
    private readonly Lazy<ReportViewModel> reportViewModel;
    private readonly Func<IBookListReport> bookListReportFactory;
    private readonly Func<IBorrowedBooksReport> borrowedBooksReportFactory;
    private readonly DelegateCommand createBookListReportCommand;
    private readonly DelegateCommand createBorrowedBooksReportCommand;

    public ModuleController(IShellService shellService, IEntityService entityService, Lazy<ReportViewModel> reportViewModel,
        Func<IBookListReport> bookListReportFactory, Func<IBorrowedBooksReport> borrowedBooksReportFactory)
    {
        this.shellService = shellService;
        this.entityService = entityService;
        this.reportViewModel = reportViewModel;
        this.bookListReportFactory = bookListReportFactory;
        this.borrowedBooksReportFactory = borrowedBooksReportFactory;
        createBookListReportCommand = new(CreateBookListReport);
        createBorrowedBooksReportCommand = new(CreateBorrowedBooksReport);
    }

    private ReportViewModel ReportViewModel => reportViewModel.Value;

    public void Initialize()
    {
        shellService.IsReportingEnabled = true;
        shellService.LazyReportingView = new(InitializeReportView);
    }

    public void Run() { }

    public void Shutdown() { }

    private object InitializeReportView()
    {
        ReportViewModel.CreateBookListReportCommand = createBookListReportCommand;
        ReportViewModel.CreateBorrowedBooksReportCommand = createBorrowedBooksReportCommand;
        return ReportViewModel.View;
    }

    private void CreateBookListReport()
    {
        Log.Default.Info("Create book list report");
        var bookListReport = bookListReportFactory();
        bookListReport.ReportData = new BookListReportDataModel(entityService.Books);
        ReportViewModel.Report = bookListReport.Report;
    }

    private void CreateBorrowedBooksReport()
    {
        Log.Default.Info("Create borrowed books report");
        var borrowedBooksReport = borrowedBooksReportFactory();
        borrowedBooksReport.ReportData = new BorrowedBooksReportDataModel(entityService.Books);
        ReportViewModel.Report = borrowedBooksReport.Report;
    }
}
