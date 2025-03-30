﻿using System.Diagnostics;
using System.Waf.Applications;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using Waf.InformationManager.Common.Applications.Services;
using Waf.InformationManager.Infrastructure.Interfaces.Applications;
using Waf.InformationManager.Infrastructure.Modules.Applications.ViewModels;
using Waf.InformationManager.Infrastructure.Modules.Applications.Views;

namespace Waf.InformationManager.Infrastructure.Modules.Presentation.Views;

public partial class ShellWindow : IShellView
{
    private readonly Lazy<ShellViewModel> viewModel;
    private readonly List<Control> dynamicToolBarItems = [];

    public ShellWindow()
    {
        InitializeComponent();
        showLogKeyBinding.Command = new DelegateCommand(ShowLog);
        viewModel = new(() => this.GetViewModel<ShellViewModel>()!);
        Loaded += LoadedHandler;
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

    private ShellViewModel ViewModel => viewModel.Value;

    public void AddToolBarCommands(IReadOnlyList<ToolBarCommand> commands)
    {
        int index = 0;
        foreach (var command in commands)
        {
            var accessText = new AccessText() { Text = command.Text };
            var button = new Button() { Content = accessText, Command = command.Command };
            AutomationProperties.SetAutomationId(button, command.AutomationId);
            if (!string.IsNullOrEmpty(command.ToolTip)) button.ToolTip = command.ToolTip;

            toolBar.Items.Insert(index, button);
            dynamicToolBarItems.Add(button);
            index += 1;
        }
    }

    public void ClearToolBarCommands()
    {
        dynamicToolBarItems.ForEach(toolBar.Items.Remove);
        dynamicToolBarItems.Clear();
    }

    private void LoadedHandler(object sender, RoutedEventArgs e)
    {
        var firstNode = ViewModel.NavigationService.NavigationNodes.FirstOrDefault();
        if (firstNode != null) firstNode.IsSelected = true;
    }

    private static void ShowLog()
    {
        try
        {
            Process.Start(new ProcessStartInfo(AppInfo.LogFileName) { UseShellExecute = true });
        }
        catch (Exception ex)
        {
            Log.Default.Error(ex, "ShowLog");
        }
    }
}
