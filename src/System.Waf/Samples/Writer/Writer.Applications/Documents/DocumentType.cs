using System;
using System.Waf.Foundation;

namespace Waf.Writer.Applications.Documents
{
    public abstract class DocumentType : Model, IDocumentType
    {
        protected DocumentType(string description, string fileExtension)
        {
            if (string.IsNullOrEmpty(description)) throw new ArgumentException("description must not be null or empty.", nameof(description));
            if (string.IsNullOrEmpty(fileExtension)) throw new ArgumentException("fileExtension must not be null or empty", nameof(fileExtension));
            if (fileExtension[0] != '.') throw new ArgumentException("The argument fileExtension must start with the '.' character.", nameof(fileExtension));
            Description = description;
            FileExtension = fileExtension;
        }

        public string Description { get; }

        public string FileExtension { get; }

        public virtual bool CanNew() => false;

        public IDocument New() 
        {
            if (!CanNew()) throw new NotSupportedException("The New operation is not supported. CanNew returned false.");
            return NewCore(); 
        }

        public virtual bool CanOpen() => false;

        public IDocument Open(string fileName) 
        {
            if (string.IsNullOrEmpty(fileName)) throw new ArgumentException("fileName must not be null or empty.", nameof(fileName));
            if (!CanOpen()) throw new NotSupportedException("The Open operation is not supported. CanOpen returned false.");
            var document = OpenCore(fileName);
            document.FileName = fileName;
            return document;
        }

        public virtual bool CanSave(IDocument document) => false;

        public void Save(IDocument document, string fileName) 
        {
            if (document == null) throw new ArgumentNullException(nameof(document));
            if (string.IsNullOrEmpty(fileName)) throw new ArgumentException("fileName must not be null or empty.", nameof(fileName));
            if (!CanSave(document)) throw new NotSupportedException("The Save operation is not supported. CanSave returned false.");

            SaveCore(document, fileName);
            if (CanOpen())
            {
                document.FileName = fileName;
                document.Modified = false;
            }
        }

        protected virtual IDocument NewCore() => throw new NotSupportedException();

        protected virtual IDocument OpenCore(string fileName) => throw new NotSupportedException();

        protected virtual void SaveCore(IDocument document, string fileName) => throw new NotSupportedException();
    }
}
