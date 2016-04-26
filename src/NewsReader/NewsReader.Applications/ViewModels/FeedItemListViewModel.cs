using Jbe.NewsReader.Applications.Services;
using Jbe.NewsReader.Applications.Views;
using System.Composition;
using System.Waf.Applications;
using System.Windows.Input;

namespace Jbe.NewsReader.Applications.ViewModels
{
    [Export, Shared]
    public class FeedItemListViewModel : ViewModel<IFeedItemListView>
    {
        [ImportingConstructor]
        public FeedItemListViewModel(IFeedItemListView view, SelectionService selectionService) : base(view)
        {
            SelectionService = selectionService;
        }


        public SelectionService SelectionService { get; }

        public ICommand ReadUnreadCommand { get; set; }

        public ICommand ShowFeedItemViewCommand { get; set; }
    }
}
