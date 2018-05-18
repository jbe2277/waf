using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Waf.Applications;
using Waf.Writer.Applications.Documents;
using Waf.Writer.Applications.Services;
using Waf.Writer.Applications.ViewModels;

namespace Waf.Writer.Applications.Controllers
{
    /// <summary>
    /// Responsible to synchronize RTF Documents with RichTextViewModels.
    /// </summary>
    [Export]
    internal class RichTextDocumentController : DocumentController
    {
        private readonly IFileService fileService;
        private readonly MainViewModel mainViewModel;
        private readonly ExportFactory<RichTextViewModel> richTextViewModelFactory;
        private readonly Dictionary<RichTextDocument, RichTextViewModel> richTextViewModels;

        [ImportingConstructor]
        public RichTextDocumentController(IFileService fileService, MainViewModel mainViewModel, ExportFactory<RichTextViewModel> richTextViewModelFactory) 
            : base(fileService)
        {
            this.fileService = fileService;
            this.mainViewModel = mainViewModel;
            this.richTextViewModelFactory = richTextViewModelFactory;
            richTextViewModels = new Dictionary<RichTextDocument, RichTextViewModel>();
            
            PropertyChangedEventManager.AddHandler(mainViewModel, MainViewModelPropertyChanged, "");
        }

        protected override void OnDocumentAdded(IDocument document)
        {
            var richTextDocument = document as RichTextDocument;
            if (richTextDocument != null)
            {
                RichTextViewModel richTextViewModel = richTextViewModelFactory.CreateExport().Value;
                richTextViewModel.Document = richTextDocument;
                richTextViewModels.Add(richTextDocument, richTextViewModel);
                mainViewModel.DocumentViews.Add(richTextViewModel.View);
            }
        }

        protected override void OnDocumentRemoved(IDocument document)
        {
            var richTextDocument = document as RichTextDocument;
            if (richTextDocument != null)
            {
                mainViewModel.DocumentViews.Remove(richTextViewModels[richTextDocument].View);
                richTextViewModels.Remove(richTextDocument);
            }
        }

        protected override void OnActiveDocumentChanged(IDocument activeDocument)
        {
            if (activeDocument == null)
            {
                mainViewModel.ActiveDocumentView = null;
            }
            else
            {
                var richTextDocument = activeDocument as RichTextDocument;
                if (richTextDocument != null)
                {
                    mainViewModel.ActiveDocumentView = richTextViewModels[richTextDocument].View;
                }
            }
        }

        private void MainViewModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(MainViewModel.ActiveDocumentView))
            {
                var richTextView = mainViewModel.ActiveDocumentView as IView;
                if (richTextView != null)
                {
                    var richTextViewModel = ViewHelper.GetViewModel<RichTextViewModel>(richTextView);
                    if (richTextViewModel != null)
                    {
                        fileService.ActiveDocument = richTextViewModel.Document;
                    }
                }
            }
        }
    }
}
