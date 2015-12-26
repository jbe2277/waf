using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Waf.Applications.Services;

namespace Test.Waf.Applications.Services
{
    [TestClass]
    public class FileDialogResultTest
    {
        [TestMethod]
        public void ResultTest()
        {
            FileDialogResult result = new FileDialogResult();
            Assert.IsFalse(result.IsValid);

            FileType rtfFileType = new FileType("RichText Document", ".rtf");
            result = new FileDialogResult(@"C:\Document 1.rtf", rtfFileType);
            Assert.IsTrue(result.IsValid);
            Assert.AreEqual(@"C:\Document 1.rtf", result.FileName);
            Assert.AreEqual(rtfFileType, result.SelectedFileType);
        }
    }
}
