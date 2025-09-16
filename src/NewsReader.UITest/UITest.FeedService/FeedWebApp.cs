using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System.ServiceModel.Syndication;
using System.Xml;

namespace UITest.FeedService;

public static class FeedWebApp
{
    public static IAsyncDisposable RunService(SyndicationData feed, string? address)
    {
        var builder = WebApplication.CreateBuilder();
        builder.Services.AddSingleton(feed);
        var app = builder.Build();

        address ??= "http://localhost:5000";
        app.Urls.Add(address);

        var feedGroup = app.MapGroup("/feed");
        feedGroup.MapGet("/rss", GetFeedRss);
        feedGroup.MapGet("/atom", GetFeedAtom);

        return new AppAsyncDisposable(app, app.RunAsync());
    }

    private static IResult GetFeedRss(SyndicationData data)
    {
        var stream = new MemoryStream();
        using (var writer = XmlWriter.Create(stream, new() { Indent = true }))
        {
            var rssFormatter = new Rss20FeedFormatter(data.Feed);
            rssFormatter.WriteTo(writer);
        }
        stream.Position = 0;
        return TypedResults.Stream(stream, "application/rss+xml");
    }

    private static IResult GetFeedAtom(SyndicationData data)
    {
        var stream = new MemoryStream();
        using (var writer = XmlWriter.Create(stream, new() { Indent = true }))
        {
            var atomFormatter = new Atom10FeedFormatter(data.Feed);
            atomFormatter.WriteTo(writer);
        }
        stream.Position = 0;
        return TypedResults.Stream(stream, "application/atom+xml");
    }


    private sealed class AppAsyncDisposable(WebApplication app, Task appTask) : IAsyncDisposable
    {
        public async ValueTask DisposeAsync()
        {
            await app.StopAsync().ConfigureAwait(false);
            await appTask.ConfigureAwait(false);
        }
    }
}
