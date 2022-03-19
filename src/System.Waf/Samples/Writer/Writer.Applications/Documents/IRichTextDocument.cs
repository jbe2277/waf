namespace Waf.Writer.Applications.Documents;

public interface IRichTextDocument : IDocument
{
    object Content { get; }

    object CloneContent();
}

