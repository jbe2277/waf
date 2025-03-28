namespace Waf.BookLibrary.Library.Applications.Services;

internal class ShellService : Model, IShellService
{
    public object? ShellView { get; set; }

    public object? BookListView { get; set; }

    public object? BookView { get; set; }

    public object? PersonListView { get; set; }

    public object? PersonView { get; set; }

    public bool IsReportingEnabled { get; set; }

    public Lazy<object>? LazyReportingView { get; set; }
}
