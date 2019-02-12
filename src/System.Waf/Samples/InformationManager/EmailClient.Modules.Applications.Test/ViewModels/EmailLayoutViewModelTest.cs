using Microsoft.VisualStudio.TestTools.UnitTesting;
using Waf.InformationManager.EmailClient.Modules.Applications.ViewModels;
using System.Waf.UnitTesting;

namespace Test.InformationManager.EmailClient.Modules.Applications.ViewModels
{
    [TestClass]
    public class EmailLayoutViewModelTest : EmailClientTest
    {
        [TestMethod]
        public void PropertiesTest()
        {
            var viewModel = Get<EmailLayoutViewModel>();

            var emailListView = new object();
            AssertHelper.PropertyChangedEvent(viewModel, x => x.EmailListView, () => viewModel.EmailListView = emailListView);
            Assert.AreEqual(emailListView, viewModel.EmailListView);

            var emailView = new object();
            AssertHelper.PropertyChangedEvent(viewModel, x => x.EmailView, () => viewModel.EmailView = emailView);
            Assert.AreEqual(emailView, viewModel.EmailView);
        }
    }
}
