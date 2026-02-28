using System.Waf.Applications;
using Waf.Writer.Applications.Documents;
using Waf.Writer.Applications.Services;
using Waf.Writer.Applications.Views;

namespace Waf.Writer.Applications.ViewModels;

public class MainViewModel : ViewModel<IMainView>
{
    private readonly IShellService shellService;
    private IDocument? activeDocument;

    public MainViewModel(IMainView view, IShellService shellService, IFileService fileService) : base(view)
    {
        this.shellService = shellService;
        FileService = fileService;
        DocumentViews.CollectionChanged += DocumentViewsCollectionChanged;
        fileService.PropertyChanged += FileServicePropertyChanged;
    }

    public IFileService FileService { get; }

    public object StartView { get; set; } = null!;

    public ObservableList<object> DocumentViews { get; } = [];

    public object? ActiveDocumentView { get; set => SetProperty(ref field, value); }

    private void DocumentViewsCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        ViewCore.ContentViewState = DocumentViews.Any() ? ContentViewState.DocumentViewVisible : ContentViewState.StartViewVisible;
    }

    private void FileServicePropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(IFileService.ActiveDocument))
        {
            activeDocument?.PropertyChanged -= ActiveDocumentPropertyChanged;
            activeDocument = FileService.ActiveDocument;
            activeDocument?.PropertyChanged += ActiveDocumentPropertyChanged;
            UpdateShellServiceDocumentName();
        }
    }

    private void ActiveDocumentPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(Document.FileName)) UpdateShellServiceDocumentName();
    }

    private void UpdateShellServiceDocumentName()
    {
        shellService.DocumentName = FileService.ActiveDocument == null ? null : Path.GetFileName(FileService.ActiveDocument.FileName);
    }
}
