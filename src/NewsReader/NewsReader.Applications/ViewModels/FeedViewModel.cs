using System;
using System.Collections.Generic;
using System.Linq;
using System.Waf.Foundation;
using System.Windows.Input;
using Waf.NewsReader.Applications.Views;
using Waf.NewsReader.Domain;
using Waf.NewsReader.Domain.Foundation;

namespace Waf.NewsReader.Applications.ViewModels
{
    public class FeedViewModel : ViewModel<IFeedView>
    {
        private readonly ThrottledAction updateSearchAction;
        private Feed feed;
        private string searchText = "";

        public FeedViewModel(IFeedView view) : base(view)
        {
            updateSearchAction = new ThrottledAction(UpdateSearch);
            UpdateItemsListView();
        }

        public ICommand RefreshCommand { get; set; }

        public ICommand ReadUnreadCommand { get; set; }

        public ICommand ShowFeedItemViewCommand { get; set; }

        public ObservableGroupedListView<DateTime, FeedItem> ItemsListView { get; private set; }

        public Feed Feed
        {
            get => feed;
            set
            {
                if (SetProperty(ref feed, value))
                {
                    SearchText = "";
                    UpdateItemsListView();
                }
            }
        }

        public string SearchText
        {
            get => searchText;
            set
            {
                if (SetProperty(ref searchText, value))
                {
                    updateSearchAction.InvokeAccumulated();
                }
            }
        }


        private void UpdateSearch()
        {
            ItemsListView.Refresh();
        }

        private bool FilterFeedItems(FeedItem item)
        {
            return string.IsNullOrEmpty(SearchText)
                || (item.Name ?? "").Contains(SearchText, StringComparison.CurrentCultureIgnoreCase)
                || (item.Description ?? "").Contains(SearchText, StringComparison.CurrentCultureIgnoreCase);
        }

        private void UpdateItemsListView()
        {
            ItemsListView = new ObservableGroupedListView<DateTime, FeedItem>(
                Feed?.Items ?? (IReadOnlyList<FeedItem>)Array.Empty<FeedItem>(),
                x => x.GroupBy(y => y.Date.LocalDateTime.Date))
            {
                Filter = FilterFeedItems
            };
            RaisePropertyChanged(nameof(ItemsListView));
        }
    }
}
