using Microsoft.VisualStudio.TestTools.UnitTesting;
using Test.Writer.Applications.Documents;
using Test.Writer.Applications.Views;
using Waf.Writer.Applications.ViewModels;

namespace Test.Writer.Applications.ViewModels
{
    [TestClass]
    public class SaveChangesViewModelTest
    {
        [TestMethod]
        public void SaveChangesViewModelCloseTest()
        {
            var documentType = new MockDocumentType("Mock Document", ".mock");
            var documents = new[] 
            {
                documentType.New(),
                documentType.New(),
                documentType.New()
            };

            var view = new MockSaveChangesView();
            var viewModel = new SaveChangesViewModel(view) { Documents = documents };
            
            // In this case it tries to get the title of the unit test framework which is ""
            Assert.AreEqual("", viewModel.Title);
            Assert.AreEqual(documents, viewModel.Documents);
            
            object owner = new object();
            Assert.IsFalse(view.IsVisible);
            MockSaveChangesView.ShowDialogAction = v => 
            {
                Assert.AreEqual(owner, v.Owner);
                Assert.IsTrue(v.IsVisible);    
            };
            bool? dialogResult = viewModel.ShowDialog(owner);
            Assert.IsNull(dialogResult);
            Assert.IsFalse(view.IsVisible);

            MockSaveChangesView.ShowDialogAction = v =>
            {
                viewModel.YesCommand.Execute(null);
            };
            dialogResult = viewModel.ShowDialog(owner);            
            Assert.AreEqual(true, dialogResult);

            MockSaveChangesView.ShowDialogAction = v =>
            {
                viewModel.NoCommand.Execute(null);
            };
            dialogResult = viewModel.ShowDialog(owner);
            Assert.AreEqual(false, dialogResult);

            MockSaveChangesView.ShowDialogAction = null;
        }
    }
}
