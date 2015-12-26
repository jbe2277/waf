using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Waf.InformationManager.EmailClient.Modules.Applications.ViewModels;
using Waf.InformationManager.EmailClient.Modules.Domain.Emails;
using System.Waf.UnitTesting;
using System.Waf.Applications;
using Test.InformationManager.EmailClient.Modules.Applications.Views;

namespace Test.InformationManager.EmailClient.Modules.Applications.ViewModels
{
    [TestClass]
    public class EmailListViewModelTest : EmailClientTest
    {
        [TestMethod]
        public void PropertiesTest()
        {
            var viewModel = Container.GetExportedValue<EmailListViewModel>();

            List<Email> emails = new List<Email>();
            emails.Add(new Email());
            emails.Add(new Email());

            Assert.IsNull(viewModel.Emails);
            AssertHelper.PropertyChangedEvent(viewModel, x => x.Emails, () => viewModel.Emails = emails);
            Assert.AreEqual(emails, viewModel.Emails);

            Assert.IsNull(viewModel.SelectedEmail);
            AssertHelper.PropertyChangedEvent(viewModel, x => x.SelectedEmail, () => viewModel.SelectedEmail = emails.First());
            Assert.AreEqual(emails.First(), viewModel.SelectedEmail);

            var emptyCommand = new DelegateCommand(() => { });
            Assert.IsNull(viewModel.DeleteEmailCommand);
            AssertHelper.PropertyChangedEvent(viewModel, x => x.DeleteEmailCommand, () => viewModel.DeleteEmailCommand = emptyCommand);
            Assert.AreEqual(emptyCommand, viewModel.DeleteEmailCommand);

            Assert.AreEqual("", viewModel.FilterText);
            AssertHelper.PropertyChangedEvent(viewModel, x => x.FilterText, () => viewModel.FilterText = "abc");
            Assert.AreEqual("abc", viewModel.FilterText);
        }

        [TestMethod]
        public void FilterTest()
        {
            var viewModel = Container.GetExportedValue<EmailListViewModel>();

            var email1 = new Email() { Title = "Duis nunc", From = "user@adventure-works.com" };
            email1.To = new[] { "harry@example.com", "admin@adventure-works.com" };

            Assert.IsTrue(viewModel.Filter(email1));

            viewModel.FilterText = "dui";
            Assert.IsTrue(viewModel.Filter(email1));

            viewModel.FilterText = "user";
            Assert.IsTrue(viewModel.Filter(email1));

            viewModel.FilterText = "harry";
            Assert.IsTrue(viewModel.Filter(email1));

            viewModel.FilterText = "admin";
            Assert.IsTrue(viewModel.Filter(email1));

            viewModel.FilterText = "wrong filter";
            Assert.IsFalse(viewModel.Filter(new Email()));

            // Check that the filter works when the Email properties are null.
            Assert.IsFalse(viewModel.Filter(new Email()));
        }

        [TestMethod]
        public void FocusItemTest()
        {
            var viewModel = Container.GetExportedValue<EmailListViewModel>();
            var view = (MockEmailListView)viewModel.View;

            bool focusItemActionCalled = false;
            view.FocusItemAction = v =>
            {
                focusItemActionCalled = true;
            };

            viewModel.FocusItem();
            Assert.IsTrue(focusItemActionCalled);
        }
    }
}
