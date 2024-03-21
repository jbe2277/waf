using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Waf.UnitTesting.Mocks;
using Test.Writer.Applications.Services;
using Test.Writer.Applications.Views;
using Waf.Writer.Applications.ViewModels;
using Waf.Writer.Applications.Views;

namespace Test.Writer.Presentation;

[TestClass]
public class IntegrationTest : PresentationTest
{
    protected override void OnCleanup()
    {
        MockSaveChangesView.ShowDialogAction = null;
        base.OnCleanup();
    }

    [TestMethod]
    public void OpenDocumentViaCommandLineIntegrationTest()
    {
        var systemService = Get<MockSystemService>();
        systemService.DocumentFileName = "2i0501fh-89f1-4197-a318-d5241135f4f6.rtf";

        var messageService = Get<MockMessageService>();

        // Call open with a fileName that doesn't exist
        messageService.Clear();

        StartApp();

        Assert.AreEqual(MessageType.Error, messageService.MessageType);
        Assert.IsFalse(string.IsNullOrEmpty(messageService.Message));
    }

    [TestMethod]
    public void NewModifyZoomPrintPreviewPrintAndCloseWithSave()
    {
        StartApp();
        var shellViewModel = Get<ShellViewModel>();
        var mainView = (MockMainView)shellViewModel.ContentView;
        Assert.AreEqual(ContentViewState.StartViewVisible, mainView.ContentViewState);
        var mainViewModel = mainView.ViewModel;
        
        mainViewModel.FileService.NewCommand.Execute(null);
        Assert.AreEqual(ContentViewState.DocumentViewVisible, mainView.ContentViewState);
        var richTextView = (MockRichTextView)mainViewModel.ActiveDocumentView!;
        var richTextViewModel = richTextView.ViewModel;
        var shellService = shellViewModel.ShellService;
        
        Assert.IsFalse(shellService.ActiveZoomCommands.ZoomInCommand.CanExecute(null));
        richTextViewModel.IsVisible = true;
        Assert.IsTrue(shellService.ActiveZoomCommands.ZoomInCommand.CanExecute(null));
        shellService.ActiveZoomCommands.ZoomInCommand.Execute(null);
        Assert.AreEqual(1.1, shellService.ActiveZoomCommands.Zoom);
        Assert.AreSame(richTextViewModel, shellService.ActiveEditingCommands);

        var document = richTextViewModel.Document;
        document.Modified = true;
        Assert.AreEqual("Document 1.rtf", document.FileName);

        shellViewModel.PrintPreviewCommand.Execute(null);
        var printPreviewView = (MockPrintPreviewView)shellViewModel.ContentView;
        printPreviewView.ViewModel.IsVisible = true;
        Assert.AreSame(document, printPreviewView.ViewModel.Document);
        printPreviewView.ViewModel.ZoomOutCommand.Execute(null);
        Assert.AreEqual(0.9, printPreviewView.ViewModel.Zoom);

        var printDialogService = Get<MockPrintDialogService>();
        printDialogService.ShowDialogResult = true;
        shellViewModel.PrintCommand.Execute(null);
        Assert.AreEqual(document.FileName, printDialogService.Description);
        shellViewModel.ClosePrintPreviewCommand.Execute(null);
        Assert.IsInstanceOfType<MockMainView>(shellViewModel.ContentView);

        bool saveChangesViewShown = false;
        MockSaveChangesView.ShowDialogAction = v =>
        {
            saveChangesViewShown = true;
            v.ViewModel.YesCommand.Execute(null);
        };

        var fileDialogService = Get<MockFileDialogService>();
        fileDialogService.Clear();
        fileDialogService.Result = new("Test File", fileDialogService.DefaultFileType);

        mainViewModel.FileService.CloseCommand.Execute(null);
        Assert.IsTrue(saveChangesViewShown);
        Assert.AreEqual("Document 1", fileDialogService.DefaultFileName);
        Assert.AreEqual(".rtf", fileDialogService.DefaultFileType!.FileExtensions.Single());
        Assert.IsNull(mainViewModel.ActiveDocumentView);
        Assert.AreEqual(ContentViewState.StartViewVisible, mainView.ContentViewState);
    }
}