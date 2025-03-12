using System.Globalization;
using System.Waf.Applications;
using System.Waf.Applications.Services;
using System.Windows.Input;
using Waf.Writer.Applications.Properties;
using Waf.Writer.Applications.Services;
using Waf.Writer.Applications.Views;

namespace Waf.Writer.Applications.ViewModels;

public class ShellViewModel : ViewModel<IShellView>
{
    private readonly IMessageService messageService;
    private readonly AppSettings settings;
    private object contentView = null!;
    private bool isPrintPreviewVisible;

    public ShellViewModel(IShellView view, IMessageService messageService, IShellService shellService, IFileService fileService, ISettingsService settingsService) : base(view)
    {
        this.messageService = messageService;
        ShellService = shellService;
        FileService = fileService;
        EnglishCommand = new DelegateCommand(() => SelectLanguage(new CultureInfo("en-US")));
        GermanCommand = new DelegateCommand(() => SelectLanguage(new CultureInfo("de-DE")));
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

    public IFileService FileService { get; }

    public CultureInfo? NewLanguage { get; private set; }

    public ICommand EnglishCommand { get; }

    public ICommand GermanCommand { get; }

    public ICommand AboutCommand { get; }

    public ICommand PrintPreviewCommand { get; set; } = DelegateCommand.DisabledCommand;

    public ICommand ClosePrintPreviewCommand { get; set; } = DelegateCommand.DisabledCommand;

    public ICommand PrintCommand { get; set; } = DelegateCommand.DisabledCommand;

    public ICommand ExitCommand { get; set; } = DelegateCommand.DisabledCommand;

    public object ContentView
    {
        get => contentView;
        set => SetProperty(ref contentView, value);
    }

    public bool IsPrintPreviewVisible
    {
        get => isPrintPreviewVisible;
        set => SetProperty(ref isPrintPreviewVisible, value);
    }

    public event CancelEventHandler? Closing;

    public void Show() => ViewCore.Show();

    public void Close() => ViewCore.Close();

    private void SelectLanguage(CultureInfo uiCulture)
    {
        if (!uiCulture.Equals(CultureInfo.CurrentUICulture))
        {
            messageService.ShowMessage(ShellService.ShellView, Resources.RestartApplication + "\n\n" + Resources.ResourceManager.GetString("RestartApplication", uiCulture));
        }
        NewLanguage = uiCulture;
    }

    private void ShowAboutMessage() => messageService.ShowMessage(ShellService.ShellView, Resources.AboutText, ApplicationInfo.ProductName, ApplicationInfo.Version, Environment.Version);

    protected virtual void OnClosing(CancelEventArgs e) => Closing?.Invoke(this, e);

    private void ViewClosing(object? sender, CancelEventArgs e) => OnClosing(e);

    private void ViewClosed(object? sender, EventArgs e)
    {
        settings.Left = ViewCore.Left;
        settings.Top = ViewCore.Top;
        settings.Height = ViewCore.Height;
        settings.Width = ViewCore.Width;
        settings.IsMaximized = ViewCore.IsMaximized;
        settings.UICulture = NewLanguage?.Name;
    }
}
