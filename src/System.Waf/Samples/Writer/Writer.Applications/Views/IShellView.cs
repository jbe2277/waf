﻿using System.Waf.Applications;

namespace Waf.Writer.Applications.Views;

public interface IShellView : IView
{
    double VirtualScreenLeft { get; }

    double VirtualScreenTop { get; }

    double VirtualScreenWidth { get; }

    double VirtualScreenHeight { get; }

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
