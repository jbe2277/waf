using System.Waf.Applications;
using System.Waf.Applications.Services;
using System.Windows.Input;
using Waf.InformationManager.Infrastructure.Interfaces.Applications;
using Waf.InformationManager.Infrastructure.Modules.Applications.Properties;
using Waf.InformationManager.Infrastructure.Modules.Applications.Services;
using Waf.InformationManager.Infrastructure.Modules.Applications.Views;

namespace Waf.InformationManager.Infrastructure.Modules.Applications.ViewModels;

public class ShellViewModel : ViewModel<IShellView>, IShellService
{
    private readonly IMessageService messageService;
    private readonly ISystemService systemService;
    private readonly AppSettings settings;
    private object? contentView;

    public ShellViewModel(IShellView view, IMessageService messageService, ISystemService systemService, NavigationService navigationService, ISettingsService settingsService) : base(view)
    {
        this.messageService = messageService;
        this.systemService = systemService;
        NavigationService = navigationService;
        ExitCommand = new DelegateCommand(Close);
        AboutCommand = new DelegateCommand(ShowAboutMessage);
        settings = settingsService.Get<AppSettings>();
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

    public NavigationService NavigationService { get; }

    public ICommand ExitCommand { get; }

    public ICommand AboutCommand { get; }

    public object ShellView => ViewCore;

    public object? ContentView
    {
        get => contentView;
        set => SetProperty(ref contentView, value);
    }

    public void Show() => ViewCore.Show();

    public void Close() => ViewCore.Close();

    private void ViewClosed(object? sender, EventArgs e)
    {
        settings.Left = ViewCore.Left;
        settings.Top = ViewCore.Top;
        settings.Height = ViewCore.Height;
        settings.Width = ViewCore.Width;
        settings.IsMaximized = ViewCore.IsMaximized;
    }

    private void ShowAboutMessage()
    {
        messageService.ShowMessage(View, "{0} {1}\n\nThis software is a reference sample of the Win Application Framework (WAF).\n\nhttps://github.com/jbe2277/waf\n\n.NET Runtime: {2}",
            ApplicationInfo.ProductName, ApplicationInfo.Version, Environment.Version);
    }

    public void AddToolBarCommands(IReadOnlyList<ToolBarCommand> commands) => ViewCore.AddToolBarCommands(commands);

    public void ClearToolBarCommands() => ViewCore.ClearToolBarCommands();

    public void CommitUIChanges() => systemService.CommitUIChanges();
}
