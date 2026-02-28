using System.Waf.Applications;
using System.Windows.Input;
using Waf.BookLibrary.Reporting.Applications.Views;

namespace Waf.BookLibrary.Reporting.Applications.ViewModels;

public class ReportViewModel(IReportView view) : ViewModel<IReportView>(view)
{
    public object? Report { get; set => SetProperty(ref field, value); }

    public ICommand? CreateBookListReportCommand { get; set; }

    public ICommand? CreateBorrowedBooksReportCommand { get; set; }
}
