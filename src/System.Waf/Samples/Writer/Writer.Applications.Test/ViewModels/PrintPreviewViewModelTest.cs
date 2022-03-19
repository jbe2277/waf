using Microsoft.VisualStudio.TestTools.UnitTesting;
using Test.Writer.Applications.Documents;
using Test.Writer.Applications.Views;
using Waf.Writer.Applications.ViewModels;

namespace Test.Writer.Applications.ViewModels;

[TestClass]
public class PrintPreviewViewModelTest : ApplicationsTest
{
    [TestMethod]
    public void ShowPrintDocument()
    {
        var viewModel = Get<PrintPreviewViewModel>();
        var document = new MockRichTextDocument(new MockRichTextDocumentType());

        viewModel.Document = document;
        Assert.AreEqual(document, viewModel.Document);
    }

    [TestMethod]
    public void FitToWidth()
    {
        var viewModel = Get<PrintPreviewViewModel>();
        var view = (MockPrintPreviewView)viewModel.View;

        viewModel.FitToWidthCommand.Execute(null);
        Assert.IsTrue(view.FitToWidthCalled);
    }
}
