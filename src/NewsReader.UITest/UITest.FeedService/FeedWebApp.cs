using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.Extensions.DependencyInjection;

namespace UITest.FeedService;

public static class FeedWebApp
{
    public static async Task RunService(SyndicationData feed, string? address, CancellationToken cancellation)
    {
        var builder = WebApplication.CreateBuilder();
        builder.Services.AddSingleton(feed);
        var part = new AssemblyPart(typeof(FeedWebApp).Assembly);
        builder.Services.AddControllers().ConfigureApplicationPartManager(x => x.ApplicationParts.Add(part));

        var app = builder.Build();
        app.MapControllers();

        address ??= "http://localhost:5000";
        var serviceTask = app.RunAsync(address);

        try
        {
            await Task.Delay(Timeout.InfiniteTimeSpan, cancellation).ConfigureAwait(false);
        }
        catch (OperationCanceledException) { }
        finally
        {
            try
            {
                await Task.WhenAll(app.StopAsync(CancellationToken.None), serviceTask).ConfigureAwait(false);
            }
            catch (OperationCanceledException) { }
        }
    }
}
