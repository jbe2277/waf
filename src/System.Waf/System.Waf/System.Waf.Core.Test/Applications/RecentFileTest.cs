using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Waf.UnitTesting;
using System.Waf.Applications;
using System.Xml.Serialization;

namespace Test.Waf.Applications
{
    [TestClass]
    public class RecentFileTest
    {
        [TestMethod]
        public void ArgumentsTest()
        {
            AssertHelper.ExpectedException<ArgumentException>(() => new RecentFile(null));

            var recentFile = new RecentFile("Doc1");
            Assert.AreEqual("Doc1", recentFile.Path);

            AssertHelper.PropertyChangedEvent(recentFile, x => x.IsPinned, () => recentFile.IsPinned = true);
            Assert.IsTrue(recentFile.IsPinned);

            IXmlSerializable serializable = recentFile;
            Assert.IsNull(serializable.GetSchema());
            AssertHelper.ExpectedException<ArgumentNullException>(() => serializable.ReadXml(null));
            AssertHelper.ExpectedException<ArgumentNullException>(() => serializable.WriteXml(null));
        }
    }
}
