using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Waf.Writer.Applications.Documents;
using System.Waf.UnitTesting;

namespace Test.Writer.Applications.Documents
{
    [TestClass]
    public class RichTextDocumentTypeTest
    {
        public TestContext TestContext { get; set; }


        [TestMethod]
        public void DocumentTypeTest()
        {
            RichTextDocumentType documentType = new RichTextDocumentType();
            Assert.AreEqual(".rtf", documentType.FileExtension);
            Assert.AreEqual("RichText Documents (*.rtf)", documentType.Description);
        }

        [TestMethod]
        public void NewDocumentTest()
        {
            RichTextDocumentType documentType = new RichTextDocumentType();
            Assert.IsTrue(documentType.CanNew());
            RichTextDocument document = documentType.New() as RichTextDocument;
            Assert.IsNotNull(document);
            Assert.AreEqual("Document 1.rtf", document.FileName);
        }

        [TestMethod]
        public void SaveAndOpenDocumentTest()
        {
            RichTextDocumentType documentType = new RichTextDocumentType();
            IDocument document = documentType.New();
            Assert.AreEqual("Document 1.rtf", document.FileName);

            Assert.IsTrue(documentType.CanSave(document));
            documentType.Save(document, "TestDocument1.rtf");
            Assert.AreEqual("TestDocument1.rtf", document.FileName);

            Assert.IsTrue(documentType.CanOpen());
            IDocument openedDocument = documentType.Open("TestDocument1.rtf");
            Assert.AreEqual("TestDocument1.rtf", openedDocument.FileName);

            // Note: What's missing is to compare the document content of both documents.
        }

        [TestMethod]
        public void DocumentTest()
        {
            RichTextDocumentType documentType = new RichTextDocumentType();
            IDocument document = documentType.New();

            Assert.AreEqual(document.DocumentType, documentType);

            Assert.AreEqual("Document 1.rtf", document.FileName);
            AssertHelper.PropertyChangedEvent(document, x => x.FileName, () => document.FileName = "Document 2.rtf");
            Assert.AreEqual("Document 2.rtf", document.FileName);

            Assert.IsFalse(document.Modified);
            AssertHelper.PropertyChangedEvent(document, x => x.Modified, () => document.Modified = true);
            Assert.IsTrue(document.Modified);
        }
    }
}
