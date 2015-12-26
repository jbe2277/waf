using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            Assert.IsNull(fileDialogService.FileTypes);
            Assert.IsNull(fileDialogService.DefaultFileType);
            Assert.IsNull(fileDialogService.DefaultFileName);


            fileDialogService.Result = result;
            Assert.AreEqual(result, fileDialogService.ShowSaveFileDialog(owner, fileTypes, fileType, "defaultFileName"));

            Assert.AreEqual(FileDialogType.SaveFileDialog, fileDialogService.FileDialogType);
            Assert.AreEqual(owner, fileDialogService.Owner);
            Assert.AreEqual(fileTypes, fileDialogService.FileTypes);
            Assert.AreEqual(fileType, fileDialogService.DefaultFileType);
            Assert.AreEqual("defaultFileName", fileDialogService.DefaultFileName);
        }
    }
}
