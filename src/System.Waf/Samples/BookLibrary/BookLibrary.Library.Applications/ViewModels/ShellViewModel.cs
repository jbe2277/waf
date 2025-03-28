﻿using System.Waf.Applications;
using System.Waf.Applications.Services;
using System.Windows.Input;
using Waf.BookLibrary.Library.Applications.Properties;
using Waf.BookLibrary.Library.Applications.Services;
using Waf.BookLibrary.Library.Applications.Views;

namespace Waf.BookLibrary.Library.Applications.ViewModels;

public class ShellViewModel : ViewModel<IShellView>
{
    private readonly IMessageService messageService;
    private readonly AppSettings settings;
    private bool isValid = true;

    public ShellViewModel(IShellView view, IMessageService messageService, IShellService shellService, ISettingsService settingsService) : base(view)
    {
        this.messageService = messageService;
        ShellService = shellService;
        AboutCommand = new DelegateCommand(ShowAboutMessage);
        settings = settingsService.Get<AppSettings>();
        view.Closing += ViewClosing;
        view.Closed += ViewClosed;

        // Restore the window size when the values are valid.
        if (settings.Left >= view.VirtualScreenLeft && settings.Top >= view.VirtualScreenTop
            && settings.Width > 0 && settings.Left + settings.Width <= view.VirtualScreenLeft + view.VirtualScreenWidth
            && settings.Height > 0 && settings.Top + settings.Height <= view.VirtualScreenTop + view.VirtualScreenHeight)
        {
            view.Left = settings.Left;
            view.Top = settings.Top;
            view.Height = settings.Height;
            view.Width = settings.Width;
        }
        view.IsMaximized = settings.IsMaximized;
    }

    public string Title => ApplicationInfo.ProductName;

    public IShellService ShellService { get; }

    public ICommand AboutCommand { get; }

    public ICommand? SaveCommand { get; set; }

    public ICommand? ExitCommand { get; set; }

    public bool IsValid
    {
        get => isValid;
        set => SetProperty(ref isValid, value);
    }

    public string DatabasePath { get; set; } = Resources.NotAvailable;

    public event CancelEventHandler? Closing;

    public void Show() => ViewCore.Show();

    public void Close() => ViewCore.Close();

    protected virtual void OnClosing(CancelEventArgs e) => Closing?.Invoke(this, e);

    private void ViewClosing(object? sender, CancelEventArgs e) => OnClosing(e);

    private void ViewClosed(object? sender, EventArgs e)
    {
        settings.Left = ViewCore.Left;
        settings.Top = ViewCore.Top;
        settings.Height = ViewCore.Height;
        settings.Width = ViewCore.Width;
        settings.IsMaximized = ViewCore.IsMaximized;
    }

    private void ShowAboutMessage() => messageService.ShowMessage(View, Resources.AboutText, ApplicationInfo.ProductName, ApplicationInfo.Version, Environment.Version);
}
