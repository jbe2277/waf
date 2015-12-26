using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Waf.BookLibrary.Reporting.Applications.Views;
using System.ComponentModel.Composition;

namespace Waf.BookLibrary.Reporting.Presentation.Views
{
    [Export(typeof(IReportView))]
    public partial class ReportView : UserControl, IReportView
    {
        public ReportView()
        {
            InitializeComponent();
            Loaded += LoadedHandler;
        }


        private void LoadedHandler(object sender, RoutedEventArgs e)
        {
            createButton.Focus();
        }
    }
}
