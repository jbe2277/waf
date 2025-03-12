using System.Windows.Documents;
using System.IO.Packaging;
using System.Windows.Xps.Packaging;
using System.Windows.Xps.Serialization;
using Waf.Writer.Applications.Documents;
using System.IO;
using Waf.Writer.Presentation.Properties;

namespace Waf.Writer.Presentation.Services;

public class XpsExportDocumentType : DocumentType, IXpsExportDocumentType
{
    public XpsExportDocumentType() : base(Resources.XpsDocuments, ".xps")
    {
    }

    public override bool CanSave(IDocument document) => document is RichTextDocument;

    protected override void SaveCore(IDocument document, string fileName)
    {
        // We have to clone the FlowDocument before we use different pagination settings for the export.        
        var richTextDocument = (RichTextDocument)document;
        var clone = richTextDocument.CloneContent();
        clone.ColumnWidth = double.PositiveInfinity;

        using var package = Package.Open(fileName, FileMode.Create);
        using var xpsDocument = new XpsDocument(package, CompressionOption.Maximum);
        using var policy = new XpsPackagingPolicy(xpsDocument);
        using var serializer = new XpsSerializationManager(policy, false);
        var paginator = ((IDocumentPaginatorSource)clone).DocumentPaginator;
        serializer.SaveAsXaml(paginator);
    }
}
