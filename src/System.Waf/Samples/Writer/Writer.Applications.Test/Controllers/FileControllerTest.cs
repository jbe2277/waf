using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections;
using System.Reflection;
using System.Waf.Applications.Services;
using System.Waf.UnitTesting;
using System.Waf.UnitTesting.Mocks;
using Test.Writer.Applications.Documents;
using Test.Writer.Applications.Views;
using Waf.Writer.Applications.Controllers;
using Waf.Writer.Applications.Services;
using Waf.Writer.Applications.ViewModels;

namespace Test.Writer.Applications.Controllers;

[TestClass]
public class FileControllerTest : ApplicationsTest
{
    protected override void OnInitialize()
    {
        base.OnInitialize();
        StartApp();
    }

    [TestMethod]
    public void RegisterDocumentTypesTest()
    {
        var fileController = Get<FileController>();
        var fileService = Get<IFileService>();
        var documentType = Get<MockRichTextDocumentType>();

        Assert.IsFalse(fileService.Documents.Any());
        fileController.New(documentType);
        Assert.AreEqual(documentType, fileService.Documents.Single().DocumentType);
    }

    [TestMethod]
    public void NewAndActiveDocumentTest()
    {
        var fileController = Get<FileController>();
        var fileService = Get<IFileService>();
        var documentType = Get<MockRichTextDocumentType>();

        Assert.IsFalse(fileService.Documents.Any());
        Assert.IsNull(fileService.ActiveDocument);

        var document = fileController.New(documentType);
        AssertHelper.SequenceEqual(new[] { document }, fileService.Documents);
        Assert.AreEqual(document, fileService.ActiveDocument);

        AssertHelper.ExpectedException<ArgumentNullException>(() => fileController.New(null!));
        AssertHelper.ExpectedException<ArgumentException>(() => fileController.New(new MockRichTextDocumentType()));

        AssertHelper.PropertyChangedEvent(fileService, x => x.ActiveDocument, () => fileService.ActiveDocument = null);
        Assert.AreEqual(null, fileService.ActiveDocument);

        AssertHelper.ExpectedException<ArgumentException>(() => fileService.ActiveDocument = documentType.New());
    }

    [TestMethod]
    public void OpenDocumentTest()
    {
        var fileDialogService = Get<MockFileDialogService>();
        _ = Get<FileController>();
        var fileService = Get<IFileService>();
        var documentType = Get<MockRichTextDocumentType>();

        Assert.IsFalse(fileService.Documents.Any());
        Assert.IsNull(fileService.ActiveDocument);

        fileDialogService.Result = new FileDialogResult("Document1.rtf", new FileType("Mock Document", ".rtf"));
        fileService.OpenCommand.Execute(null);

        Assert.AreEqual(FileDialogType.OpenFileDialog, fileDialogService.FileDialogType);
        Assert.AreEqual("Mock RTF", fileDialogService.FileTypes.First().Description);
        Assert.AreEqual(".rtf", fileDialogService.FileTypes.First().FileExtensions.Single());

        Assert.AreEqual(DocumentOperation.Open, documentType.DocumentOperation);
        Assert.AreEqual("Document1.rtf", documentType.FileName);

        var document = fileService.Documents[^1];
        Assert.AreEqual("Document1.rtf", document.FileName);

        AssertHelper.SequenceEqual(new[] { document }, fileService.Documents);
        Assert.AreEqual(document, fileService.ActiveDocument);

        // Open the same file again -> It's not opened again, just activated.

        fileService.ActiveDocument = null;
        fileService.OpenCommand.Execute("Document1.rtf");
        AssertHelper.SequenceEqual(new[] { document }, fileService.Documents);
        Assert.AreEqual(document, fileService.ActiveDocument);

        // Now the user cancels the OpenFileDialog box

        fileDialogService.Result = new FileDialogResult();
        int documentsCount = fileService.Documents.Count;
        fileService.OpenCommand.Execute(null);
        Assert.AreEqual(documentsCount, fileService.Documents.Count);

        AssertHelper.SequenceEqual(new[] { document }, fileService.Documents);
        Assert.AreEqual(document, fileService.ActiveDocument);
    }

