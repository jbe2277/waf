namespace Waf.NewsReader.Applications.Services;

public interface ISyndicationService
{
    Task<FeedDto> RetrieveFeed(Uri uri);
}

public sealed record FeedDto(string Title, IReadOnlyList<FeedItemDto> Items, IReadOnlyList<Exception> Errors);

public sealed record FeedItemDto(Uri? Uri, DateTimeOffset Date, string Name, string Description);
