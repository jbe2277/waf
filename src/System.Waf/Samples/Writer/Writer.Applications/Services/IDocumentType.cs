namespace Waf.Writer.Applications.Services;

public interface IDocumentType
{
    string Description { get; }

    string FileExtension { get; }

    bool CanNew();

    IDocument New();

    bool CanOpen();

    IDocument Open(string fileName);

    bool CanSave(IDocument document);

    void Save(IDocument document, string fileName);
}
