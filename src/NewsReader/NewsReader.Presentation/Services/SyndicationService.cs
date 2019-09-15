using HtmlAgilityPack;
using Microsoft.SyndicationFeed;
using Microsoft.SyndicationFeed.Atom;
using Microsoft.SyndicationFeed.Rss;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml;
using Waf.NewsReader.Applications.Services;

namespace Waf.NewsReader.Presentation.Services
{
    public class SyndicationService : ISyndicationService
    {
        public async Task<FeedDto> RetrieveFeed(Uri uri)
        {
            string title = "";
            var items = new List<ISyndicationItem>();

            using (var client = new HttpClient())
            using (var stream = await client.GetStreamAsync(uri).ConfigureAwait(false))
            using (var xmlReader = XmlReader.Create(stream, new XmlReaderSettings { Async = true }))
            {
                var feedReader = GetReader(xmlReader);
                while (await feedReader.Read().ConfigureAwait(false))
                {
                    switch (feedReader.ElementType)
                    {
                        case SyndicationElementType.Item:
                            items.Add(await feedReader.ReadItem().ConfigureAwait(false));
                            break;
                        case SyndicationElementType.Content:
                            ISyndicationContent content = await feedReader.ReadContent().ConfigureAwait(false);
                            if (content.Name == "title") title = content.Value;
                            break;
                    }
                }
            }

            return new FeedDto(title, items.Select(x => new FeedItemDto(x.Links.FirstOrDefault()?.Uri, x.Published, 
                RemoveHtmlTags(x.Title), RemoveHtmlTags(x.Description))).ToArray());
        }

        private static XmlFeedReader GetReader(XmlReader reader)
        {
            reader.MoveToContent();
            if (reader.Name == "feed") return new AtomFeedReader(reader);
            if (reader.Name == "rss") return new RssFeedReader(reader);
            throw new NotSupportedException("The feed type was not recognized.");
        }

        private static string RemoveHtmlTags(string message)
        {
            if (string.IsNullOrEmpty(message)) return message;
            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(message);
            return htmlDoc.DocumentNode.InnerText.Trim();
        }
    }
}
