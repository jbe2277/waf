using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using Waf.Writer.Applications.Documents;
using Waf.Writer.Applications.Services;

namespace Waf.Writer.Applications.Controllers
{
    /// <summary>
    /// Responsible to synchronize the Documents with the UI Elements that represent these Documents.
    /// </summary>
    internal abstract class DocumentController
    {
        private readonly IFileService fileService;
        
        protected DocumentController(IFileService fileService)
        {
            this.fileService = fileService ?? throw new ArgumentNullException(nameof(fileService));
            PropertyChangedEventManager.AddHandler(fileService, FileServicePropertyChanged, "");
            CollectionChangedEventManager.AddHandler(fileService.Documents, DocumentsCollectionChanged);
        }

        protected abstract void OnDocumentAdded(IDocument document);

        protected abstract void OnDocumentRemoved(IDocument document);

        protected abstract void OnActiveDocumentChanged(IDocument? activeDocument);

        private void FileServicePropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(IFileService.ActiveDocument))
            {
                OnActiveDocumentChanged(fileService.ActiveDocument);
            }
        }

        private void DocumentsCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    OnDocumentAdded(e.NewItems.Cast<Document>().Single());
                    break;
                case NotifyCollectionChangedAction.Remove:
                    OnDocumentRemoved(e.OldItems.Cast<Document>().Single());
                    break;
                default:
                    throw new NotSupportedException("This kind of documents collection change is not supported.");
            }
        }
    }
}
