using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Waf.UnitTesting;
using Waf.Writer.Applications.Documents;

namespace Test.Writer.Applications.Documents;

[TestClass]
public class DocumentTypeTest
{
    [TestMethod]
    public void ConstructorTest()
    {
        AssertHelper.ExpectedException<ArgumentException>(() => new MockDocumentTypeBase("", ".rtf"));
        AssertHelper.ExpectedException<ArgumentNullException>(() => new MockDocumentTypeBase("RichText Documents", null!));
        AssertHelper.ExpectedException<ArgumentException>(() => new MockDocumentTypeBase("RichText Documents", "rtf"));
    }

    [TestMethod]
    public void CheckBaseImplementation()
    {
        var documentType = new MockDocumentTypeBase("RichText Documents", ".rtf");
        var document = new DocumentBaseMock(documentType);
        Assert.IsFalse(documentType.CanNew());
        Assert.IsFalse(documentType.CanOpen());
        Assert.IsFalse(documentType.CanSave(document));

        var documentType2 = new MockDocumentTypeBase("XPS Documents", ".xps");
        AssertHelper.ExpectedException<NotSupportedException>(() => documentType.New());
        AssertHelper.ExpectedException<NotSupportedException>(() => documentType.Open("TestDocument1.rtf"));
        AssertHelper.ExpectedException<NotSupportedException>(() => documentType.Save(new DocumentBaseMock(documentType2), "TestDocument1.rtf"));

        AssertHelper.ExpectedException<ArgumentException>(() => documentType.Open(""));
        AssertHelper.ExpectedException<ArgumentException>(() => documentType.Save(new DocumentBaseMock(documentType2), ""));
        AssertHelper.ExpectedException<ArgumentNullException>(() => documentType.Save(null!, "TestDocument1.rtf"));

        AssertHelper.ExpectedException<NotSupportedException>(() => documentType.CallNewCore());
        AssertHelper.ExpectedException<NotSupportedException>(() => documentType.CallOpenCore(@"C:\test.tmp"));
        AssertHelper.ExpectedException<NotSupportedException>(() => documentType.CallSaveCore(document, @"C:\test.tmp"));
    }


    private sealed class MockDocumentTypeBase(string description, string fileExtension) : DocumentType(description, fileExtension)
    {
        public IDocument CallNewCore() => NewCore();

        public IDocument CallOpenCore(string fileName) => OpenCore(fileName);

        public void CallSaveCore(IDocument document, string fileName) => SaveCore(document, fileName);
    }

    private sealed class DocumentBaseMock(MockDocumentTypeBase documentType) : Document(documentType)
    {
    }
}
