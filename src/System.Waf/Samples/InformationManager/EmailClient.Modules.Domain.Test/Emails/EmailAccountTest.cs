using Microsoft.VisualStudio.TestTools.UnitTesting;
using Waf.InformationManager.EmailClient.Modules.Domain.Emails;
using System.Waf.UnitTesting;
using System.Waf.Foundation;
using Waf.InformationManager.EmailClient.Modules.Domain.AccountSettings;
using Test.InformationManager.Common.Domain;
using System.Linq;

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
            emailAccount.Validate();

            Assert.AreEqual("The Name field is required.", emailAccount.GetErrors(nameof(EmailAccount.Name)).Single().ErrorMessage);
            emailAccount.Name = "bill";
            Assert.IsFalse(emailAccount.GetErrors(nameof(EmailAccount.Name)).Any());

            Assert.AreEqual("The Email Address field is required.", emailAccount.GetErrors(nameof(EmailAccount.Email)).Single().ErrorMessage);
            emailAccount.Email = longText;
            Assert.AreEqual("The field Email Address must be a string with a maximum length of 100.", emailAccount.GetErrors(nameof(EmailAccount.Email)).Single().ErrorMessage);
            emailAccount.Email = "wrong email address";
            Assert.AreEqual("The Email Address field is not a valid e-mail address.", emailAccount.GetErrors(nameof(EmailAccount.Email)).Single().ErrorMessage);
            emailAccount.Email = "harry@example.com";
            Assert.IsFalse(emailAccount.GetErrors(nameof(EmailAccount.Email)).Any());
        }
    }
}
