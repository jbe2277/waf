using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Waf.InformationManager.EmailClient.Modules.Domain.Emails;
using System.Waf.UnitTesting;
using System.Waf.Foundation;
using Waf.InformationManager.EmailClient.Modules.Domain.AccountSettings;
using Test.InformationManager.Common.Domain;

namespace Test.InformationManager.EmailClient.Modules.Domain.Emails
{
    [TestClass]
    public class EmailAccountTest : DomainTest
    {
        [TestMethod]
        public void PropertiesTest()
        {
            var emailAccount = new EmailAccount();

            AssertHelper.PropertyChangedEvent(emailAccount, x => x.Name, () => emailAccount.Name = "Harry Thompson");
            Assert.AreEqual("Harry Thompson", emailAccount.Name);

            AssertHelper.PropertyChangedEvent(emailAccount, x => x.Email, () => emailAccount.Email = "harry@example.com");
            Assert.AreEqual("harry@example.com", emailAccount.Email);

            var exchangeSettings = new ExchangeSettings();
            AssertHelper.PropertyChangedEvent(emailAccount, x => x.EmailAccountSettings, () => emailAccount.EmailAccountSettings = exchangeSettings);
            Assert.AreEqual(exchangeSettings, emailAccount.EmailAccountSettings);
        }

        [TestMethod]
        public void CloneTest()
        {
            var exchangeSettings = new ExchangeSettings() { UserName = "bill" };
            var emailAccount = new EmailAccount() { Name = "Harry Thompson", Email = "harry@example.com", EmailAccountSettings = exchangeSettings };
            var clone = emailAccount.Clone();

            Assert.AreNotEqual(emailAccount, clone);
            Assert.AreEqual(emailAccount.Name, clone.Name);
            Assert.AreEqual(emailAccount.Email, clone.Email);
            
            Assert.AreNotEqual(emailAccount.EmailAccountSettings, clone.EmailAccountSettings);
            Assert.AreEqual(((ExchangeSettings)emailAccount.EmailAccountSettings).UserName, ((ExchangeSettings)clone.EmailAccountSettings).UserName);

            emailAccount.EmailAccountSettings = null;
            clone = emailAccount.Clone();
            Assert.IsNull(clone.EmailAccountSettings);
        }

        [TestMethod]
        public void ValidationTest()
        {
            var longText = "abcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyzab@example.com";
            var emailAccount = new EmailAccount();

            Assert.AreEqual("The Name field is required.", emailAccount.Validate("Name"));
            emailAccount.Name = "bill";
            Assert.AreEqual("", emailAccount.Validate("Name"));

            Assert.AreEqual("The Email Address field is required.", emailAccount.Validate("Email"));
            emailAccount.Email = longText;
            Assert.AreEqual("The field Email Address must be a string with a maximum length of 100.", emailAccount.Validate("Email"));
            emailAccount.Email = "wrong email address";
            Assert.AreEqual("The Email Address field is not a valid e-mail address.", emailAccount.Validate("Email"));
            emailAccount.Email = "harry@example.com";
            Assert.AreEqual("", emailAccount.Validate("Email"));
        }
    }
}
