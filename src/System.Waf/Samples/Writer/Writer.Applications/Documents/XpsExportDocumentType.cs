using System.Windows.Documents;
using System.IO.Packaging;
using System.IO;
using System.Windows.Xps.Packaging;
using System.Windows.Xps.Serialization;
using Waf.Writer.Applications.Properties;

namespace Waf.Writer.Applications.Documents
{
    public class XpsExportDocumentType : DocumentType
    {
        public XpsExportDocumentType() : base(Resources.XpsDocuments, ".xps")
        {
        }
        
        public override bool CanSave(IDocument document) { return document is RichTextDocument; }

        protected override void SaveCore(IDocument document, string fileName)
        {
            // We have to clone the FlowDocument before we use different pagination settings for the export.        
            var richTextDocument = (RichTextDocument)document;
            var clone = richTextDocument.CloneContent();
            clone.ColumnWidth = double.PositiveInfinity;

            using (var package = Package.Open(fileName, FileMode.Create))
            using (var xpsDocument = new XpsDocument(package, CompressionOption.Maximum))
            {
                var serializer = new XpsSerializationManager(new XpsPackagingPolicy(xpsDocument), false);
                var paginator = ((IDocumentPaginatorSource)clone).DocumentPaginator;
                serializer.SaveAsXaml(paginator);
            }
        }
    }
}
