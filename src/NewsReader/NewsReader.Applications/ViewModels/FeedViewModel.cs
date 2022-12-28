using System.Waf.Applications;
using System.Windows.Input;
using Waf.NewsReader.Applications.Views;
using Waf.NewsReader.Domain;
using Waf.NewsReader.Domain.Foundation;

namespace Waf.NewsReader.Applications.ViewModels;

public class FeedViewModel : ViewModelCore<IFeedView>
{
    private readonly ThrottledAction updateSearchAction;
    private Feed? feed;
    private string searchText = "";

    public FeedViewModel(IFeedView view) : base(view, false)
    {
        updateSearchAction = new ThrottledAction(UpdateSearch);
        UpdateItemsListView();
    }

    public ICommand RefreshCommand { get; set; } = null!;

    public ICommand ReadUnreadCommand { get; set; } = null!;

    public ICommand ShowFeedItemViewCommand { get; set; } = null!;

    public ObservableGroupedListView<DateTime, FeedItem> ItemsListView { get; private set; } = null!;

    public Feed? Feed
    {
        get => feed;
        set
        {
            if (!SetProperty(ref feed, value)) return;
            SearchText = "";
            UpdateItemsListView();
        }
    }

    public string SearchText
    {
        get => searchText;
        set
        {
            if (!SetProperty(ref searchText, value)) return;
            updateSearchAction.InvokeAccumulated();
        }
    }

    private void UpdateSearch() => ItemsListView.Refresh();

    private bool FilterFeedItems(FeedItem item) => string.IsNullOrEmpty(SearchText)
        || (item.Name ?? "").Contains(SearchText, StringComparison.CurrentCultureIgnoreCase)
        || (item.Description ?? "").Contains(SearchText, StringComparison.CurrentCultureIgnoreCase);

    private void UpdateItemsListView()
    {
        ItemsListView = new ObservableGroupedListView<DateTime, FeedItem>(Feed?.Items ?? (IReadOnlyList<FeedItem>)Array.Empty<FeedItem>(),
            x => x.GroupBy(y => y.Date.LocalDateTime.Date)) { Filter = FilterFeedItems };
        RaisePropertyChanged(nameof(ItemsListView));
    }
}
