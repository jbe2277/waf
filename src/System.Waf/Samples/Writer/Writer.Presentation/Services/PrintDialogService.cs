using System.ComponentModel.Composition;
using System.Windows.Controls;
using System.Windows.Documents;
using Waf.Writer.Applications.Services;

namespace Waf.Writer.Presentation.Services
{
    [Export(typeof(IPrintDialogService))]
    internal class PrintDialogService : IPrintDialogService
    {
        private readonly PrintDialog printDialog = new PrintDialog();

        public bool ShowDialog()
        {
            return printDialog.ShowDialog() == true;
        }

        public void PrintDocument(DocumentPaginator documentPaginator, string description)
        {
            printDialog.PrintDocument(documentPaginator, description);
        }
    }
}
