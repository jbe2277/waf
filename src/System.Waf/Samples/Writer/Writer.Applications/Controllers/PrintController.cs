using System.ComponentModel.Composition;
using System.Waf.Applications;
using Waf.Writer.Applications.Documents;
using Waf.Writer.Applications.Services;
using Waf.Writer.Applications.ViewModels;

namespace Waf.Writer.Applications.Controllers;

/// <summary>Responsible for the print related commands and the PrintPreview.</summary>
[Export]
internal class PrintController
{
    private readonly IFileService fileService;
    private readonly IPrintDialogService printDialogService;
    private readonly ShellViewModel shellViewModel;
    private readonly ExportFactory<PrintPreviewViewModel> printPreviewViewModelFactory;
    private readonly DelegateCommand printPreviewCommand;
    private readonly DelegateCommand printCommand;
    private readonly DelegateCommand closePrintPreviewCommand;
    private object? previousView;

    [ImportingConstructor]
    public PrintController(IFileService fileService, IPrintDialogService printDialogService, ShellViewModel shellViewModel, ExportFactory<PrintPreviewViewModel> printPreviewViewModelFactory)
    {
        this.fileService = fileService;
        this.printDialogService = printDialogService;
        this.shellViewModel = shellViewModel;
        this.printPreviewViewModelFactory = printPreviewViewModelFactory;
        printPreviewCommand = new DelegateCommand(ShowPrintPreview, CanShowPrintPreview);
        printCommand = new DelegateCommand(PrintDocument, CanPrintDocument);
        closePrintPreviewCommand = new DelegateCommand(ClosePrintPreview);
        fileService.PropertyChanged += FileServicePropertyChanged;
    }

    public void Initialize()
    {
        shellViewModel.PrintPreviewCommand = printPreviewCommand;
        shellViewModel.ClosePrintPreviewCommand = closePrintPreviewCommand;
        shellViewModel.PrintCommand = printCommand;
    }

    private bool CanShowPrintPreview() => fileService.ActiveDocument != null && !shellViewModel.IsPrintPreviewVisible;

    private void ShowPrintPreview()
    {
        var printPreviewViewModel = printPreviewViewModelFactory.CreateExport().Value;
        printPreviewViewModel.Document = (IRichTextDocument)fileService.ActiveDocument!;
        previousView = shellViewModel.ContentView;
        shellViewModel.ContentView = printPreviewViewModel.View;
        shellViewModel.IsPrintPreviewVisible = true;
        printPreviewCommand.RaiseCanExecuteChanged();
    }

    private bool CanPrintDocument() => fileService.ActiveDocument != null;

    private void PrintDocument()
    {
        if (!printDialogService.ShowDialog()) return;
        // We have to clone the FlowDocument before we use different pagination settings for the export.        
        var document = (IRichTextDocument)fileService.ActiveDocument!;
        printDialogService.PrintDocument(document.CloneContent(), fileService.ActiveDocument!.FileName);
    }

    private void ClosePrintPreview()
    {
        shellViewModel.ContentView = previousView!;
        previousView = null;
        shellViewModel.IsPrintPreviewVisible = false;
        printPreviewCommand.RaiseCanExecuteChanged();
    }

    private void FileServicePropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(IFileService.ActiveDocument))
        {
            printPreviewCommand.RaiseCanExecuteChanged();
            printCommand.RaiseCanExecuteChanged();
        }
    }
}
