using System.ComponentModel.Composition;
using System.Waf.Applications;
using System.Waf.Applications.Services;
using Waf.Writer.Applications.Documents;
using Waf.Writer.Applications.Properties;
using Waf.Writer.Applications.Services;
using Waf.Writer.Applications.ViewModels;

namespace Waf.Writer.Applications.Controllers;

/// <summary>Responsible for the file related commands.</summary>
[Export]
internal class FileController
{
    private readonly IMessageService messageService;
    private readonly ISystemService systemService;
    private readonly IFileDialogService fileDialogService;
    private readonly IShellService shellService;
    private readonly FileService fileService;
    private readonly ExportFactory<SaveChangesViewModel> saveChangesViewModelFactory;
    private readonly List<IDocumentType> documentTypes;
    private readonly RecentFileList recentFileList;
    private readonly DelegateCommand closeCommand;
    private readonly DelegateCommand saveCommand;
    private readonly DelegateCommand saveAsCommand;
    private readonly AppSettings settings;
    private IDocument? lastActiveDocument;

    [ImportingConstructor]
    public FileController(IMessageService messageService, ISystemService systemService, IFileDialogService fileDialogService, ISettingsService settingsService, IShellService shellService, 
        FileService fileService, ExportFactory<SaveChangesViewModel> saveChangesViewModelFactory, IRichTextDocumentType richTextDocumentType, IXpsExportDocumentType xpsExportDocumentType)
    {
        this.messageService = messageService;
        this.systemService = systemService;
        this.fileDialogService = fileDialogService;
        this.shellService = shellService;
        this.fileService = fileService;
        this.saveChangesViewModelFactory = saveChangesViewModelFactory;
        documentTypes = [ richTextDocumentType, xpsExportDocumentType ];
        fileService.NewCommand = new DelegateCommand(NewCommand);
        fileService.OpenCommand = new DelegateCommand(OpenCommand);
        fileService.CloseCommand = closeCommand = new(CloseCommand, CanCloseCommand);
        fileService.SaveCommand = saveCommand = new(SaveCommand, CanSaveCommand);
        fileService.SaveAsCommand = saveAsCommand = new(SaveAsCommand, CanSaveAsCommand);
        settings = settingsService.Get<AppSettings>();
        recentFileList = settings.RecentFileList ?? new();
        this.fileService.RecentFileList = recentFileList;
        fileService.PropertyChanged += FileServicePropertyChanged;
    }

    private IReadOnlyObservableList<IDocument> Documents => fileService.Documents;

    private IDocument? ActiveDocument
    {
        get => fileService.ActiveDocument;
        set => fileService.ActiveDocument = value;
    }

    public void Shutdown() => settings.RecentFileList = recentFileList;

    public IDocument? Open(string fileName)
    {
        ArgumentException.ThrowIfNullOrEmpty(fileName);
        return OpenCore(fileName!);
    }

    public bool CloseAll()
    {
        if (!CanDocumentsClose(Documents)) return false;
        ActiveDocument = null;
        while (Documents.Any())
        {
            fileService.RemoveDocument(Documents[0]);
        }
        return true;
    }

    private void NewCommand() => New(documentTypes[0]);

    private void OpenCommand(object? commandParameter)
    {
        if (commandParameter is string fileName && !string.IsNullOrEmpty(fileName)) Open(fileName);
        else Open();
    }

    private bool CanCloseCommand() => ActiveDocument != null;

    private void CloseCommand() => Close(ActiveDocument!);

    private bool CanSaveCommand() => ActiveDocument is { Modified: true };

    private void SaveCommand() => Save(ActiveDocument!);

    private bool CanSaveAsCommand() => ActiveDocument != null;

    private void SaveAsCommand() => SaveAs(ActiveDocument!);

