using System.Windows;
using Waf.BookLibrary.Reporting.Applications.Views;

namespace Waf.BookLibrary.Reporting.Presentation.Views;

public partial class ReportView : IReportView
{
    public ReportView()
    {
        InitializeComponent();
        Loaded += LoadedHandler;
    }

    private void LoadedHandler(object sender, RoutedEventArgs e) => createButton.Focus();
}
