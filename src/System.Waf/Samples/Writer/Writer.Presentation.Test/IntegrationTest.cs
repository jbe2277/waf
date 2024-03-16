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
    public void NewModifyAndCloseWithoutSave()
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
        Assert.AreSame(richTextViewModel, shellService.ActiveEditingCommands);
        
        richTextViewModel.Document.Modified = true;

        bool showDialogActionShown = false;
        MockSaveChangesView.ShowDialogAction = v =>
        {
            showDialogActionShown = true;
            v.ViewModel.NoCommand.Execute(null);
        };

        mainViewModel.FileService.CloseCommand.Execute(null);
        Assert.IsTrue(showDialogActionShown);
        Assert.IsNull(mainViewModel.ActiveDocumentView);
        Assert.AreEqual(ContentViewState.StartViewVisible, mainView.ContentViewState);
    }
}