    private void FileServicePropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(IFileService.ActiveDocument))
        {
            if (lastActiveDocument != null) lastActiveDocument.PropertyChanged -= ActiveDocumentPropertyChanged;
            lastActiveDocument = fileService.ActiveDocument;
            if (lastActiveDocument != null) lastActiveDocument.PropertyChanged += ActiveDocumentPropertyChanged;
            UpdateCommands();
        }
    }

    private void ActiveDocumentPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(Document.Modified)) UpdateCommands();
    }

    private void UpdateCommands() => DelegateCommand.RaiseCanExecuteChanged(closeCommand, saveCommand, saveAsCommand);

    internal IDocument New(IDocumentType documentType)
    {
        ArgumentNullException.ThrowIfNull(documentType);
        if (!documentTypes.Contains(documentType)) throw new ArgumentException("documentType is not an item of the DocumentTypes collection.");
        var document = documentType.New();
        fileService.AddDocument(document);
        ActiveDocument = document;
        return document;
    }

    private IDocument? Open()
    {
        var fileTypes = documentTypes.Where(x => x.CanOpen()).Select(x => new FileType(x.Description, x.FileExtension)).ToArray();
        if (!fileTypes.Any()) throw new InvalidOperationException("No DocumentType is registered that supports the Open operation.");
        var result = fileDialogService.ShowOpenFileDialog(shellService.ShellView, fileTypes);
        if (result.IsValid) return OpenCore(result.FileName!, result.SelectedFileType);
        return null;
    }

    private void Save(IDocument document)
    {
        if (Path.IsPathRooted(document.FileName))
        {
            var saveTypes = documentTypes.Where(d => d.CanSave(document)).ToArray();
            var documentType = saveTypes.First(d => d.FileExtension == Path.GetExtension(document.FileName));
            SaveCore(documentType, document, document.FileName);
        }
        else SaveAs(document);
    }

    private void SaveAs(IDocument document)
    {
        var fileTypes = documentTypes.Where(x => x.CanSave(document)).Select(x => new FileType(x.Description, x.FileExtension)).ToArray();
        if (!fileTypes.Any()) throw new InvalidOperationException("No DocumentType is registered that supports the Save operation.");

        FileType selectedFileType;
        if (systemService.FileExists(document.FileName))
        {
            var saveTypes = documentTypes.Where(d => d.CanSave(document)).ToArray();
            var documentType = saveTypes.First(d => d.FileExtension == Path.GetExtension(document.FileName));
            selectedFileType = fileTypes.First(f => f.Description == documentType.Description && f.FileExtensions.Contains(documentType.FileExtension));
        }
        else selectedFileType = fileTypes[0];
        var fileName = Path.GetFileNameWithoutExtension(document.FileName);
        var result = fileDialogService.ShowSaveFileDialog(shellService.ShellView, fileTypes, selectedFileType, fileName);
        if (result.IsValid)
        {
            var documentType = GetDocumentType(result.SelectedFileType!);
            SaveCore(documentType, document, result.FileName!);
        }
    }

    private void Close(IDocument document)
    {
        if (!CanDocumentsClose([ document ])) return;
        if (ActiveDocument == document) ActiveDocument = null;
        fileService.RemoveDocument(document);
    }

    private bool CanDocumentsClose(IEnumerable<IDocument> documentsToClose)
    {
        var modifiedDocuments = documentsToClose.Where(d => d.Modified).ToArray();
        if (!modifiedDocuments.Any()) return true;

        // Show the save changes view to the user
        var saveChangesViewModel = saveChangesViewModelFactory.CreateExport().Value;
        saveChangesViewModel.Documents = modifiedDocuments;
        var dialogResult = saveChangesViewModel.ShowDialog(shellService.ShellView);
        if (dialogResult == true)
        {
            foreach (IDocument x in modifiedDocuments) Save(x);
        }
        return dialogResult != null;
    }

    private IDocument? OpenCore(string fileName, FileType? fileType = null)
    {
        // Check if document is already opened
        var document = Documents.SingleOrDefault(d => d.FileName == fileName);
        if (document == null)
        {
            IDocumentType? documentType;
            if (fileType != null)
            {
                documentType = GetDocumentType(fileType);
            }
            else
            {
                documentType = documentTypes.FirstOrDefault(dt => dt.FileExtension == Path.GetExtension(fileName));
                if (documentType == null)
                {
                    Log.Default.Warn("The extension of the file '{0}' is not supported.", fileName);
                    messageService.ShowError(shellService.ShellView, Resources.FileExtensionNotSupported, fileName);
                    return null;
                }
            }
            try
            {
                document = documentType.Open(fileName);
            }
            catch (Exception e)
            {
                Log.Default.Error(e, "Error in open document");
                messageService.ShowError(shellService.ShellView, Resources.CannotOpenFile, fileName);
                if (e is FileNotFoundException)
                {
                    var recentFile = recentFileList.RecentFiles.FirstOrDefault(x => x.Path == fileName);
                    if (recentFile != null) recentFileList.Remove(recentFile);
                }
                return null;
            }
            fileService.AddDocument(document);
            recentFileList.AddFile(document.FileName);
        }
        ActiveDocument = document;
        return document;
    }

    private void SaveCore(IDocumentType documentType, IDocument document, string fileName)
    {
        try
        {
            documentType.Save(document, fileName);
        }
        catch (Exception e)
        {
            Log.Default.Error(e, "Error in save document");
            messageService.ShowError(shellService.ShellView, Resources.CannotSaveFile, fileName);
        }
        if (documentType.CanOpen()) recentFileList.AddFile(fileName);
    }

    private IDocumentType GetDocumentType(FileType fileType) => documentTypes.First(x => fileType.FileExtensions.Contains(x.FileExtension));
}
