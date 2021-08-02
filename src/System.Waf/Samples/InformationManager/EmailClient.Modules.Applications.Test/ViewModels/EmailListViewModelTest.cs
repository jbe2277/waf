using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Waf.InformationManager.EmailClient.Modules.Applications.ViewModels;
using Waf.InformationManager.EmailClient.Modules.Domain.Emails;
using System.Waf.UnitTesting;
using Test.InformationManager.EmailClient.Modules.Applications.Views;

namespace Test.InformationManager.EmailClient.Modules.Applications.ViewModels
{
    [TestClass]
    public class EmailListViewModelTest : EmailClientTest
    {
        [TestMethod]
        public void PropertiesTest()
        {
            var viewModel = Get<EmailListViewModel>();
            var emails = new List<Email>()
            {
                new Email(),
                new Email(),
            };
            
            Assert.IsNull(viewModel.SelectedEmail);
            AssertHelper.PropertyChangedEvent(viewModel, x => x.SelectedEmail, () => viewModel.SelectedEmail = emails[0]);
            Assert.AreEqual(emails[0], viewModel.SelectedEmail);

            Assert.AreEqual("", viewModel.FilterText);
            AssertHelper.PropertyChangedEvent(viewModel, x => x.FilterText, () => viewModel.FilterText = "abc");
            Assert.AreEqual("abc", viewModel.FilterText);
        }

        [TestMethod]
        public void FilterTest()
        {
            var viewModel = Get<EmailListViewModel>();
            var email1 = new Email()
            {
                Title = "Duis nunc",
                From = "user@adventure-works.com",
                To = new[] { "harry@example.com", "admin@adventure-works.com" }
            };

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
            var viewModel = Get<EmailListViewModel>();
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
