using System;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Waf.Applications;
using System.Waf.Foundation;
using System.Windows.Input;
using Waf.Writer.Applications.Documents;

namespace Waf.Writer.Applications.Services
{
    [Export(typeof(IFileService)), Export]
    internal class FileService : Model, IFileService
    {
        private readonly ObservableCollection<IDocument> documents;
        private readonly ReadOnlyObservableCollection<IDocument> readOnlyDocuments;
        private IDocument activeDocument;
        private RecentFileList recentFileList;
        private ICommand newCommand;
        private ICommand openCommand;
        private ICommand closeCommand;
        private ICommand saveCommand;
        private ICommand saveAsCommand;


        [ImportingConstructor]
        public FileService()
        {
            this.documents = new ObservableCollection<IDocument>();
            this.readOnlyDocuments = new ReadOnlyObservableCollection<IDocument>(documents);
        }


        public ReadOnlyObservableCollection<IDocument> Documents => readOnlyDocuments;

        public IDocument ActiveDocument
        {
            get { return activeDocument; }
            set
            {
                if (activeDocument != value)
                {
                    if (value != null && !documents.Contains(value))
                    {
                        throw new ArgumentException("value is not an item of the Documents collection.");
                    }
                    activeDocument = value;
                    RaisePropertyChanged();
                }
            }
        }

        public RecentFileList RecentFileList
        {
            get { return recentFileList; }
            set { SetProperty(ref recentFileList, value); }
        }

        public ICommand NewCommand
        {
            get { return newCommand; }
            set { SetProperty(ref newCommand, value); }
        }

        public ICommand OpenCommand
        {
            get { return openCommand; }
            set { SetProperty(ref openCommand, value); }
        }

        public ICommand CloseCommand
        {
            get { return closeCommand; }
            set { SetProperty(ref closeCommand, value); }
        }

        public ICommand SaveCommand
        {
            get { return saveCommand; }
            set { SetProperty(ref saveCommand, value); }
        }

        public ICommand SaveAsCommand
        {
            get { return saveAsCommand; }
            set { SetProperty(ref saveAsCommand, value); }
        }


        public void AddDocument(IDocument document)
        {
            documents.Add(document);
        }

        public void RemoveDocument(IDocument document)
        {
            documents.Remove(document);
        }
    }
}
