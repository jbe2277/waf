using Waf.Writer.Applications.Documents;
using Waf.Writer.Applications.ViewModels;
using Waf.Writer.Applications.Views;
using Waf.Writer.Presentation.Views;

namespace Waf.Writer.Presentation.DesignData
{
    public class SampleMainViewModel : MainViewModel
    {
        public SampleMainViewModel() : this(null) { }

        public SampleMainViewModel(IMainView view)
            : base(view ?? new MockMainView() { ContentViewState = ContentViewState.DocumentViewVisible }, 
                 new MockShellService(), new MockFileService())
        {
            DocumentViews.Add(CreateRichTextViewModel(@"C:\Users\Admin\My Documents\Document 1.rtf").View);
            DocumentViews.Add(CreateRichTextViewModel(@"C:\Users\Admin\My Documents\ReadMe.rtf").View);
            ActiveDocumentView = DocumentViews[0];
        }


        private static RichTextViewModel CreateRichTextViewModel(string fileName)
        {
            return new RichTextViewModel(new RichTextView(), new MockShellService()) { Document = new RichTextDocument(new RichTextDocumentType()) { FileName = fileName } };
        }


        private class MockMainView : MockView, IMainView
        {
            public ContentViewState ContentViewState { get; set; }
        }
    }
}
