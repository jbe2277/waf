﻿using System.Diagnostics;
using System.Waf.Applications;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using Waf.Writer.Applications.Views;

namespace Waf.Writer.Presentation.Views;

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

    private static void ShowLog()
    {
        try
        {
            Process.Start(new ProcessStartInfo(App.LogFileName) { UseShellExecute = true });
        }
        catch (Exception ex)
        {
            Log.Default.Error(ex, "ShowLog");
        }
    }

    private void ZoomBoxDropDownClosedHandler(object sender, EventArgs e) => BindingOperations.GetBindingExpression(zoomBox, ComboBox.TextProperty).UpdateSource();

    private void ZoomBoxKeyDownHandler(object sender, KeyEventArgs e)
    {
        if (e.Key == Key.Return) BindingOperations.GetBindingExpression(zoomBox, ComboBox.TextProperty).UpdateSource();
    }
}
