﻿using System.ServiceModel.Syndication;
using System.Text.RegularExpressions;
using System.Web;
using System.Xml;
using Waf.NewsReader.Applications.Services;

namespace Waf.NewsReader.Presentation.Services;

public partial class SyndicationService : ISyndicationService
{
    public async Task<FeedDto> RetrieveFeed(Uri uri)
    {
        using var client = new HttpClient();
        using var stream = await client.GetStreamAsync(uri).ConfigureAwait(false);
        using var xmlReader = XmlReader.Create(stream);
        var feed = SyndicationFeed.Load(xmlReader);
        return new FeedDto(feed.Title.Text, feed.Items.Select(x => new FeedItemDto(
            Uri.TryCreate(x.Id, UriKind.Absolute, out var uri) ? uri : x.Links.FirstOrDefault()?.Uri, 
            x.PublishDate, RemoveHtmlTags(x.Title.Text), RemoveHtmlTags(x.Summary.Text))).ToArray());
    }

    private static string RemoveHtmlTags(string message)
    {
        if (string.IsNullOrEmpty(message)) return message;
        message = HtmlTagsRegex().Replace(message, "");
        return HttpUtility.HtmlDecode(message);
    }

    [GeneratedRegex("<.*?>")]
    private static partial Regex HtmlTagsRegex();
}
