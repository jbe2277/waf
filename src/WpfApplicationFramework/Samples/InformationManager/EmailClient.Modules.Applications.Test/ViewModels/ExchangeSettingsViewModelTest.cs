using Microsoft.VisualStudio.TestTools.UnitTesting;
using Waf.InformationManager.EmailClient.Modules.Applications.ViewModels;
using System.Waf.UnitTesting;
using Waf.InformationManager.EmailClient.Modules.Domain.AccountSettings;

namespace Test.InformationManager.EmailClient.Modules.Applications.ViewModels
{
    [TestClass]
    public class ExchangeSettingsViewModelTest : EmailClientTest
    {
        [TestMethod]
        public void PropertiesTest()
        {
            var viewModel = Container.GetExportedValue<ExchangeSettingsViewModel>();

            var exchangeSettings = new ExchangeSettings();
            AssertHelper.PropertyChangedEvent(viewModel, x => x.Model, () => viewModel.Model = exchangeSettings);
            Assert.AreEqual(exchangeSettings, viewModel.Model);
        }
    }
}
