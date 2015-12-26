using Waf.Writer.Applications.ViewModels;
using Waf.Writer.Applications.Views;
using Waf.Writer.Presentation.Views;

namespace Waf.Writer.Presentation.DesignData
{
    public class SampleMainViewModel : MainViewModel
    {
        public SampleMainViewModel()
            : base(new MockMainView() { ContentViewState = ContentViewState.StartViewVisible }, 
                 new MockShellService(), new MockFileService())
        {
            StartView = new StartView();
        }


        private class MockMainView : MockView, IMainView
        {
            public ContentViewState  ContentViewState { get; set; }
        }
    }
}
