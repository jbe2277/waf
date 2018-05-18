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
            RichTextDocument document = new RichTextDocument(this);
            document.FileName = string.Format(CultureInfo.CurrentCulture, Resources.DocumentFileName, 
                ++documentCount, FileExtension);
            return document;
        }

        protected override IDocument OpenCore(string fileName)
        {
            FlowDocument flowDocument = new FlowDocument();
            TextRange range = new TextRange(flowDocument.ContentStart, flowDocument.ContentEnd);
            using (FileStream stream = new FileStream(fileName, FileMode.Open))
            {
                range.Load(stream, DataFormats.Rtf);
            }

            RichTextDocument document = new RichTextDocument(this, flowDocument);
            documentCount++;
            return document;
        }

        protected override void SaveCore(IDocument document, string fileName)
        {
            FlowDocument flowDocument = ((RichTextDocument)document).Content;
            TextRange range = new TextRange(flowDocument.ContentStart, flowDocument.ContentEnd);
            using (FileStream stream = new FileStream(fileName, FileMode.Create))
            {
                range.Save(stream, DataFormats.Rtf);
            }
        }
    }
}
