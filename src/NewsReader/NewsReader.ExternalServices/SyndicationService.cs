using Jbe.NewsReader.Applications.Services;
using System;
using System.Composition;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Windows.Web;
using Windows.Web.Syndication;

namespace Jbe.NewsReader.ExternalServices
{
    [Export(typeof(ISyndicationService)), Export, Shared]
    public class SyndicationService : ISyndicationService
    {
        public Applications.Services.ISyndicationClient CreateClient()
        {
            return new SyndicationClientAdapter(new SyndicationClient());
        }
    }

    internal class SyndicationClientAdapter : Applications.Services.ISyndicationClient
    {
        private readonly SyndicationClient client;

        public SyndicationClientAdapter(SyndicationClient client)
        {
            this.client = client;
        }

        public async Task<FeedDto> RetrieveFeedAsync(Uri uri)
        {
            try
            {
                var feed = await client.RetrieveFeedAsync(uri);
                return new FeedDto(feed.Title.Text,
                    feed.Items.Select(x => new FeedItemDto(
                                x.ItemUri ?? x.Links.FirstOrDefault()?.Uri,
                                x.PublishedDate,
                                x.Title.Text,
                                RemoveHtmlTags(x.Summary?.Text),
                                x.Authors.FirstOrDefault()?.NodeValue
                            )).ToArray()
                );
            }
            catch (Exception ex) when (WebError.GetStatus(ex.HResult) == WebErrorStatus.NotModified)
            {
                throw new SyndicationServiceException(SyndicationServiceError.NotModified, ex);
            }
        }

        private static string RemoveHtmlTags(string message)
        {
            if (string.IsNullOrEmpty(message)) { return message; }
            return Regex.Replace(Regex.Replace(message, "\\&.{0,4}\\;", ""), "<.*?>", "");
        }
    }
}
