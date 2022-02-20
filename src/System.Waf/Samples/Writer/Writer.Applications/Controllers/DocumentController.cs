using Waf.Writer.Applications.Documents;
using Waf.Writer.Applications.Services;

namespace Waf.Writer.Applications.Controllers
{
    /// <summary>Responsible to synchronize the Documents with the UI Elements that represent these Documents.</summary>
    internal abstract class DocumentController
    {
        private readonly IFileService fileService;
        
        protected DocumentController(IFileService fileService)
        {
            this.fileService = fileService ?? throw new ArgumentNullException(nameof(fileService));
            fileService.PropertyChanged += FileServicePropertyChanged;
            ((INotifyCollectionChanged)fileService.Documents).CollectionChanged += DocumentsCollectionChanged;
        }

        protected abstract void OnDocumentAdded(IDocument document);

        protected abstract void OnDocumentRemoved(IDocument document);

        protected abstract void OnActiveDocumentChanged(IDocument? activeDocument);

        private void FileServicePropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(IFileService.ActiveDocument)) OnActiveDocumentChanged(fileService.ActiveDocument);
        }

        private void DocumentsCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add) OnDocumentAdded(e.NewItems!.Cast<Document>().Single());
            else if (e.Action == NotifyCollectionChangedAction.Remove) OnDocumentRemoved(e.OldItems!.Cast<Document>().Single());
            else throw new NotSupportedException("This kind of documents collection change is not supported.");
        }
    }
}
