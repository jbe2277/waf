using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Waf.Applications.Services;
using System.Waf.UnitTesting;

namespace Test.Waf.Applications.Services
{
    [TestClass]
    public class FileTypeTest
    {
        [TestMethod]
        public void ConstructorTest()
        {
            var fileType = new FileType("RichText Documents (*.rtf)", ".rtf");
            Assert.AreEqual("RichText Documents (*.rtf)", fileType.Description);
            Assert.AreEqual(".rtf", fileType.FileExtension);

            AssertHelper.ExpectedException<ArgumentException>(() => new FileType(null, ".rtf"));
            AssertHelper.ExpectedException<ArgumentException>(() => new FileType("", ".rtf"));
            AssertHelper.ExpectedException<ArgumentException>(() => new FileType("RichText Documents", (string)null));
            AssertHelper.ExpectedException<ArgumentException>(() => new FileType("RichText Documents", ""));

            fileType = new FileType("RichText Documents", "rtf");
            Assert.AreEqual("rtf", fileType.FileExtension);
            fileType = new FileType("All Files", "*.*");
            Assert.AreEqual("*.*", fileType.FileExtension);

            fileType = new FileType("Pictures", new[] { ".jpg", ".png" });
            Assert.AreEqual("Pictures", fileType.Description);
            Assert.AreEqual("*.jpg;*.png", fileType.FileExtension);

            fileType = new FileType("Pictures", new[] { ".jpg", "*.png" });
            Assert.AreEqual("*.jpg;*.png", fileType.FileExtension);

            AssertHelper.ExpectedException<ArgumentNullException>(() => new FileType("Pictures", (string[])null));
            AssertHelper.ExpectedException<ArgumentException>(() => new FileType("Pictures", new[] { ".jpg", "" }));
        }
    }
}
