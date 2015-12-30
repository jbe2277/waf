using System.Windows.Documents;
using System.IO;
using System.Windows;

namespace Waf.Writer.Applications.Documents
{
    public class RichTextDocument : Document
    {
        public RichTextDocument(RichTextDocumentType documentType) : this(documentType, new FlowDocument())
        {
        }

        public RichTextDocument(RichTextDocumentType documentType, FlowDocument content) : base(documentType)
        {
            Content = content;
        }


        public FlowDocument Content { get; }


        public FlowDocument CloneContent()
        {
            FlowDocument clone = new FlowDocument();

            using (MemoryStream stream = new MemoryStream())
            {
                TextRange source = new TextRange(Content.ContentStart, Content.ContentEnd);
                source.Save(stream, DataFormats.Xaml);
                TextRange target = new TextRange(clone.ContentStart, clone.ContentEnd);
                target.Load(stream, DataFormats.Xaml);
            }

            return clone;
        }
    }
}
