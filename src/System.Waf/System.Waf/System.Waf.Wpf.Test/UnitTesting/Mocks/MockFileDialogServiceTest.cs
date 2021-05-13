using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using System.Waf.Applications.Services;
using System.Waf.UnitTesting.Mocks;

namespace Test.Waf.UnitTesting.Mocks
{
    [TestClass]
    public class MockFileDialogServiceTest
    {
        [TestMethod]
        public void ShowMockFileDialogService()
        {
            var fileDialogService = new MockFileDialogService();

            var owner = new object();
            var fileType = new FileType("description", ".fileExtension");
            var fileTypes = new[] { fileType };
            var result = new FileDialogResult("selectedName", fileType);
            fileDialogService.Result = result;
            
            Assert.AreEqual(result, fileDialogService.ShowOpenFileDialog(owner, fileTypes, fileType, "defaultFileName"));

            Assert.AreEqual(FileDialogType.OpenFileDialog, fileDialogService.FileDialogType);
            Assert.AreEqual(owner, fileDialogService.Owner);
            Assert.AreEqual(fileTypes, fileDialogService.FileTypes);
            Assert.AreEqual(fileType, fileDialogService.DefaultFileType);
            Assert.AreEqual("defaultFileName", fileDialogService.DefaultFileName);

            fileDialogService.Clear();

            Assert.AreEqual(FileDialogType.None, fileDialogService.FileDialogType);
            Assert.IsNull(fileDialogService.Owner);
            Assert.IsFalse(fileDialogService.FileTypes.Any());
            Assert.IsNull(fileDialogService.DefaultFileType);
            Assert.IsNull(fileDialogService.DefaultFileName);


            fileDialogService.Result = result;
            Assert.AreEqual(result, fileDialogService.ShowSaveFileDialog(owner, fileTypes, fileType, "defaultFileName"));

            Assert.AreEqual(FileDialogType.SaveFileDialog, fileDialogService.FileDialogType);
            Assert.AreEqual(owner, fileDialogService.Owner);
            Assert.AreEqual(fileTypes, fileDialogService.FileTypes);
            Assert.AreEqual(fileType, fileDialogService.DefaultFileType);
            Assert.AreEqual("defaultFileName", fileDialogService.DefaultFileName);

            var showOpenFileDialogCalled = false;
            fileDialogService.ShowOpenFileDialogStub = (view, fTypes, defaultFType, defaultFName) =>
            {
                showOpenFileDialogCalled = true;
                Assert.AreSame(owner, view);
                Assert.AreSame(fTypes, fileTypes);
                Assert.AreSame(defaultFType, fileType);
                Assert.AreEqual("defaultFileName", defaultFName);
                return new FileDialogResult();
            };
            Assert.IsFalse(fileDialogService.ShowOpenFileDialog(owner, fileTypes, fileType, "defaultFileName").IsValid);
            Assert.IsTrue(showOpenFileDialogCalled);

            var showSaveFileDialogCalled = false;
            fileDialogService.ShowSaveFileDialogStub = (view, fTypes, defaultFType, defaultFName) =>
            {
                showSaveFileDialogCalled = true;
                Assert.AreSame(owner, view);
                Assert.AreSame(fTypes, fileTypes);
                Assert.AreSame(defaultFType, fileType);
                Assert.AreEqual("defaultFileName", defaultFName);
                return new FileDialogResult();
            };
            Assert.IsFalse(fileDialogService.ShowSaveFileDialog(owner, fileTypes, fileType, "defaultFileName").IsValid);
            Assert.IsTrue(showSaveFileDialogCalled);
        }
    }
}
