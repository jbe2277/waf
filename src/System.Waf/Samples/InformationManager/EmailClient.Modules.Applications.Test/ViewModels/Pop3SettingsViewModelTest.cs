using Microsoft.VisualStudio.TestTools.UnitTesting;
using Waf.InformationManager.EmailClient.Modules.Applications.ViewModels;
using Waf.InformationManager.EmailClient.Modules.Domain.AccountSettings;
using System.Waf.UnitTesting;

namespace Test.InformationManager.EmailClient.Modules.Applications.ViewModels
{
    [TestClass]
    public class Pop3SettingsViewModelTest : EmailClientTest
    {
        [TestMethod]
        public void PropertiesTest()
        {
            var viewModel = Get<Pop3SettingsViewModel>();

            var pop3Settings = new Pop3Settings();
            pop3Settings.Pop3UserCredits.UserName = "pu";
            pop3Settings.Pop3UserCredits.Password = "pp";
            pop3Settings.SmtpUserCredits.UserName = "su";
            pop3Settings.SmtpUserCredits.Password = "sp";

            AssertHelper.PropertyChangedEvent(viewModel, x => x.Model, () => viewModel.Model = pop3Settings);
            Assert.AreEqual(pop3Settings, viewModel.Model);

            Assert.AreEqual("su", pop3Settings.SmtpUserCredits.UserName);
            Assert.AreEqual("sp", pop3Settings.SmtpUserCredits.Password);

            // Check the UseSameUserCredits function

            Assert.IsFalse(viewModel.UseSameUserCredits);
            AssertHelper.PropertyChangedEvent(viewModel, x => x.UseSameUserCredits, () => viewModel.UseSameUserCredits = true);
            Assert.IsTrue(viewModel.UseSameUserCredits);

            Assert.AreEqual("pu", pop3Settings.SmtpUserCredits.UserName);
            Assert.AreEqual("pp", pop3Settings.SmtpUserCredits.Password);

            pop3Settings.Pop3UserCredits.UserName = "pu2";
            pop3Settings.Pop3UserCredits.Password = "pp2";

            Assert.AreEqual("pu2", pop3Settings.SmtpUserCredits.UserName);
            Assert.AreEqual("pp2", pop3Settings.SmtpUserCredits.Password);
        }
    }
}
