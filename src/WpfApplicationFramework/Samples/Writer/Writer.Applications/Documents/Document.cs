using System;
using System.Waf.Foundation;

namespace Waf.Writer.Applications.Documents
{
    public abstract class Document : Model, IDocument
    {
        private readonly IDocumentType documentType;
        private string fileName;
        private bool modified;


        protected Document(IDocumentType documentType)
        {
            if (documentType == null) { throw new ArgumentNullException("documentType"); }
            this.documentType = documentType;
        }


        public IDocumentType DocumentType { get { return documentType; } }

        public string FileName 
        {
            get { return fileName; }
            set { SetProperty(ref fileName, value); }
        }

        public bool Modified
        {
            get { return modified; }
            set { SetProperty(ref modified, value); }
        }
    }
}
