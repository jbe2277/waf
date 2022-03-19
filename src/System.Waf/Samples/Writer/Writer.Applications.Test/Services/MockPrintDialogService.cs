using System.ComponentModel.Composition;
using Waf.Writer.Applications.Services;

namespace Test.Writer.Applications.Services;

[Export(typeof(IPrintDialogService)), Export]
public class MockPrintDialogService : IPrintDialogService
{
    public bool ShowDialogResult { get; set; }

    public object? DocumentPaginatorSource { get; private set; }

    public string? Description { get; private set; }

    public bool ShowDialog()
    {
        DocumentPaginatorSource = null;
        Description = null;
        return ShowDialogResult;
    }

    public void PrintDocument(object documentPaginatorSource, string description)
    {
        DocumentPaginatorSource = documentPaginatorSource;
        Description = description;
    }
}
