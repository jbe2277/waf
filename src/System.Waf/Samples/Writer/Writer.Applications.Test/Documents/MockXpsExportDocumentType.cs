using System.ComponentModel.Composition;
using Waf.Writer.Applications.Documents;

namespace Test.Writer.Applications.Documents;

[Export(typeof(IXpsExportDocumentType)), Export]
internal class MockXpsExportDocumentType : MockRichTextDocumentType, IXpsExportDocumentType
{
    public MockXpsExportDocumentType() : base("Mock XPS", ".xps")
    {
    }
}
