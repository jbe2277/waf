using Waf.Writer.Applications.Services;

namespace Test.Writer.Applications.Services;

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
