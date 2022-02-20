namespace Waf.Writer.Applications.Documents;

public interface IDocument : INotifyPropertyChanged
{
    IDocumentType DocumentType { get; }

    string FileName { get; set; }

    bool Modified { get; set; }
}
