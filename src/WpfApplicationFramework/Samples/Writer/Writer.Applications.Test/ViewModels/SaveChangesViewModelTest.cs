using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using Waf.Writer.Applications.Documents;
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
            MockDocumentType documentType = new MockDocumentType("Mock Document", ".mock");
            var documents = new IDocument[] 
            {
                documentType.New(),
                documentType.New(),
                documentType.New()
            };

            MockSaveChangesView view = new MockSaveChangesView();
            SaveChangesViewModel viewModel = new SaveChangesViewModel(view) { Documents = documents };
            
            // In this case it tries to get the title of the unit test framework which is ""
            Assert.AreEqual("", SaveChangesViewModel.Title);
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
