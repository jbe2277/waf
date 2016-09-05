using Jbe.NewsReader.Applications.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Test.NewsReader.Applications.UnitTesting;
using Test.NewsReader.Applications.Views;

namespace Test.NewsReader.Applications.ViewModels
{
    [TestClass]
    public class ShellViewModelTest : ApplicationsTest
    {
        [TestMethod]
        public void ShowTest()
        {
            var shellViewModel = Container.GetExport<ShellViewModel>();
            var shellView = (MockShellView)shellViewModel.View;

            bool showCalled = false;
            shellView.ShowStub = () => showCalled = true;
            shellViewModel.Show();

            Assert.IsTrue(showCalled);
        }
    }
}
