using Waf.Writer.Applications.ViewModels;
using Waf.Writer.Applications.Views;
using Waf.Writer.Presentation.Views;

namespace Waf.Writer.Presentation.DesignData;

public class SampleShellViewModel : ShellViewModel
{
    public SampleShellViewModel() : base(new MockShellView(), null!, new MockShellService(), new MockFileService(), new MockSettingsService())
    {
        ContentView = new SampleMainViewModel(new MainView()).View;
    }


    private sealed class MockShellView : MockView, IShellView
    {
        public double VirtualScreenLeft { get; set; }

        public double VirtualScreenTop { get; set; }

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

        public void RaiseClosing(CancelEventArgs e) => Closing?.Invoke(this, e);

        public void RaiseClosed() => Closed?.Invoke(this, EventArgs.Empty);
    }
}
