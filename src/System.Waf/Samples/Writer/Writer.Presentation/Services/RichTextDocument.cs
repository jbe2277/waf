using System.Windows.Documents;
using System.Windows;
using Waf.Writer.Applications.Documents;
using System.IO;

namespace Waf.Writer.Presentation.Services;

public class RichTextDocument : Document, IRichTextDocument
{
    public RichTextDocument(IRichTextDocumentType documentType, FlowDocument? content = null) : base(documentType)
    {
        Content = content ?? new FlowDocument();
    }

    public FlowDocument Content { get; }
    
    object IRichTextDocument.Content => Content;

    public FlowDocument CloneContent()
    {
        using var stream = new MemoryStream();
        var source = new TextRange(Content.ContentStart, Content.ContentEnd);
        source.Save(stream, DataFormats.Xaml);
        var clone = new FlowDocument();
        var target = new TextRange(clone.ContentStart, clone.ContentEnd);
        target.Load(stream, DataFormats.Xaml);
        return clone;
    }

    object IRichTextDocument.CloneContent() => CloneContent();
}
