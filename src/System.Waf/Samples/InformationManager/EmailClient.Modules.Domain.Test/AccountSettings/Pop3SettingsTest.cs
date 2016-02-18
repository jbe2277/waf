using Waf.InformationManager.EmailClient.Modules.Domain.AccountSettings;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Waf.UnitTesting;
using System.Waf.Foundation;
using Test.InformationManager.Common.Domain;

namespace Test.InformationManager.EmailClient.Modules.Domain.AccountSettings
{
    [TestClass]
    public class Pop3SettingsTest : DomainTest
    {
        [TestMethod]
        public void PropertiesTest()
        {
            var pop3Settings = new Pop3Settings();

            AssertHelper.PropertyChangedEvent(pop3Settings, x => x.Pop3ServerPath, () => pop3Settings.Pop3ServerPath = "pop3.example.com");
            Assert.AreEqual("pop3.example.com", pop3Settings.Pop3ServerPath);

            Assert.IsNotNull(pop3Settings.Pop3UserCredits);

            AssertHelper.PropertyChangedEvent(pop3Settings, x => x.SmtpServerPath, () => pop3Settings.SmtpServerPath = "smtp.example.com");
            Assert.AreEqual("smtp.example.com", pop3Settings.SmtpServerPath);

            Assert.IsNotNull(pop3Settings.SmtpUserCredits);
        }

        [TestMethod]
        public void CloneTest()
        {
            var pop3Settings = new Pop3Settings() { Pop3ServerPath = "pop3.example.com", SmtpServerPath = "smtp.example.com" };
            pop3Settings.Pop3UserCredits.UserName = "pop3User";
            pop3Settings.SmtpUserCredits.UserName = "smtpUser";
            var clone = (Pop3Settings)pop3Settings.Clone();

            Assert.AreNotEqual(pop3Settings, clone);
            Assert.AreEqual("pop3.example.com", pop3Settings.Pop3ServerPath);
            Assert.AreEqual("pop3User", pop3Settings.Pop3UserCredits.UserName);
            Assert.AreEqual("smtp.example.com", pop3Settings.SmtpServerPath);
            Assert.AreEqual("smtpUser", pop3Settings.SmtpUserCredits.UserName);
        }

        [TestMethod]
        public void ValidationTest()
        {
            var pop3Settings = new Pop3Settings();

            Assert.AreEqual("The POP3 Server field is required.", pop3Settings.Validate(nameof(pop3Settings.Pop3ServerPath)));
            Assert.AreEqual("The SMTP Server field is required.", pop3Settings.Validate(nameof(pop3Settings.SmtpServerPath)));

            pop3Settings.Pop3ServerPath = "pop3.example.com";
            pop3Settings.SmtpServerPath = "smtp.example.com";

            Assert.AreEqual("", pop3Settings.Validate(nameof(pop3Settings.Pop3ServerPath)));
            Assert.AreEqual("", pop3Settings.Validate(nameof(pop3Settings.SmtpServerPath)));
        }
    }
}
