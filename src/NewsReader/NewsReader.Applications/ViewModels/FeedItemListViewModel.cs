using Jbe.NewsReader.Applications.Services;
using Jbe.NewsReader.Applications.Views;
using Jbe.NewsReader.Domain;
using Jbe.NewsReader.Domain.Foundation;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Composition;
using System.Linq;
using System.Waf.Applications;
using System.Waf.Foundation;
using System.Windows.Input;

namespace Jbe.NewsReader.Applications.ViewModels
{
    [Export, Shared]
    public class FeedItemListViewModel : ViewModelCore<IFeedItemListView>
    {
        private ObservableGroupedListView<DateTime, FeedItem> itemsListView;
        private string searchText = "";


        [ImportingConstructor]
        public FeedItemListViewModel(IFeedItemListView view, SelectionService selectionService, INavigationService navigationService) : base(view)
        {
            SelectionService = selectionService;
            SelectionService.PropertyChanged += SelectionServicePropertyChanged;
            navigationService.Navigated += NavigationServiceNavigated;
            UpdateItemsListView();
        }


        public SelectionService SelectionService { get; }

        public ICommand RefreshCommand { get; set; }

        public ICommand ReadUnreadCommand { get; set; }

        public ICommand ShowFeedItemViewCommand { get; set; }

        public ObservableGroupedListView<DateTime, FeedItem> ItemsListView => itemsListView;

        public string SearchText
        {
            get => searchText;
            set
            {
                if (SetProperty(ref searchText, value))
                {
                    var selectedFeedItems = SelectionService.SelectedFeedItems.ToArray();
                    itemsListView.Refresh();  // The UI resets the selection because of the CollectionChanged Reset event
                    if (SelectionService.SelectedFeedItem == null)
                    {
                        var currentItems = itemsListView.SelectMany(x => x).ToArray();
                        foreach (var item in selectedFeedItems.Where(x => currentItems.Contains(x))) SelectionService.SelectedFeedItems.Add(item);
                    }
                }
            }
        }


        private bool FilterFeedItems(FeedItem item)
        {
            return string.IsNullOrEmpty(SearchText)
                || (item.Name ?? "").Contains(SearchText, StringComparison.CurrentCultureIgnoreCase)
                || (item.Description ?? "").Contains(SearchText, StringComparison.CurrentCultureIgnoreCase);
        }

        private void UpdateItemsListView()
        {
            itemsListView = new ObservableGroupedListView<DateTime, FeedItem>(
                SelectionService.SelectedFeed?.Items ?? (IReadOnlyList<FeedItem>)Array.Empty<FeedItem>(),
                x => x.GroupBy(y => y.Date.LocalDateTime.Date))
            {
                Filter = FilterFeedItems
            };
            RaisePropertyChanged(nameof(ItemsListView));
        }

        private void SelectionServicePropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(SelectionService.SelectedFeed))
            {
                ViewCore.CancelMultipleSelectionMode();
                SearchText = "";
                UpdateItemsListView();
            }
        }

        private void NavigationServiceNavigated(object sender, EventArgs e)
        {
            ViewCore.CancelMultipleSelectionMode();
            SearchText = "";
        }
    }
}