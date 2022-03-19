using Waf.Writer.Applications.Documents;
using Waf.Writer.Applications.ViewModels;
using Waf.Writer.Applications.Views;
using Waf.Writer.Presentation.Services;

namespace Waf.Writer.Presentation.DesignData;

public class SampleSaveChangesViewModel : SaveChangesViewModel
{
    public SampleSaveChangesViewModel() : base(new MockSaveChangesView())
    {
        Documents = new List<IDocument>
        {
            new RichTextDocument(new RichTextDocumentType()) { FileName = @"C:\Users\Admin\My Documents\Document 1.rtf" },
            new RichTextDocument(new RichTextDocumentType()) { FileName = @"C:\Users\Admin\My Documents\WAF Writer\Readme.rtf" }
        };
    }

    private class MockSaveChangesView : ISaveChangesView
    {
        public object? DataContext { get; set; }

        public void Close() { }

        public void ShowDialog(object owner) { }
    }
}
