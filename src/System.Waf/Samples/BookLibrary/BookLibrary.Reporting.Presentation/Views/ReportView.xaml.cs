using System.Windows;
using Waf.BookLibrary.Reporting.Applications.Views;
using System.ComponentModel.Composition;

namespace Waf.BookLibrary.Reporting.Presentation.Views
{
    [Export(typeof(IReportView))]
    public partial class ReportView : IReportView
    {
        public ReportView()
        {
            InitializeComponent();
            Loaded += LoadedHandler;
        }

        private void LoadedHandler(object sender, RoutedEventArgs e) => createButton.Focus();
    }
}
