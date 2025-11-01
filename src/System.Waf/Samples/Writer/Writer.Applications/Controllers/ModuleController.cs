using System.Waf.Applications;
using Waf.Writer.Applications.Services;
using Waf.Writer.Applications.ViewModels;

namespace Waf.Writer.Applications.Controllers;

/// <summary>Responsible for the application lifecycle.</summary>
internal class ModuleController : IModuleController
{
    private readonly ISystemService systemService;
    private readonly FileController fileController;
    private readonly PrintController printController;
    private readonly ShellViewModel shellViewModel;
    private readonly MainViewModel mainViewModel;
    private readonly StartViewModel startViewModel;
    private readonly DelegateCommand exitCommand;

    public ModuleController(ISystemService systemService, ShellService shellService, Lazy<FileController> fileController, Lazy<RichTextDocumentController> richTextDocumentController,
        Lazy<PrintController> printController, Lazy<ShellViewModel> shellViewModel, Lazy<MainViewModel> mainViewModel, Lazy<StartViewModel> startViewModel)
    {
        this.systemService = systemService;
        this.fileController = fileController.Value;
        _ = richTextDocumentController.Value;
        this.printController = printController.Value;
        this.shellViewModel = shellViewModel.Value;
        this.mainViewModel = mainViewModel.Value;
        this.startViewModel = startViewModel.Value;
        shellService.ShellView = this.shellViewModel.View;
        this.shellViewModel.Closing += ShellViewModelClosing;
        exitCommand = new(Close);
    }

    public void Initialize()
    {
        shellViewModel.ExitCommand = exitCommand;
        mainViewModel.StartView = startViewModel.View;
        printController.Initialize();
    }

    public void Run()
    {
        shellViewModel.ContentView = mainViewModel.View;
        if (!string.IsNullOrEmpty(systemService.DocumentFileName)) fileController.Open(systemService.DocumentFileName!);
        shellViewModel.Show();
    }

    public void Shutdown() { }

    private void Close() => shellViewModel.Close();

    private void ShellViewModelClosing(object? sender, CancelEventArgs e) => e.Cancel = !fileController.CloseAll();
}
