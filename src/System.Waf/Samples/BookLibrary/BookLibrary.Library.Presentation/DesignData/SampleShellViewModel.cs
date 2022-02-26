using Waf.BookLibrary.Library.Applications.ViewModels;
using Waf.BookLibrary.Library.Applications.Views;
using Waf.BookLibrary.Library.Presentation.Views;

namespace Waf.BookLibrary.Library.Presentation.DesignData;

public class SampleShellViewModel : ShellViewModel
{
    public SampleShellViewModel() : base(new MockShellView(), null!, new MockShellService(), new MockSettingsService())
    {
        ShellService.BookListView = new BookListView();
        ShellService.BookView = new BookView();
        ShellService.PersonListView = new PersonListView();
        ShellService.PersonView = new PersonView();
    }

    public new string Title => "WAF Book Library";

    private class MockShellView : IShellView
    {
        public object? DataContext { get; set; }

        public double VirtualScreenWidth { get; set; }

        public double VirtualScreenHeight { get; set; }

        public double Left { get; set; }

        public double Top { get; set; }

        public double Width { get; set; }

        public double Height { get; set; }

        public bool IsMaximized { get; set; }

        public event CancelEventHandler? Closing;

        public event EventHandler? Closed;

        public void Show() { }

        public void Close() { }

        protected virtual void OnClosing(CancelEventArgs e) => Closing?.Invoke(this, e);

        protected virtual void OnClosed(EventArgs e) => Closed?.Invoke(this, e);
    }
}
