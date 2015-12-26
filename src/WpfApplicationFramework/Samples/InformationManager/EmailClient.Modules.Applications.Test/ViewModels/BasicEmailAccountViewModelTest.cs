using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Waf.InformationManager.EmailClient.Modules.Applications.ViewModels;
using Waf.InformationManager.EmailClient.Modules.Domain.AccountSettings;
using Waf.InformationManager.EmailClient.Modules.Domain.Emails;
using System.Waf.UnitTesting;

namespace Test.InformationManager.EmailClient.Modules.Applications.ViewModels
{
    [TestClass]
    public class BasicEmailAccountViewModelTest : EmailClientTest
    {
        [TestMethod]
        public void PropertiesTest()
        {
            var viewModel = Container.GetExportedValue<BasicEmailAccountViewModel>();

            Assert.IsTrue(viewModel.IsPop3Checked);
            Assert.IsFalse(viewModel.IsExchangeChecked);

            // Set the email account and check that the boolean flags are set right.

            var emailAccount = new EmailAccount();
            emailAccount.EmailAccountSettings = new Pop3Settings();

            AssertHelper.PropertyChangedEvent(viewModel, x => x.EmailAccount, () => viewModel.EmailAccount = emailAccount);
            Assert.AreEqual(emailAccount, viewModel.EmailAccount);

            Assert.IsTrue(viewModel.IsPop3Checked);
            Assert.IsFalse(viewModel.IsExchangeChecked);

            emailAccount = new EmailAccount();
            emailAccount.EmailAccountSettings = new ExchangeSettings();
            viewModel.EmailAccount = emailAccount;

            Assert.IsFalse(viewModel.IsPop3Checked);
            Assert.IsTrue(viewModel.IsExchangeChecked);

            // Change the boolean flags

            AssertHelper.PropertyChangedEvent(viewModel, x => x.IsPop3Checked, () => viewModel.IsPop3Checked = true);

            Assert.IsTrue(viewModel.IsPop3Checked);
            Assert.IsFalse(viewModel.IsExchangeChecked);

            AssertHelper.PropertyChangedEvent(viewModel, x => x.IsExchangeChecked, () => viewModel.IsExchangeChecked = true);

            Assert.IsFalse(viewModel.IsPop3Checked);
            Assert.IsTrue(viewModel.IsExchangeChecked);

            AssertHelper.PropertyChangedEvent(viewModel, x => x.IsExchangeChecked, () => viewModel.IsPop3Checked = true);
            AssertHelper.PropertyChangedEvent(viewModel, x => x.IsPop3Checked, () => viewModel.IsExchangeChecked = true);
        }
    }
}
