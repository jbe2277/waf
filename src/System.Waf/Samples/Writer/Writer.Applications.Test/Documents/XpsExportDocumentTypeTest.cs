using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Waf.Writer.Applications.Documents;

namespace Test.Writer.Applications.Documents
{
    [TestClass]
    public class XpsExportDocumentTypeTest
    {
        public TestContext TestContext { get; set; }


        [TestMethod]
        public void DocumentTypeTest()
        {
            XpsExportDocumentType documentType = new XpsExportDocumentType();
            Assert.AreEqual(".xps", documentType.FileExtension);
            Assert.AreEqual("XPS Documents (*.xps)", documentType.Description);
        }

        [TestMethod]
        public void SaveDocumentTest()
        {
            RichTextDocumentType rtfDocumentType = new RichTextDocumentType();
            IDocument document = rtfDocumentType.New();
            Assert.AreEqual("Document 1.rtf", document.FileName);

            XpsExportDocumentType xpsDocumentType = new XpsExportDocumentType();
            Assert.IsTrue(xpsDocumentType.CanSave(document));
            xpsDocumentType.Save(document, "TestDocument1.xps");
            
            // The document file name is still the original one because XPS is just an export format.
            Assert.AreEqual("Document 1.rtf", document.FileName);

            // Note: What's missing is to check the content of TestDocument1.xps.
        }
    }
}
