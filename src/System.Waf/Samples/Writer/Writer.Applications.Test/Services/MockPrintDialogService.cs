using System.ComponentModel.Composition;
using System.Windows.Documents;
using Waf.Writer.Applications.Services;

namespace Test.Writer.Applications.Services;

[Export(typeof(IPrintDialogService)), Export]
public class MockPrintDialogService : IPrintDialogService
{
    public bool ShowDialogResult { get; set; }

    public DocumentPaginator? DocumentPaginator { get; private set; }

    public string? Description { get; private set; }

    public bool ShowDialog()
    {
        DocumentPaginator = null;
        Description = null;
        return ShowDialogResult;
    }

    public void PrintDocument(DocumentPaginator documentPaginator, string description)
    {
        DocumentPaginator = documentPaginator;
        Description = description;
    }
}
