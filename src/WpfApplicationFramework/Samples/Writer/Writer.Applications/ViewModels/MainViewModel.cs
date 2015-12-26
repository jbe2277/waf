using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Waf.Applications;
using System.Waf.Applications.Services;
using System.Windows.Input;
using Waf.Writer.Applications.Documents;
using Waf.Writer.Applications.Properties;
using Waf.Writer.Applications.Services;
using Waf.Writer.Applications.Views;

namespace Waf.Writer.Applications.ViewModels
{
    [Export]
    public class MainViewModel : ViewModel<IMainView>
    {
        private readonly IShellService shellService;
        private readonly IFileService fileService;
        private readonly ObservableCollection<object> documentViews;
        private object startView;
        private IDocument activeDocument;
        private object activeDocumentView;
        

        [ImportingConstructor]
        public MainViewModel(IMainView view, IShellService shellService, IFileService fileService) 
            : base(view)
        {
            this.shellService = shellService;
            this.fileService = fileService;
            this.documentViews = new ObservableCollection<object>();
            
            CollectionChangedEventManager.AddHandler(documentViews, DocumentViewsCollectionChanged);
            PropertyChangedEventManager.AddHandler(fileService, FileServicePropertyChanged, "");
        }


        public IFileService FileService { get { return fileService; } }

        public object StartView
        {
            get { return startView; }
            set { SetProperty(ref startView, value); }
        }

        public ObservableCollection<object> DocumentViews { get { return documentViews; } }

        public object ActiveDocumentView
        {
            get { return activeDocumentView; }
            set { SetProperty(ref activeDocumentView, value); }
        }


        private void DocumentViewsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (!documentViews.Any())
            {
                ViewCore.ContentViewState = ContentViewState.StartViewVisible;
            }
            else
            {
                ViewCore.ContentViewState = ContentViewState.DocumentViewVisible;
            }
        }

        private void FileServicePropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "ActiveDocument")
            {
                if (activeDocument != null) { PropertyChangedEventManager.RemoveHandler(activeDocument, ActiveDocumentPropertyChanged, ""); }

                activeDocument = fileService.ActiveDocument;

                if (activeDocument != null) { PropertyChangedEventManager.AddHandler(activeDocument, ActiveDocumentPropertyChanged, ""); }

                UpdateShellServiceDocumentName();
            }
        }

        private void ActiveDocumentPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "FileName")
            {
                UpdateShellServiceDocumentName();
            }
        }

        private void UpdateShellServiceDocumentName()
        {
            if (fileService.ActiveDocument != null)
            {
                shellService.DocumentName = Path.GetFileName(fileService.ActiveDocument.FileName);
            }
            else
            {
                shellService.DocumentName = null;
            }
        }
    }
}
