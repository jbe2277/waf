using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using System.Waf.UnitTesting;
using Waf.Writer.Applications.Services;
using Waf.Writer.Applications.ViewModels;

namespace Test.Writer.Applications.ViewModels
{
    [TestClass]
    public class MainViewModelTest : TestClassBase
    {
        protected override void OnTestInitialize()
        {
            base.OnTestInitialize();
            InitializePrintController();
        }

        [TestMethod]
        public void DocumentViewTest()
        {
            var mainViewModel = Container.GetExportedValue<MainViewModel>();

            Assert.IsFalse(mainViewModel.DocumentViews.Any());
            Assert.IsNull(mainViewModel.ActiveDocumentView);

            mainViewModel.FileService.NewCommand.Execute(null);

            Assert.AreEqual(mainViewModel.DocumentViews.Single(), mainViewModel.ActiveDocumentView);

            mainViewModel.FileService.NewCommand.Execute(null);

            Assert.AreEqual(mainViewModel.DocumentViews.Last(), mainViewModel.ActiveDocumentView);
            Assert.AreEqual(2, mainViewModel.DocumentViews.Count);

            mainViewModel.ActiveDocumentView = mainViewModel.DocumentViews.First();
            mainViewModel.FileService.CloseCommand.Execute(null);

            Assert.AreEqual(1, mainViewModel.DocumentViews.Count);
            Assert.IsNull(mainViewModel.ActiveDocumentView);

            mainViewModel.ActiveDocumentView = mainViewModel.DocumentViews.Single();
            mainViewModel.FileService.CloseCommand.Execute(null);

            Assert.IsFalse(mainViewModel.DocumentViews.Any());
            Assert.IsNull(mainViewModel.ActiveDocumentView);
        }

        [TestMethod]
        public void UpdateShellServiceDocumentNameTest()
        {
            var fileService = Container.GetExportedValue<IFileService>();
            var shellService = Container.GetExportedValue<IShellService>();

            fileService.NewCommand.Execute(null);
            fileService.ActiveDocument = fileService.Documents.First();
            AssertHelper.PropertyChangedEvent(shellService, x => x.DocumentName, () => fileService.ActiveDocument.FileName = "Unit Test.rtf");
            Assert.AreEqual("Unit Test.rtf", shellService.DocumentName);
        }
    }
}
