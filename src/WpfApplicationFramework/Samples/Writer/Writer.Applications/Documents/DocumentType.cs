using System;
using System.Waf.Foundation;

namespace Waf.Writer.Applications.Documents
{
    public abstract class DocumentType : Model, IDocumentType
    {
        private readonly string description;
        private readonly string fileExtension;

        
        protected DocumentType(string description, string fileExtension)
        {
            if (string.IsNullOrEmpty(description)) { throw new ArgumentException("description must not be null or empty."); }
            if (string.IsNullOrEmpty(fileExtension)) { throw new ArgumentException("fileExtension must not be null or empty"); }
            if (fileExtension[0] != '.') { throw new ArgumentException("The argument fileExtension must start with the '.' character."); }
            
            this.description = description;
            this.fileExtension = fileExtension;
        }


        public string Description { get { return description; } }

        public string FileExtension { get { return fileExtension; } }


        public virtual bool CanNew() { return false; }

        public IDocument New() 
        {
            if (!CanNew()) { throw new NotSupportedException("The New operation is not supported. CanNew returned false."); }

            return NewCore(); 
        }

        public virtual bool CanOpen() { return false; }

        public IDocument Open(string fileName) 
        {
            if (string.IsNullOrEmpty(fileName)) { throw new ArgumentException("fileName must not be null or empty."); }
            if (!CanOpen()) { throw new NotSupportedException("The Open operation is not supported. CanOpen returned false."); }

            IDocument document = OpenCore(fileName);
            if (document != null) { document.FileName = fileName; }
            return document;
        }

        public virtual bool CanSave(IDocument document) { return false; }

        public void Save(IDocument document, string fileName) 
        {
            if (document == null) { throw new ArgumentNullException("document"); }
            if (string.IsNullOrEmpty(fileName)) { throw new ArgumentException("fileName must not be null or empty."); }
            if (!CanSave(document)) { throw new NotSupportedException("The Save operation is not supported. CanSave returned false."); }

            SaveCore(document, fileName);

            if (CanOpen())
            {
                document.FileName = fileName;
                document.Modified = false;
            }
        }

        protected virtual IDocument NewCore() 
        {
            throw new NotSupportedException();
        }

        protected virtual IDocument OpenCore(string fileName)
        {
            throw new NotSupportedException();
        }

        protected virtual void SaveCore(IDocument document, string fileName)
        {
            throw new NotSupportedException();
        }
    }
}
