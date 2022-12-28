using System.Runtime.Serialization;

namespace Waf.NewsReader.Domain;

[DataContract]
public class FeedManager : Model, IDataManager
{
    [DataMember] private readonly ObservableCollection<Feed> feeds;
    [DataMember] private TimeSpan? itemLifetime;
    [DataMember] private uint? maxItemsLimit;

    public FeedManager()
    {
        // Note: Serializer does not call the constructor.
        feeds = new ObservableCollection<Feed> { new Feed(new Uri("https://devblogs.microsoft.com/dotnet/category/maui/feed/")) };
        ItemLifetime = TimeSpan.FromDays(365);
        MaxItemsLimit = 250;
        Initialize();
    }

    public ObservableCollection<Feed> Feeds => feeds;

    public TimeSpan? ItemLifetime
    {
        get => itemLifetime;
        set => SetProperty(ref itemLifetime, value);
    }

    public uint? MaxItemsLimit
    {
        get => maxItemsLimit;
        set => SetProperty(ref maxItemsLimit, value);
    }

    public void Merge(FeedManager newFeedManager)
    {
        ItemLifetime = newFeedManager.ItemLifetime;
        MaxItemsLimit = newFeedManager.MaxItemsLimit;
        CollectionHelper.Merge(Feeds, newFeedManager.Feeds, FeedEqualityComparer.Default);
        foreach (var feed in Feeds)
        {
            var newFeed = newFeedManager.Feeds.Single(x => x.Uri == feed.Uri);
            feed.UpdateItems(newFeed.Items, cloneItemsBeforeInsert: true);
        }
    }

    private void Initialize()
    {
        feeds.CollectionChanged += FeedsCollectionChanged;
        foreach (var x in feeds) x.DataManager = this;
    }

    private void FeedsCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        if (e.Action == NotifyCollectionChangedAction.Reset) throw new NotSupportedException("The Reset action is not supported.");
        foreach (var x in e.NewItems?.Cast<Feed>() ?? Array.Empty<Feed>()) x.DataManager = this;
        foreach (var x in e.OldItems?.Cast<Feed>() ?? Array.Empty<Feed>()) x.DataManager = null;
    }

    [OnDeserialized]
    private void OnDeserialized(StreamingContext context) => Initialize();


    internal sealed class FeedEqualityComparer : IEqualityComparer<Feed>
    {
        public static FeedEqualityComparer Default { get; } = new FeedEqualityComparer();

        public bool Equals(Feed? x, Feed? y) => x?.Uri == y?.Uri;

        public int GetHashCode(Feed obj) => obj?.Uri?.GetHashCode() ?? 0;
    }
}
