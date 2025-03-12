using System.Waf.Applications;
using System.Windows.Input;
using Waf.Writer.Applications.Documents;

namespace Waf.Writer.Applications.Services;

internal class FileService : Model, IFileService
{
    private readonly ObservableList<IDocument> documents = [];
    private IDocument? activeDocument;

    public FileService()
    {
        Documents = new ReadOnlyObservableList<IDocument>(documents);
    }

    public IReadOnlyObservableList<IDocument> Documents { get; }

    public IDocument? ActiveDocument
    {
        get => activeDocument;
        set
        {
            if (activeDocument == value) return;
            if (value != null && !documents.Contains(value)) throw new ArgumentException("value is not an item of the Documents collection.");
            activeDocument = value;
            RaisePropertyChanged();
        }
    }

    public RecentFileList RecentFileList { get; set; } = null!;

    public ICommand NewCommand { get; set; } = DelegateCommand.DisabledCommand;

    public ICommand OpenCommand { get; set; } = DelegateCommand.DisabledCommand;

    public ICommand CloseCommand { get; set; } = DelegateCommand.DisabledCommand;

    public ICommand SaveCommand { get; set; } = DelegateCommand.DisabledCommand;

    public ICommand SaveAsCommand { get; set; } = DelegateCommand.DisabledCommand;

    public void AddDocument(IDocument document) => documents.Add(document);

    public void RemoveDocument(IDocument document) => documents.Remove(document);
}
