using Jbe.NewsReader.Applications.Services;
using System;
using System.Composition;
using System.Linq;
using System.Net;
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
                var feed = await client.RetrieveFeedAsync(uri).AsTask().ConfigureAwait(false);
                return new FeedDto(feed.Title.Text,
                    feed.Items.Select(x => new FeedItemDto(
                                x.ItemUri ?? x.Links.FirstOrDefault()?.Uri,
                                x.PublishedDate.UtcDateTime > new DateTime(1601, 1, 2) ? x.PublishedDate : x.LastUpdatedTime,  // 1601 is used when PublishedDate is not set
                                RemoveHtmlTags(x.Title.Text),
                                RemoveHtmlTags(x.Summary?.Text)
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
            message = WebUtility.HtmlDecode(message);
            return Regex.Replace(Regex.Replace(message, "\\&.{0,4}\\;", ""), "<.*?>", "");
        }
    }
}