    [TestMethod]
    public void OpenDocumentViaCommandLineTest()
    {
        _ = Get<FileController>();
        var fileService = Get<IFileService>();

        Assert.IsFalse(fileService.Documents.Any());
        Assert.IsNull(fileService.ActiveDocument);

        // Open is called with a fileName which might be a command line parameter.
        fileService.OpenCommand.Execute("Document1.rtf");
        var document = fileService.Documents[^1];
        Assert.AreEqual("Document1.rtf", document.FileName);

        AssertHelper.SequenceEqual(new[] { document }, fileService.Documents);
        Assert.AreEqual(document, fileService.ActiveDocument);

        // Call open with a fileName that has an invalid extension
        var messageService = Get<MockMessageService>();
        messageService.Clear();
        fileService.OpenCommand.Execute("Document.wrongextension");
        Assert.AreEqual(MessageType.Error, messageService.MessageType);
        Assert.IsFalse(string.IsNullOrEmpty(messageService.Message));
    }

    [TestMethod]
    public void OpenExceptionsTest()
    {
        var fileController = Get<FileController>();
        var documentTypesField = typeof(FileController).GetField("documentTypes", BindingFlags.Instance | BindingFlags.NonPublic)!;
        ((IList)documentTypesField.GetValue(fileController)!).Clear();

        AssertHelper.ExpectedException<ArgumentException>(() => fileController.Open(null!));

        var fileService = Get<IFileService>();
        AssertHelper.ExpectedException<InvalidOperationException>(() => fileService.OpenCommand.Execute(null));
    }

    [TestMethod]
    public void OpenExceptionTestWithRecentFileList()
    {
        _ = Get<FileController>();
        var fileService = Get<IFileService>();
        foreach (var x in fileService.RecentFileList.RecentFiles.ToArray()) fileService.RecentFileList.Remove(x);

        var documentType = Get<MockRichTextDocumentType>();
        documentType.ThrowException = true;

        fileService.RecentFileList.AddFile("Document1.rtf");
        Assert.IsTrue(fileService.RecentFileList.RecentFiles.Any());

        fileService.OpenCommand.Execute("Document1.rtf");

        // Ensure that the recent file is remove from the list.
        Assert.IsFalse(fileService.RecentFileList.RecentFiles.Any());
    }

    [TestMethod]
    public void SaveDocumentTest()
    {
        var fileDialogService = Get<MockFileDialogService>();
        var fileController = Get<FileController>();
        var fileService = Get<IFileService>();
        var documentType = Get<MockRichTextDocumentType>();
        fileController.New(documentType);
        var document = fileService.Documents.Single();
        document.FileName = "Document.rtf";

        fileDialogService.Result = new FileDialogResult("Document1.rtf", new FileType("Mock Document", ".rtf"));
        fileService.SaveAsCommand.Execute(null);

        Assert.AreEqual(FileDialogType.SaveFileDialog, fileDialogService.FileDialogType);
        Assert.AreEqual("Mock RTF", fileDialogService.FileTypes.First().Description);
        Assert.AreEqual(".rtf", fileDialogService.FileTypes.First().FileExtensions.Single());
        Assert.AreEqual("Mock RTF", fileDialogService.DefaultFileType!.Description);
        Assert.AreEqual(".rtf", fileDialogService.DefaultFileType.FileExtensions.Single());
        Assert.AreEqual("Document", fileDialogService.DefaultFileName);

        Assert.AreEqual(DocumentOperation.Save, documentType.DocumentOperation);
        Assert.AreEqual(document, documentType.Document);

        Assert.AreEqual("Document1.rtf", documentType.FileName);

        // Change the CanSave to return false so that no documentType is able to save the document anymore

        documentType.CanSaveResult = false;
        Assert.IsFalse(fileService.SaveCommand.CanExecute(null));

        // Simulate an exception during the Save operation.

        var messageService = Get<MockMessageService>();
        messageService.Clear();
        documentType.ThrowException = true;
        documentType.CanSaveResult = true;
        fileService.SaveAsCommand.Execute(null);
        Assert.AreEqual(MessageType.Error, messageService.MessageType);
        Assert.IsFalse(string.IsNullOrEmpty(messageService.Message));
    }

