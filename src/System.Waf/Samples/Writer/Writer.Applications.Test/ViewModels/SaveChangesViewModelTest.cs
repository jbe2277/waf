using Microsoft.VisualStudio.TestTools.UnitTesting;
using Test.Writer.Applications.Documents;
using Test.Writer.Applications.Views;
using Waf.Writer.Applications.ViewModels;

namespace Test.Writer.Applications.ViewModels;

[TestClass]
public class SaveChangesViewModelTest
{
    [TestMethod]
    public void SaveChangesViewModelCloseTest()
    {
        var documentType = new MockRichTextDocumentType();
        var documents = new[]
        {
            documentType.New(),
            documentType.New(),
            documentType.New()
        };

        var view = new MockSaveChangesView();
        var viewModel = new SaveChangesViewModel(view) { Documents = documents };

        Assert.AreEqual(documents, viewModel.Documents);

        var owner = new object();
        Assert.IsFalse(view.IsVisible);
        MockSaveChangesView.ShowDialogAction = v =>
        {
            Assert.AreEqual(owner, v.Owner);
            Assert.IsTrue(v.IsVisible);
        };
        var dialogResult = viewModel.ShowDialog(owner);
        Assert.IsNull(dialogResult);
        Assert.IsFalse(view.IsVisible);

        MockSaveChangesView.ShowDialogAction = _ =>
        {
            viewModel.YesCommand.Execute(null);
        };
        dialogResult = viewModel.ShowDialog(owner);
        Assert.AreEqual(true, dialogResult);

        MockSaveChangesView.ShowDialogAction = _ =>
        {
            viewModel.NoCommand.Execute(null);
        };
        dialogResult = viewModel.ShowDialog(owner);
        Assert.AreEqual(false, dialogResult);

        MockSaveChangesView.ShowDialogAction = null;
    }
}
