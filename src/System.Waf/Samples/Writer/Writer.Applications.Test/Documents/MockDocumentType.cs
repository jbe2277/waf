using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Waf.Writer.Applications.Documents;
using System.ComponentModel;
using System.IO;

namespace Test.Writer.Applications.Documents
{
    public class MockDocumentType : DocumentType
    {
        public MockDocumentType(string description, string fileExtension)
            : base(description, fileExtension)
        {
            CanSaveResult = true;
        }


        public bool CanSaveResult { get; set; }
        
        public DocumentOperation DocumentOperation { get; private set; }
        
        public IDocument Document { get; private set; }
        
        public string FileName { get; private set; }

        public bool ThrowException { get; set; }


        public void Clear()
        {
            DocumentOperation = DocumentOperation.None;
            FileName = null;
            Document = null;
        }
        
        public override bool CanNew() { return true; }

        protected override IDocument NewCore()
        {
            CheckThrowException();
            DocumentOperation = DocumentOperation.New;
            return new MockDocument(this);
        }

        public override bool CanOpen() { return true; }

        protected override IDocument OpenCore(string fileName)
        {
            CheckThrowException();
            DocumentOperation = DocumentOperation.Open;
            FileName = fileName;
            return new MockDocument(this);
        }

        public override bool CanSave(IDocument document) { return CanSaveResult && document is MockDocument; }

        protected override void SaveCore(IDocument document, string fileName)
        {
            CheckThrowException();
            DocumentOperation = DocumentOperation.Save;
            Document = document;
            FileName = fileName;
        }

        private void CheckThrowException()
        {
            if (ThrowException) { throw new FileNotFoundException("ThrowException has been activated on the MockDocumentType."); }
        }
    }

    public enum DocumentOperation
    {
        None,
        New,
        Open,
        Save
    }

    public class MockDocument : Document
    {
        public MockDocument(MockDocumentType documentType) : base(documentType)
        {
        }
    }
}
