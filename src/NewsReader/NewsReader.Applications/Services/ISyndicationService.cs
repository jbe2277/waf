namespace Waf.NewsReader.Applications.Services;

public interface ISyndicationService
{
    Task<FeedDto> RetrieveFeed(Uri uri);
}

public sealed class FeedDto
{
    public FeedDto(string title, IReadOnlyList<FeedItemDto> items, IReadOnlyList<Exception> errors)
    {
        Title = title;
        Items = items;
        Errors = errors;
    }

    public string Title { get; }

    public IReadOnlyList<FeedItemDto> Items { get; }

    public IReadOnlyList<Exception> Errors { get; }
}

public sealed class FeedItemDto
{
    public FeedItemDto(Uri? uri, DateTimeOffset date, string name, string description)
    {
        Uri = uri;
        Date = date;
        Name = name;
        Description = description;
    }

    public Uri? Uri { get; }

    public DateTimeOffset Date { get; }

    public string Name { get; }

    public string Description { get; }
}
