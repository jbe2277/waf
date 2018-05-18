using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Waf.UnitTesting;
using Waf.Writer.Applications.Documents;

namespace Test.Writer.Applications.Documents
{
    [TestClass]
    public class DocumentTypeTest
    {
        [TestMethod]
        public void ConstructorTest()
        {
            AssertHelper.ExpectedException<ArgumentException>(() => new MockDocumentTypeBase("", ".rtf"));
            AssertHelper.ExpectedException<ArgumentException>(() => new MockDocumentTypeBase("RichText Documents", null));
            AssertHelper.ExpectedException<ArgumentException>(() => new MockDocumentTypeBase("RichText Documents", "rtf"));

            AssertHelper.ExpectedException<ArgumentNullException>(() => new DocumentBaseMock(null));
        }

        [TestMethod]
        public void CheckBaseImplementation()
        {
            var documentType = new MockDocumentTypeBase("RichText Documents", ".rtf");
            Assert.IsFalse(documentType.CanNew());
            Assert.IsFalse(documentType.CanOpen());
            Assert.IsFalse(documentType.CanSave(null));

            var documentType2 = new MockDocumentTypeBase("XPS Documents", ".xps");
            AssertHelper.ExpectedException<NotSupportedException>(() => documentType.New());
            AssertHelper.ExpectedException<NotSupportedException>(() => documentType.Open("TestDocument1.rtf"));
            AssertHelper.ExpectedException<NotSupportedException>(() => 
                documentType.Save(new DocumentBaseMock(documentType2), "TestDocument1.rtf"));

            AssertHelper.ExpectedException<ArgumentException>(() => documentType.Open(""));
            AssertHelper.ExpectedException<ArgumentException>(() => documentType.Save(new DocumentBaseMock(documentType2), ""));
            AssertHelper.ExpectedException<ArgumentNullException>(() => documentType.Save(null, "TestDocument1.rtf"));

            AssertHelper.ExpectedException<NotSupportedException>(() => documentType.CallNewCore());
            AssertHelper.ExpectedException<NotSupportedException>(() => documentType.CallOpenCore(null));
            AssertHelper.ExpectedException<NotSupportedException>(() => documentType.CallSaveCore(null, null));
        }


        private class MockDocumentTypeBase : DocumentType
        {
            public MockDocumentTypeBase(string description, string fileExtension)
                : base(description, fileExtension)
            {
            }

            public IDocument CallNewCore() { return NewCore(); }

            public IDocument CallOpenCore(string fileName) { return OpenCore(fileName); }

            public void CallSaveCore(IDocument document, string fileName)
            {
                SaveCore(document, fileName);
            }
        }

        private class DocumentBaseMock : Document
        {
            public DocumentBaseMock(MockDocumentTypeBase documentType)
                : base(documentType)
            {
            }
        }
    }
}
