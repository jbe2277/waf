using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Waf.Presentation.Services;
using System.Waf.UnitTesting;
using System.Waf.Applications.Services;
using System.Reflection;

namespace Test.Waf.Presentation.Services
{
    [TestClass]
    public class FileDialogServiceTest
    {
        [TestMethod]
        public void ShowOpenFileDialogTest()
        {
            var service = new FileDialogService();
            
            AssertHelper.ExpectedException<ArgumentNullException>(() => service.ShowOpenFileDialog(null!, null, null));
            AssertHelper.ExpectedException<ArgumentException>(() => 
                service.ShowOpenFileDialog(new FileType[] {}, null, null));
        }

        [TestMethod]
        public void ShowSaveFileDialogTest()
        {
            var service = new FileDialogService();

            AssertHelper.ExpectedException<ArgumentNullException>(() => service.ShowSaveFileDialog(null!, null, null));
            AssertHelper.ExpectedException<ArgumentException>(() =>
                service.ShowSaveFileDialog(new FileType[] { }, null, null));
        }

        [TestMethod]
        public void CreateFilterTest()
        {
            var rtfFileType = new FileType("RichText Document", ".rtf");
            var xpsFileType = new FileType("XPS Document", ".xps");
            
            Assert.AreEqual("RichText Document|*.rtf", InvokeCreateFilter(new[] { rtfFileType }));
            Assert.AreEqual("RichText Document|*.rtf|XPS Document|*.xps",
                InvokeCreateFilter(new[] { rtfFileType, xpsFileType }));
        }

        private static string InvokeCreateFilter(IEnumerable<FileType> fileTypes)
        {
            var createFilterInfo = typeof(FileDialogService).GetMethod("CreateFilter", BindingFlags.Static | BindingFlags.NonPublic);
            return (string)createFilterInfo!.Invoke(null, new object[] { fileTypes })!;
        }
    }
}
