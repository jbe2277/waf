using System.Windows.Documents;
using System.IO;
using System.Windows;

namespace Waf.Writer.Applications.Documents
{
    public class RichTextDocument : Document
    {
        public RichTextDocument(RichTextDocumentType documentType, FlowDocument? content = null) : base(documentType)
        {
            Content = content ?? new FlowDocument();
        }

        public FlowDocument Content { get; }

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
    }
}
