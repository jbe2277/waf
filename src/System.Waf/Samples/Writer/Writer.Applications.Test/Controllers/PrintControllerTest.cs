using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Waf.Applications;
using System.Waf.UnitTesting;
using Test.Writer.Applications.Services;
using Waf.Writer.Applications.Services;
using Waf.Writer.Applications.ViewModels;

namespace Test.Writer.Applications.Controllers;

[TestClass]
public class PrintControllerTest : TestClassBase
{
    protected override void OnInitialize()
    {
        base.OnInitialize();
        InitializePrintController();
    }

    protected override void OnCleanup()
    {
        ShutdownPrintController();
        base.OnCleanup();
    }

    [TestMethod]
    public void PrintPreviewTest()
    {
        var shellViewModel = Get<ShellViewModel>();
        var mainViewModel = Get<MainViewModel>();
        shellViewModel.ContentView = mainViewModel.View;

        // When no document is available then the command cannot be executed
        Assert.IsFalse(shellViewModel.PrintPreviewCommand.CanExecute(null));

        // Create a new document and check that we can execute the PrintPreview command
        mainViewModel.FileService.NewCommand.Execute(null);
        Assert.IsTrue(shellViewModel.PrintPreviewCommand.CanExecute(null));

        // Execute the PrintPreview command and check the the PrintPreviewView is visible inside the ShellView
        shellViewModel.PrintPreviewCommand.Execute(null);

        // Execute the Close command and check that the MainView is visible again
        shellViewModel.ClosePrintPreviewCommand.Execute(null);
        Assert.AreEqual(ViewHelper.GetViewModel<MainViewModel>((IView)shellViewModel.ContentView), mainViewModel);
    }

    [TestMethod]
    public void PrintTest()
    {
        var shellViewModel = Get<ShellViewModel>();
        Assert.IsFalse(shellViewModel.PrintCommand.CanExecute(null));

        shellViewModel.FileService.NewCommand.Execute(null);
        Assert.IsTrue(shellViewModel.PrintCommand.CanExecute(null));

        var fileService = Get<IFileService>();
        var printDialogService = Get<MockPrintDialogService>();

        printDialogService.ShowDialogResult = true;
        shellViewModel.PrintCommand.Execute(null);
        Assert.IsNotNull(printDialogService.DocumentPaginator);
        Assert.AreEqual(fileService.ActiveDocument!.FileName, printDialogService.Description);

        printDialogService.ShowDialogResult = false;
        shellViewModel.PrintCommand.Execute(null);
        Assert.IsNull(printDialogService.DocumentPaginator);
        Assert.IsNull(printDialogService.Description);
    }

    [TestMethod]
    public void UpdateCommandsTest()
    {
        var fileService = Get<IFileService>();
        var shellViewModel = Get<ShellViewModel>();

        fileService.NewCommand.Execute(null);
        fileService.NewCommand.Execute(null);
        fileService.ActiveDocument = null;

        AssertHelper.CanExecuteChangedEvent(shellViewModel.PrintPreviewCommand, () => fileService.ActiveDocument = fileService.Documents[0]);
        AssertHelper.CanExecuteChangedEvent(shellViewModel.PrintCommand, () => fileService.ActiveDocument = fileService.Documents[^1]);
    }
}
