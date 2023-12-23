using System.ComponentModel.Composition;
using System.Diagnostics;
using System.Waf.Applications;
using System.Windows;
using Waf.BookLibrary.Library.Applications.Views;

namespace Waf.BookLibrary.Library.Presentation.Views;

[Export(typeof(IShellView))]
public partial class ShellWindow : IShellView
{
    public ShellWindow()
    {
        InitializeComponent();
        showLogKeyBinding.Command = new DelegateCommand(ShowLog);
    }

    public double VirtualScreenLeft => SystemParameters.VirtualScreenLeft;

    public double VirtualScreenTop => SystemParameters.VirtualScreenTop;

    public double VirtualScreenWidth => SystemParameters.VirtualScreenWidth;

    public double VirtualScreenHeight => SystemParameters.VirtualScreenHeight;

    public bool IsMaximized
    {
        get => WindowState == WindowState.Maximized;
        set
        {
            if (value)
            {
                WindowState = WindowState.Maximized;
            }
            else if (WindowState == WindowState.Maximized)
            {
                WindowState = WindowState.Normal;
            }
        }
    }

    public static void ShowLog()
    {
        try
        {
            Process.Start(new ProcessStartInfo(App.LogFileName) { UseShellExecute = true });
        }
        catch (Exception exception)
        {
            Log.Default.Error(exception, "ShowLog");
        }
    }
}
