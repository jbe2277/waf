﻿using System.ComponentModel.Composition;
using Waf.Writer.Applications.Documents;

namespace Test.Writer.Applications.Documents;

[Export(typeof(IRichTextDocumentType)), Export]
public class MockRichTextDocumentType(string description, string fileExtension) : DocumentType(description, fileExtension), IRichTextDocumentType
{
    public MockRichTextDocumentType() : this("Mock RTF", ".rtf") { }

    public bool CanSaveResult { get; set; } = true;

    public DocumentOperation DocumentOperation { get; private set; }

    public IDocument? Document { get; private set; }

    public string? FileName { get; private set; }

    public bool ThrowException { get; set; }

    public void Clear()
    {
        DocumentOperation = DocumentOperation.None;
        FileName = null;
        Document = null;
    }

    public override bool CanNew() => true;

    protected override IDocument NewCore()
    {
        CheckThrowException();
        DocumentOperation = DocumentOperation.New;
        return new MockRichTextDocument(this);
    }

    public override bool CanOpen() => true;

    protected override IDocument OpenCore(string fileName)
    {
        CheckThrowException();
        DocumentOperation = DocumentOperation.Open;
        FileName = fileName;
        return new MockRichTextDocument(this);
    }

    public override bool CanSave(IDocument document) => CanSaveResult && document is MockRichTextDocument;

    protected override void SaveCore(IDocument document, string fileName)
    {
        CheckThrowException();
        DocumentOperation = DocumentOperation.Save;
        Document = document;
        FileName = fileName;
    }

    private void CheckThrowException()
    {
        if (ThrowException) throw new FileNotFoundException("ThrowException has been activated on the MockDocumentType.");
    }
}

public enum DocumentOperation
{
    None,
    New,
    Open,
    Save
}
