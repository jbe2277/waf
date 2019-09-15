using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Waf.NewsReader.Applications.Services
{
    public interface ISyndicationService
    {
        Task<FeedDto> RetrieveFeed(Uri uri);
    }

    public sealed class FeedDto
    {
        public FeedDto(string title, IReadOnlyList<FeedItemDto> items)
        {
            Title = title;
            Items = items;
        }

        public string Title { get; }

        public IReadOnlyList<FeedItemDto> Items { get; }
    }

    public sealed class FeedItemDto
    {
        public FeedItemDto(Uri uri, DateTimeOffset date, string name, string description)
        {
            Uri = uri;
            Date = date;
            Name = name;
            Description = description;
        }

        public Uri Uri { get; }

        public DateTimeOffset Date { get; }

        public string Name { get; }

        public string Description { get; }
    }
}
