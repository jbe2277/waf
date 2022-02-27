using Microsoft.VisualStudio.TestTools.UnitTesting;
using Waf.InformationManager.EmailClient.Modules.Applications.ViewModels;
using System.Waf.UnitTesting;
using Test.InformationManager.EmailClient.Modules.Applications.Views;

namespace Test.InformationManager.EmailClient.Modules.Applications.ViewModels;

[TestClass]
public class EditEmailAccountViewModelTest : EmailClientTest
{
    protected override void OnCleanup()
    {
        MockEditEmailAccountView.ShowDialogAction = null;
        base.OnCleanup();
    }

    [TestMethod]
    public void PropertiesTest()
    {
        var viewModel = Get<EditEmailAccountViewModel>();

        var contentView = new object();
        AssertHelper.PropertyChangedEvent(viewModel, x => x.ContentView, () => viewModel.ContentView = contentView);
        Assert.AreEqual(contentView, viewModel.ContentView);

        Assert.IsTrue(viewModel.IsValid);
        AssertHelper.PropertyChangedEvent(viewModel, x => x.IsValid, () => viewModel.IsValid = false);
        Assert.IsFalse(viewModel.IsValid);

        Assert.IsFalse(viewModel.IsLastPage);
        AssertHelper.PropertyChangedEvent(viewModel, x => x.IsLastPage, () => viewModel.IsLastPage = true);
        Assert.IsTrue(viewModel.IsLastPage);
    }

    [TestMethod]
    public void ShowDialogAndCloseTest()
    {
        var viewModel = Get<EditEmailAccountViewModel>();
        var ownerView = new object();

        bool showDialogCalled = false;
        MockEditEmailAccountView.ShowDialogAction = view =>
        {
            showDialogCalled = true;
            Assert.AreEqual(ownerView, view.Owner);

            Assert.IsTrue(view.IsVisible);
            viewModel.Close();
            Assert.IsFalse(view.IsVisible);
        };

        viewModel.ShowDialog(ownerView);
        Assert.IsTrue(showDialogCalled);
    }
}
