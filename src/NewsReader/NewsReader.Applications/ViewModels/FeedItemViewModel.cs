using System.Waf.Applications;
using System.Windows.Input;
using Waf.NewsReader.Applications.Views;
using Waf.NewsReader.Domain;

namespace Waf.NewsReader.Applications.ViewModels;

public class FeedItemViewModel(IFeedItemView view) : ViewModelCore<IFeedItemView>(view, false)
{
    private FeedItem? feedItem;

    public ICommand LaunchBrowserCommand { get; set; } = null!;

    public FeedItem? FeedItem { get => feedItem; set => SetProperty(ref feedItem, value); }
}
