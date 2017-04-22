using Jbe.NewsReader.Applications.Services;
using Jbe.NewsReader.Applications.Views;
using Jbe.NewsReader.Domain;
using System;
using System.Composition;
using System.Waf.Applications;
using System.Windows.Input;

namespace Jbe.NewsReader.Applications.ViewModels
{
    [Export, Shared]
    public class FeedListViewModel : ViewModelCore<IFeedListView>
    {
        private FeedManager feedManager;
        private string addNewFeedUri;
        private string loadErrorMessage;


        [ImportingConstructor]
        public FeedListViewModel(IFeedListView view, SelectionService selectionService, INavigationService navigationService) : base(view)
        {
            SelectionService = selectionService;
            navigationService.Navigated += NavigationServiceNavigated;
            AddNewFeedUri = null;
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
            set
            {
                if (SetProperty(ref addNewFeedUri, value ?? @"http://"))
                {
                    LoadErrorMessage = null;
                }
            }
        }
        
        public string LoadErrorMessage
        {
            get { return loadErrorMessage; }
            set { SetProperty(ref loadErrorMessage, value); }
        }
        

        public void FeedAdded()
        {
            AddNewFeedUri = null;
            ViewCore.FeedAdded();
        }

        private void NavigationServiceNavigated(object sender, EventArgs e) => ViewCore.CancelMultipleSelectionMode();
    }
}
