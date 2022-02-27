using Microsoft.VisualStudio.TestTools.UnitTesting;
using Waf.InformationManager.EmailClient.Modules.Applications.ViewModels;
using Waf.InformationManager.EmailClient.Modules.Domain.Emails;
using System.Waf.UnitTesting;
using Test.InformationManager.EmailClient.Modules.Applications.Views;

namespace Test.InformationManager.EmailClient.Modules.Applications.ViewModels;

[TestClass]
public class EmailAccountsViewModelTest : EmailClientTest
{
    protected override void OnCleanup()
    {
        MockEmailAccountsView.ShowDialogAction = null;
        base.OnCleanup();
    }

    [TestMethod]
    public void PropertiesTest()
    {
        var viewModel = Get<EmailAccountsViewModel>();
        var root = new EmailClientRoot();
        root.AddEmailAccount(new EmailAccount());
        root.AddEmailAccount(new EmailAccount());
        viewModel.EmailClientRoot = root;
        AssertHelper.PropertyChangedEvent(viewModel, x => x.SelectedEmailAccount, () => viewModel.SelectedEmailAccount = root.EmailAccounts[1]);
        Assert.AreEqual(root.EmailAccounts[1], viewModel.SelectedEmailAccount);
    }

    [TestMethod]
    public void ShowDialogTest()
    {
        var viewModel = Get<EmailAccountsViewModel>();
        var ownerView = new object();

        bool showDialogCalled = false;
        MockEmailAccountsView.ShowDialogAction = view =>
        {
            showDialogCalled = true;
            Assert.AreEqual(ownerView, view.Owner);
        };

        viewModel.ShowDialog(ownerView);
        Assert.IsTrue(showDialogCalled);
    }
}
