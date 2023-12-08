using Waf.NewsReader.Applications.Services;

namespace Test.NewsReader.Applications.Services;

public class MockSyndicationService : ISyndicationService
{
    public Task<FeedDto> RetrieveFeed(Uri uri) => throw new NotImplementedException();
}
