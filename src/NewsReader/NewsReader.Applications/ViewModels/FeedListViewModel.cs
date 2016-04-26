using Jbe.NewsReader.Applications.Services;
using Jbe.NewsReader.Applications.Views;
using Jbe.NewsReader.Domain;
using System.Composition;
using System.Waf.Applications;
using System.Windows.Input;

namespace Jbe.NewsReader.Applications.ViewModels
{
    [Export, Shared]
    public class FeedListViewModel : ViewModel<IFeedListView>
    {
        private FeedManager feedManager;
        private string addNewFeedUri;


        [ImportingConstructor]
        public FeedListViewModel(IFeedListView view, SelectionService selectionService) : base(view)
        {
            SelectionService = selectionService;
        }


        public SelectionService SelectionService { get; }

        public FeedManager FeedManager
        {
            get { return feedManager; }
            set { SetProperty(ref feedManager, value); }
        }

        public ICommand AddNewFeedCommand { get; set; }

        public ICommand RemoveFeedCommand { get; set; }

        public ICommand ShowFeedItemListViewCommand { get; set; }

        public string AddNewFeedUri
        {
            get { return addNewFeedUri; }
            set { SetProperty(ref addNewFeedUri, value); }
        }


        public void FeedAdded()
        {
            AddNewFeedUri = null;
            ViewCore.FeedAdded();
        }
    }
}
