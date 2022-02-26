namespace Waf.BookLibrary.Library.Applications.Services
{
    public interface IShellService : INotifyPropertyChanged
    {
        object? ShellView { get; }

        object? BookListView { get; set; }

        object? BookView { get; set; }

        object? PersonListView { get; set; }

        object? PersonView { get; set; }

        bool IsReportingEnabled { get; set; }

        Lazy<object>? LazyReportingView { get; set; }
    }
}
