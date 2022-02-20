namespace Waf.Writer.Applications.Documents
{
    public abstract class Document : Model, IDocument
    {
        private string fileName = null!;
        private bool modified;

        protected Document(IDocumentType documentType)
        {
            DocumentType = documentType ?? throw new ArgumentNullException(nameof(documentType));
        }

        public IDocumentType DocumentType { get; }

        public string FileName
        {
            get => fileName;
            set => SetProperty(ref fileName, value);
        }

        public bool Modified
        {
            get => modified;
            set => SetProperty(ref modified, value);
        }
    }
}
