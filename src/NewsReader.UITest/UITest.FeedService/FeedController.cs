using Microsoft.AspNetCore.Mvc;
using System.ServiceModel.Syndication;
using System.Xml;

namespace UITest.FeedService;

[Route("feed")]
public class FeedController(SyndicationData data) : Controller
{
    [HttpGet("rss")]
    public IActionResult Rss()
    {
        using var stream = new MemoryStream();
        using (var writer = XmlWriter.Create(stream, new() { Indent = true }))
        {
            var rssFormatter = new Rss20FeedFormatter(data.Feed);
            rssFormatter.WriteTo(writer);
        }
        stream.Position = 0;
        return File(stream.ToArray(), "application/rss+xml");
    }

    [HttpGet("atom")]
    public IActionResult Atom()
    {
        using var stream = new MemoryStream();
        using (var writer = XmlWriter.Create(stream, new() { Indent = true }))
        {
            var atomFormatter = new Atom10FeedFormatter(data.Feed);
            atomFormatter.WriteTo(writer);
        }
        stream.Position = 0;
        return File(stream.ToArray(), "application/atom+xml");
    }
}
