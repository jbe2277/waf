using System.ComponentModel.Composition;
using System.Waf.Applications;
using Waf.Writer.Applications.Documents;
using Waf.Writer.Applications.Services;
using Waf.Writer.Applications.ViewModels;

namespace Waf.Writer.Applications.Controllers;

/// <summary>Responsible to synchronize RTF Documents with RichTextViewModels.</summary>
[Export]
internal class RichTextDocumentController : DocumentController
{
    private readonly IFileService fileService;
    private readonly MainViewModel mainViewModel;
    private readonly ExportFactory<RichTextViewModel> richTextViewModelFactory;
    private readonly Dictionary<IRichTextDocument, RichTextViewModel> richTextViewModels;

    [ImportingConstructor]
    public RichTextDocumentController(IFileService fileService, MainViewModel mainViewModel, ExportFactory<RichTextViewModel> richTextViewModelFactory) : base(fileService)
    {
        this.fileService = fileService;
        this.mainViewModel = mainViewModel;
        this.richTextViewModelFactory = richTextViewModelFactory;
        richTextViewModels = new Dictionary<IRichTextDocument, RichTextViewModel>();
        mainViewModel.PropertyChanged += MainViewModelPropertyChanged;
    }

    protected override void OnDocumentAdded(IDocument document)
    {
        if (document is not IRichTextDocument richTextDocument) return;
        var richTextViewModel = richTextViewModelFactory.CreateExport().Value;
        richTextViewModel.Document = richTextDocument;
        richTextViewModels.Add(richTextDocument, richTextViewModel);
        mainViewModel.DocumentViews.Add(richTextViewModel.View);
    }

    protected override void OnDocumentRemoved(IDocument document)
    {
        if (document is not IRichTextDocument richTextDocument) return;
        mainViewModel.DocumentViews.Remove(richTextViewModels[richTextDocument].View);
        richTextViewModels.Remove(richTextDocument);
    }

    protected override void OnActiveDocumentChanged(IDocument? activeDocument)
    {
        if (activeDocument == null)
        {
            mainViewModel.ActiveDocumentView = null;
        }
        else
        {
            if (activeDocument is IRichTextDocument x) mainViewModel.ActiveDocumentView = richTextViewModels[x].View;
        }
    }

    private void MainViewModelPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(MainViewModel.ActiveDocumentView))
        {
            if (mainViewModel.ActiveDocumentView is IView richTextView)
            {
                var richTextViewModel = richTextView.GetViewModel<RichTextViewModel>();
                if (richTextViewModel != null) fileService.ActiveDocument = richTextViewModel.Document;
            }
        }
    }
}
