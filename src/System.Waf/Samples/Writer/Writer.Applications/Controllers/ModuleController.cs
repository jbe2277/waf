using System.ComponentModel.Composition;
using System.Waf.Applications;
using Waf.Writer.Applications.Services;
using Waf.Writer.Applications.ViewModels;

namespace Waf.Writer.Applications.Controllers;

/// <summary>Responsible for the application lifecycle.</summary>
[Export(typeof(IModuleController)), Export]
internal class ModuleController : IModuleController
{
    private readonly IEnvironmentService environmentService;
    private readonly FileController fileController;
    private readonly PrintController printController;
    private readonly ShellViewModel shellViewModel;
    private readonly MainViewModel mainViewModel;
    private readonly StartViewModel startViewModel;
    private readonly DelegateCommand exitCommand;

    [ImportingConstructor]
    public ModuleController(IEnvironmentService environmentService, ShellService shellService, Lazy<FileController> fileController, Lazy<RichTextDocumentController> richTextDocumentController,
        Lazy<PrintController> printController, Lazy<ShellViewModel> shellViewModel, Lazy<MainViewModel> mainViewModel, Lazy<StartViewModel> startViewModel)
    {
        this.environmentService = environmentService;
        this.fileController = fileController.Value;
        _ = richTextDocumentController.Value;
        this.printController = printController.Value;
        this.shellViewModel = shellViewModel.Value;
        this.mainViewModel = mainViewModel.Value;
        this.startViewModel = startViewModel.Value;
        shellService.ShellView = this.shellViewModel.View;
        this.shellViewModel.Closing += ShellViewModelClosing;
        exitCommand = new DelegateCommand(Close);
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
        if (!string.IsNullOrEmpty(environmentService.DocumentFileName)) fileController.Open(environmentService.DocumentFileName!);
        shellViewModel.Show();
    }

    public void Shutdown() => fileController.Shutdown();

    private void Close() => shellViewModel.Close();

    private void ShellViewModelClosing(object? sender, CancelEventArgs e) => e.Cancel = !fileController.CloseAll();
}
