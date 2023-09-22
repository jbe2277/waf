using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Reflection;
using System.Waf.Applications;
using System.Waf.UnitTesting;
using Waf.Writer.Applications.Documents;
using Waf.Writer.Applications.Services;
using Waf.Writer.Applications.ViewModels;
using Waf.Writer.Applications.Views;

namespace Test.Writer.Applications.Controllers;

[TestClass]
public class RichTextDocumentControllerTest : ApplicationsTest
{
    [TestMethod]
    public void AddAndRemoveDocumentViewTest()
    {
        var mainViewModel = Get<MainViewModel>();
        var fileService = Get<IFileService>();

        Assert.IsFalse(fileService.Documents.Any());
        Assert.IsFalse(mainViewModel.DocumentViews.Any());

        // Create new documents

        fileService.NewCommand.Execute(null);
        var document = fileService.Documents[^1];

        var richTextView = mainViewModel.DocumentViews.OfType<IRichTextView>().Single();
        var richTextViewModel = ViewHelper.GetViewModel<RichTextViewModel>(richTextView)!;
        Assert.AreEqual(document, richTextViewModel.Document);

        fileService.NewCommand.Execute(null);
        document = fileService.Documents[^1];

        Assert.AreEqual(2, mainViewModel.DocumentViews.Count);
        richTextView = mainViewModel.DocumentViews.OfType<IRichTextView>().Last();
        richTextViewModel = ViewHelper.GetViewModel<RichTextViewModel>(richTextView)!;
        Assert.AreEqual(document, richTextViewModel.Document);

        // Test ActiveDocument <-> ActiveDocumentView synchronization

        Assert.AreEqual(fileService.Documents[^1], fileService.ActiveDocument);

        fileService.ActiveDocument = fileService.Documents[0];
        Assert.AreEqual(mainViewModel.DocumentViews[0], mainViewModel.ActiveDocumentView);

        mainViewModel.ActiveDocumentView = mainViewModel.DocumentViews[^1];
        Assert.AreEqual(fileService.Documents[^1], fileService.ActiveDocument);

        // Close all documents

        fileService.CloseCommand.Execute(null);
        fileService.ActiveDocument = fileService.Documents[0];
        fileService.CloseCommand.Execute(null);

        Assert.IsFalse(fileService.Documents.Any());
        Assert.IsFalse(mainViewModel.DocumentViews.Any());
    }

    [TestMethod]
    public void IllegalDocumentCollectionChangeTest()
    {
        var fileService = Get<IFileService>();

        fileService.NewCommand.Execute(null);

        // We have to use reflection to get the private documents collection field
        var documentsInfo = typeof(FileService).GetField("documents", BindingFlags.Instance | BindingFlags.NonPublic)!;
        var documents = (ObservableList<IDocument>)documentsInfo.GetValue(fileService)!;

        // Now we call a method that is not supported by the DocumentController base class
        AssertHelper.ExpectedException<NotSupportedException>(() => documents.Clear());
    }
}
