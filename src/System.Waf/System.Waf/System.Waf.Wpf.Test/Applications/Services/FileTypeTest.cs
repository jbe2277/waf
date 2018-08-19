using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
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
            Assert.AreEqual(".rtf", fileType.FileExtensions.Single());

            AssertHelper.ExpectedException<ArgumentException>(() => new FileType(null, ".rtf"));
            AssertHelper.ExpectedException<ArgumentException>(() => new FileType("", ".rtf"));
            AssertHelper.ExpectedException<ArgumentException>(() => new FileType("RichText Documents", (string)null));
            AssertHelper.ExpectedException<ArgumentException>(() => new FileType("RichText Documents", ""));

            fileType = new FileType("RichText Documents", "rtf");
            Assert.AreEqual("rtf", fileType.FileExtensions.Single());
            fileType = new FileType("All Files", "*.*");
            Assert.AreEqual("*.*", fileType.FileExtensions.Single());

            fileType = new FileType("Pictures", new[] { ".jpg", ".png" });
            Assert.AreEqual("Pictures", fileType.Description);
            AssertHelper.SequenceEqual(new[] { ".jpg", ".png" }, fileType.FileExtensions);

            fileType = new FileType("Pictures", new[] { ".jpg", "*.png" });
            AssertHelper.SequenceEqual(new[] { ".jpg", "*.png" }, fileType.FileExtensions);

            AssertHelper.ExpectedException<ArgumentNullException>(() => new FileType("Pictures", (string[])null));
            AssertHelper.ExpectedException<ArgumentException>(() => new FileType("Pictures", new[] { ".jpg", "" }));
        }
    }
}
