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
            var result = new FileDialogResult();
            Assert.IsFalse(result.IsValid);

            var rtfFileType = new FileType("RichText Document", ".rtf");
            result = new FileDialogResult(@"C:\Document 1.rtf", rtfFileType);
            Assert.IsTrue(result.IsValid);
            Assert.AreEqual(@"C:\Document 1.rtf", result.FileName);
            Assert.AreEqual(rtfFileType, result.SelectedFileType);
        }
    }
}
