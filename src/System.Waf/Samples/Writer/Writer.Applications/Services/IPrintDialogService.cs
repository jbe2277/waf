using System.Windows.Documents;

namespace Waf.Writer.Applications.Services;

public interface IPrintDialogService
{
    bool ShowDialog();

    void PrintDocument(DocumentPaginator documentPaginator, string description);
}
