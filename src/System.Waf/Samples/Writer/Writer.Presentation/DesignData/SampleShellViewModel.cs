using System;
using Waf.Writer.Applications.ViewModels;
using Waf.Writer.Applications.Views;
using System.ComponentModel;
using Waf.Writer.Presentation.Views;

namespace Waf.Writer.Presentation.DesignData
{
    public class SampleShellViewModel : ShellViewModel
    {
        public SampleShellViewModel() : base(new MockShellView(), null!, null!, new MockShellService(), new MockFileService(), new MockSettingsService())
        {
            ContentView = new SampleMainViewModel(new MainView()).View;
        }
        

        private class MockShellView : MockView, IShellView
        {
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
}
