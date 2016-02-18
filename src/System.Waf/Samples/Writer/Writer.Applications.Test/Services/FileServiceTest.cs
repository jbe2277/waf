using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Waf.Writer.Applications.Services;
using System.Waf.Applications;
using System.Waf.UnitTesting;

namespace Test.Writer.Applications.Services
{
    [TestClass]
    public class FileServiceTest : TestClassBase
    {
        [TestMethod]
        public void RecentFileList()
        {
            FileService fileService = Container.GetExportedValue<FileService>();

            RecentFileList recentFileList = new RecentFileList();
            AssertHelper.PropertyChangedEvent(fileService, x => x.RecentFileList, () => fileService.RecentFileList = recentFileList);
            Assert.AreEqual(recentFileList, fileService.RecentFileList);
        }
    }
}
