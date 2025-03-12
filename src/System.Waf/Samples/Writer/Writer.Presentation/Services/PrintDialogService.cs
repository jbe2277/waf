using System.Windows.Controls;
using System.Windows.Documents;
using Waf.Writer.Applications.Services;

namespace Waf.Writer.Presentation.Services;

internal sealed class PrintDialogService : IPrintDialogService
{
    private readonly PrintDialog printDialog = new();

    public bool ShowDialog() => printDialog.ShowDialog() == true;

    public void PrintDocument(object documentPaginatorSource, string description) => printDialog.PrintDocument(((IDocumentPaginatorSource)documentPaginatorSource).DocumentPaginator, description);
}
