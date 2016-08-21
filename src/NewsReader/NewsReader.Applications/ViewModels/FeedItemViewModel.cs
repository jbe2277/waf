using Jbe.NewsReader.Applications.Services;
using Jbe.NewsReader.Applications.Views;
using System.Composition;
using System.Waf.Applications;
using System.Windows.Input;

namespace Jbe.NewsReader.Applications.ViewModels
{
    [Export, Shared]
    public class FeedItemViewModel : ViewModelCore<IFeedItemView>
    {
        [ImportingConstructor]
        public FeedItemViewModel(IFeedItemView view, SelectionService selectionService) : base(view)
        {
            SelectionService = selectionService;
        }


        public SelectionService SelectionService { get; }

        public ICommand LaunchWebBrowserCommand { get; set; }
    }
}
