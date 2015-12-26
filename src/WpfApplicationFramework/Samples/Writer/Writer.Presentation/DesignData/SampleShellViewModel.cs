using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Waf.Writer.Applications.ViewModels;
using Waf.Writer.Applications.Views;
using System.ComponentModel;
using Waf.Writer.Applications.Services;
using System.Waf.Foundation;

namespace Waf.Writer.Presentation.DesignData
{
    public class SampleShellViewModel : ShellViewModel
    {
        public SampleShellViewModel() : base(new MockShellView(), null, null, new MockShellService(), new MockFileService())
        {
        }

        public new string Title { get { return "WAF Writer (Design Time)"; } }



        private class MockShellView : MockView, IShellView
        {
            public double Left { get; set; }

            public double Top { get; set; }

            public double Width { get; set; }

            public double Height { get; set; }

            public bool IsMaximized { get; set; }

            public event CancelEventHandler Closing;

            public event EventHandler Closed;

            public void Show() { }
            
            public void Close() { }

            protected virtual void OnClosing(CancelEventArgs e)
            {
                if (Closing != null) { Closing(this, e); }
            }

            protected virtual void OnClosed(EventArgs e)
            {
                if (Closed != null) { Closed(this, e); }
            }
        }
    }
}
