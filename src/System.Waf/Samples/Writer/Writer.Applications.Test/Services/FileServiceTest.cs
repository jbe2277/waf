using Microsoft.VisualStudio.TestTools.UnitTesting;
using Waf.Writer.Applications.Services;
using System.Waf.Applications;

namespace Test.Writer.Applications.Services
{
    [TestClass]
    public class FileServiceTest : TestClassBase
    {
        [TestMethod]
        public void RecentFileList()
        {
            var fileService = Container.GetExportedValue<FileService>();

            var recentFileList = new RecentFileList();
            fileService.RecentFileList = recentFileList;
            Assert.AreEqual(recentFileList, fileService.RecentFileList);
        }
    }
}
