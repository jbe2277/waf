using Microsoft.VisualStudio.TestTools.UnitTesting;
using Waf.Writer.Applications.ViewModels;

namespace Test.Writer.Applications.ViewModels
{
    [TestClass]
    public class StartViewModelTest : TestClassBase
    {
        [TestMethod]
        public void GetFileService()
        {
            var startViewModel = Container.GetExportedValue<StartViewModel>();
            Assert.IsNotNull(startViewModel.FileService);
        }
    }
}
