namespace Waf.Writer.Applications.Documents;

public abstract class Document(IDocumentType documentType) : Model, IDocument
{
    private string fileName = null!;
    private bool modified;

    public IDocumentType DocumentType { get; } = documentType;

    public string FileName
    {
        get => fileName;
        set => SetProperty(ref fileName, value);
    }

    public bool Modified
    {
        get => modified;
        set => SetProperty(ref modified, value);
    }
}
