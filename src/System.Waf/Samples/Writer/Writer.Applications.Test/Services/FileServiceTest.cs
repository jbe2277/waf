using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Waf.Applications;
using Waf.Writer.Applications.Services;

namespace Test.Writer.Applications.Services
{
    [TestClass]
    public class FileServiceTest : TestClassBase
    {
        [TestMethod]
        public void RecentFileList()
        {
            var fileService = Get<FileService>();

            var recentFileList = new RecentFileList();
            fileService.RecentFileList = recentFileList;
            Assert.AreEqual(recentFileList, fileService.RecentFileList);
        }
    }
}
