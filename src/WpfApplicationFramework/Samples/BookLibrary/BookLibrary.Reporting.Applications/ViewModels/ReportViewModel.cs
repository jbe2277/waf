using System.ComponentModel.Composition;
using System.Waf.Applications;
using System.Windows.Input;
using Waf.BookLibrary.Reporting.Applications.Views;

namespace Waf.BookLibrary.Reporting.Applications.ViewModels
{
    [Export]
    public class ReportViewModel : ViewModel<IReportView>
    {
        private object report;
        private ICommand createBookListReportCommand;
        private ICommand createBorrowedBooksReportCommand;


        [ImportingConstructor]
        public ReportViewModel(IReportView view) : base(view)
        {
        }


        public object Report
        {
            get { return report; }
            set { SetProperty(ref report, value); }
        }

        public ICommand CreateBookListReportCommand
        {
            get { return createBookListReportCommand; }
            set { SetProperty(ref createBookListReportCommand, value); }
        }
        
        public ICommand CreateBorrowedBooksReportCommand
        {
            get { return createBorrowedBooksReportCommand; }
            set { SetProperty(ref createBorrowedBooksReportCommand, value); }
        }
    }
}
