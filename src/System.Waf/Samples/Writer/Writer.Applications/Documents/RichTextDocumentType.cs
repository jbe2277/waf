using System.Windows.Documents;
using System.IO;
using System.Windows;
using Waf.Writer.Applications.Properties;
using System.Globalization;

namespace Waf.Writer.Applications.Documents
{
    public class RichTextDocumentType : DocumentType
    {
        private int documentCount;
        
        public RichTextDocumentType() : base(Resources.RichTextDocuments, ".rtf")
        {
        }

        public override bool CanNew() { return true; }

        public override bool CanOpen() { return true; }

        public override bool CanSave(IDocument document) { return document is RichTextDocument; }

        protected override IDocument NewCore()
        {
            return new RichTextDocument(this)
            {
                FileName = string.Format(CultureInfo.CurrentCulture, Resources.DocumentFileName, ++documentCount, FileExtension)
            };
        }

        protected override IDocument OpenCore(string fileName)
        {
            var flowDocument = new FlowDocument();
            var range = new TextRange(flowDocument.ContentStart, flowDocument.ContentEnd);
            using (var stream = new FileStream(fileName, FileMode.Open))
            {
                range.Load(stream, DataFormats.Rtf);
            }
            
            var document = new RichTextDocument(this, flowDocument);
            documentCount++;
            return document;
        }

        protected override void SaveCore(IDocument document, string fileName)
        {
            var flowDocument = ((RichTextDocument)document).Content;
            var range = new TextRange(flowDocument.ContentStart, flowDocument.ContentEnd);
            using var stream = new FileStream(fileName, FileMode.Create);
            range.Save(stream, DataFormats.Rtf);
        }
    }
}
