namespace Waf.Writer.Applications.Documents;

public abstract class Document(IDocumentType documentType) : Model, IDocument
{
    public IDocumentType DocumentType { get; } = documentType;

    public string FileName { get; set => SetProperty(ref field, value); } = null!;

    public bool Modified { get; set => SetProperty(ref field, value); }
}
