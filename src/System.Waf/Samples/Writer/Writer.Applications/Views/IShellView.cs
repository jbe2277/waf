using System;
using System.Waf.Applications;
using System.ComponentModel;

namespace Waf.Writer.Applications.Views
{
    public interface IShellView : IView
    {
        double Left { get; set; }
        
        double Top { get; set; }

        double Width { get; set; }

        double Height { get; set; }

        bool IsMaximized { get; set; }

        
        event CancelEventHandler? Closing;

        event EventHandler? Closed;

        
        void Show();

        void Close();
    }
}
