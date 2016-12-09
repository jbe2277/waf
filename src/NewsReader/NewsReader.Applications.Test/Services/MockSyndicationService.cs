using Jbe.NewsReader.Applications.Services;
using System;
using System.Composition;
using System.Threading.Tasks;

namespace Test.NewsReader.Applications.Services
{
    [Export(typeof(ISyndicationService)), Export, Shared]
    public class MockSyndicationService : ISyndicationService
    {
        public Func<ISyndicationClient> CreateClientStub { get; set; }

        public ISyndicationClient LastCreatedClient { get; private set; }

        public MockSyndicationClient LastCreatedMockClient => LastCreatedClient as MockSyndicationClient;
        

        public void SetEmptyClientStub()
        {
            CreateClientStub = () =>
            {
                return new MockSyndicationClient()
                {
                    RetrieveFeedAsyncStub = uri =>
                    {
                        return Task.FromResult(new FeedDto("Empty feed", new FeedItemDto[0]));
                    }
                };
            };
        }

        public ISyndicationClient CreateClient()
        {
            LastCreatedClient = CreateClientStub?.Invoke() ?? new MockSyndicationClient();
            return LastCreatedClient;
        }
    }

    public class MockSyndicationClient : ISyndicationClient
    {
        public Func<Uri, Task<FeedDto>> RetrieveFeedAsyncStub { get; set; }


        public static FeedDto CreateSampleFeed()
        {
            return new FeedDto("Sample Feed", new[]
            {
                new FeedItemDto(new Uri("http://www.test.com/rss/feed1"), new DateTimeOffset(2020, 5, 5, 12, 0, 0, new TimeSpan(1, 0, 0)), "name", "desc", "author"),
                new FeedItemDto(new Uri("http://www.test.com/rss/feed2"), new DateTimeOffset(2020, 5, 5, 16, 0, 0, new TimeSpan(1, 0, 0)), "name2", "desc2", "author2"),
                new FeedItemDto(new Uri("http://www.test.com/rss/feed3"), new DateTimeOffset(2020, 5, 6, 9, 0, 0, new TimeSpan(1, 0, 0)), "name3", "desc3", "author3")
            });
        }

        public Task<FeedDto> RetrieveFeedAsync(Uri uri)
        {
            return RetrieveFeedAsyncStub?.Invoke(uri) ?? Task.FromResult(CreateSampleFeed());
        }
    }
}
