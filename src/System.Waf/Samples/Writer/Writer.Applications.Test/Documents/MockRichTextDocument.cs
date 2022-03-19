using Waf.Writer.Applications.Documents;

namespace Test.Writer.Applications.Documents;

public class MockRichTextDocument : Document, IRichTextDocument
{
    public MockRichTextDocument(MockRichTextDocumentType documentType) : base(documentType)
    {
    }

    public object Content { get; } = new object();

    public object CloneContent() => new object();
}
