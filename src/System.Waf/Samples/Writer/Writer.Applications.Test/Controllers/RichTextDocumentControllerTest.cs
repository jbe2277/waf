using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Waf.Applications;
using System.Waf.UnitTesting;
using Waf.Writer.Applications.Documents;
using Waf.Writer.Applications.Services;
using Waf.Writer.Applications.ViewModels;
using Waf.Writer.Applications.Views;

namespace Test.Writer.Applications.Controllers
{
    [TestClass]
    public class RichTextDocumentControllerTest : TestClassBase
    {
        [TestMethod]
        public void AddAndRemoveDocumentViewTest()
        {
            var mainViewModel = Container.GetExportedValue<MainViewModel>();
            var fileService = Container.GetExportedValue<IFileService>();

            Assert.IsFalse(fileService.Documents.Any());
            Assert.IsFalse(mainViewModel.DocumentViews.Any());

            // Create new documents

            fileService.NewCommand.Execute(null);
            IDocument document = fileService.Documents.Last();

            var richTextView = mainViewModel.DocumentViews.OfType<IRichTextView>().Single();
            var richTextViewModel = ViewHelper.GetViewModel<RichTextViewModel>(richTextView);
            Assert.AreEqual(document, richTextViewModel.Document);

            fileService.NewCommand.Execute(null);
            document = fileService.Documents.Last();

            Assert.AreEqual(2, mainViewModel.DocumentViews.Count);
            richTextView = mainViewModel.DocumentViews.OfType<IRichTextView>().Last();
            richTextViewModel = ViewHelper.GetViewModel<RichTextViewModel>(richTextView);
            Assert.AreEqual(document, richTextViewModel.Document);

            // Test ActiveDocument <-> ActiveDocumentView synchronization

            Assert.AreEqual(fileService.Documents.Last(), fileService.ActiveDocument);

            fileService.ActiveDocument = fileService.Documents.First();
            Assert.AreEqual(mainViewModel.DocumentViews.First(), mainViewModel.ActiveDocumentView);

            mainViewModel.ActiveDocumentView = mainViewModel.DocumentViews.Last();
            Assert.AreEqual(fileService.Documents.Last(), fileService.ActiveDocument);

            // Close all documents

            fileService.CloseCommand.Execute(null);
            fileService.ActiveDocument = fileService.Documents.First();
            fileService.CloseCommand.Execute(null);

            Assert.IsFalse(fileService.Documents.Any());
            Assert.IsFalse(mainViewModel.DocumentViews.Any());
        }

        [TestMethod]
        public void IllegalDocumentCollectionChangeTest()
        {
            var fileService = Container.GetExportedValue<IFileService>();

            fileService.NewCommand.Execute(null);

            // We have to use reflection to get the private documents collection field
            var documentsInfo = typeof(FileService).GetField("documents", BindingFlags.Instance | BindingFlags.NonPublic);
            var documents = (ObservableCollection<IDocument>)documentsInfo.GetValue(fileService);

            // Now we call a method that is not supported by the DocumentController base class
            AssertHelper.ExpectedException<NotSupportedException>(() => documents.Clear());
        }
    }
}
