using Waf.Writer.Applications.Documents;

namespace Test.Writer.Applications.Documents;

public class MockRichTextDocument(MockRichTextDocumentType documentType) : Document(documentType), IRichTextDocument
{
    public object Content { get; } = new();

    public object CloneContent() => new();
}