    [TestMethod]
    public void SaveDocumentWhenFileExistsTest()
    {
        // Get the absolute file path
        string fileName = Path.GetFullPath("SaveWhenFileExistsTest.rtf");

        var fileDialogService = Get<MockFileDialogService>();
        var fileController = Get<FileController>();
        var fileService = Get<IFileService>();
        var documentType = Get<MockRichTextDocumentType>();

        using (var writer = new StreamWriter(fileName))
        {
            writer.WriteLine("Hello World");
        }

        var document = (MockRichTextDocument)fileController.New(documentType);
        document.Modified = true;
        // We set the absolute file path to simulate that we already saved the document
        document.FileName = fileName;

        fileService.SaveCommand.Execute(null);
        Assert.AreEqual(DocumentOperation.Save, documentType.DocumentOperation);
        Assert.AreEqual(document, documentType.Document);
        Assert.AreEqual(fileName, documentType.FileName);

        // Simulate the scenario when the user overwrites the existing file
        fileDialogService.Result = new FileDialogResult(fileName, new FileType("Mock Document", ".rtf"));
        fileService.SaveAsCommand.Execute(null);
        Assert.AreEqual("Mock RTF", fileDialogService.DefaultFileType!.Description);
        Assert.AreEqual(".rtf", fileDialogService.DefaultFileType.FileExtensions.Single());
        Assert.AreEqual(DocumentOperation.Save, documentType.DocumentOperation);
        Assert.AreEqual(document, documentType.Document);
        Assert.AreEqual(fileName, documentType.FileName);
    }

    [TestMethod]
    public void SaveExceptionsTest()
    {
        var fileController = Get<FileController>();
        var documentType = Get<MockRichTextDocumentType>();
        documentType.CanSaveResult = false;
        Get<MockXpsExportDocumentType>().CanSaveResult = false;

        var saveAsMethod = typeof(FileController).GetMethod("SaveAs", BindingFlags.Instance | BindingFlags.NonPublic)!;
        var document = fileController.New(documentType);
        var exception = AssertHelper.ExpectedException<TargetInvocationException>(() => saveAsMethod.Invoke(fileController, new[] { document }));
        Assert.IsInstanceOfType(exception.InnerException, typeof(InvalidOperationException));
    }

    [TestMethod]
    public void OpenSaveDocumentTest()
    {
        var fileDialogService = Get<MockFileDialogService>();
        var fileService = Get<IFileService>();

        fileDialogService.Result = new FileDialogResult();
        fileService.OpenCommand.Execute(null);
        Assert.AreEqual(FileDialogType.OpenFileDialog, fileDialogService.FileDialogType);

        Assert.IsFalse(fileService.SaveCommand.CanExecute(null));
        Assert.IsFalse(fileService.SaveAsCommand.CanExecute(null));

        fileService.NewCommand.Execute(null);

        Assert.IsFalse(fileService.SaveCommand.CanExecute(null));
        Assert.IsTrue(fileService.SaveAsCommand.CanExecute(null));

        var mainViewModel = Get<MainViewModel>();
        var richTextViewModel = ((MockRichTextView)mainViewModel.ActiveDocumentView!).ViewModel;

        AssertHelper.CanExecuteChangedEvent(fileService.SaveCommand, () => richTextViewModel.Document.Modified = true);

        Assert.IsTrue(fileService.SaveCommand.CanExecute(null));
        Assert.IsTrue(fileService.SaveAsCommand.CanExecute(null));

        fileDialogService.Result = new FileDialogResult();
        fileService.SaveCommand.Execute(null);
        Assert.AreEqual(FileDialogType.SaveFileDialog, fileDialogService.FileDialogType);
        Assert.IsTrue(richTextViewModel.Document.Modified);

        fileDialogService.Result = new FileDialogResult();
        fileService.SaveAsCommand.Execute(null);
        Assert.AreEqual(FileDialogType.SaveFileDialog, fileDialogService.FileDialogType);
    }

    [TestMethod]
    public void CloseDocumentTest()
    {
        var fileController = Get<FileController>();
        var fileService = Get<IFileService>();
        var documentType = Get<MockRichTextDocumentType>();
        fileController.New(documentType);
        fileService.CloseCommand.Execute(null);
        Assert.IsFalse(fileService.Documents.Any());
    }

    [TestMethod]
    public void UpdateCommandsTest()
    {
        var documentManager = Get<IFileService>();
        documentManager.NewCommand.Execute(null);
        documentManager.NewCommand.Execute(null);
        documentManager.ActiveDocument = null;
        AssertHelper.CanExecuteChangedEvent(documentManager.CloseCommand, () => documentManager.ActiveDocument = documentManager.Documents[0]);
        AssertHelper.CanExecuteChangedEvent(documentManager.SaveCommand, () => documentManager.ActiveDocument = documentManager.Documents[^1]);
        AssertHelper.CanExecuteChangedEvent(documentManager.SaveAsCommand, () => documentManager.ActiveDocument = documentManager.Documents[0]);
    }
}
