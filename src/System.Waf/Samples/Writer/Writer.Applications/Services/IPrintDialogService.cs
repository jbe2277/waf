namespace Waf.Writer.Applications.Services;

public interface IPrintDialogService
{
    bool ShowDialog();

    void PrintDocument(object documentPaginatorSource, string description);
}
