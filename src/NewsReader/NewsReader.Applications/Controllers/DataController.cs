using Microsoft.AppCenter.Crashes;
using System;
using System.Threading.Tasks;
using Waf.NewsReader.Applications.Services;
using Waf.NewsReader.Domain;

namespace Waf.NewsReader.Applications.Controllers
{
    internal class DataController
    {
        private readonly IDataService dataService;
        private readonly TaskCompletionSource<FeedManager> loadCompletion;

        public DataController(IDataService dataService)
        {
            this.dataService = dataService;
            loadCompletion = new TaskCompletionSource<FeedManager>();
        }

        public async Task<FeedManager> Load()
        {
            FeedManager feedManager = null;
            try
            {
                await Task.Run(() =>
                {
                    feedManager = dataService.Load<FeedManager>() ?? new FeedManager();
                });
            }
            catch (Exception ex)
            {
                // Better to forget the settings (data loss) as to never start the app again
                Log.Default.Error(ex, "DataController.Load");
                Crashes.TrackError(ex);
                feedManager = new FeedManager();
            }
            loadCompletion.SetResult(feedManager);
            return feedManager;
        }

        public Task Update()
        {
            // TODO: Download and update
            return Task.CompletedTask;
        }

        public Task Save()
        {
            if (!loadCompletion.Task.IsCompleted) return Task.CompletedTask;
            var feedManager = loadCompletion.Task.GetAwaiter().GetResult();
            dataService.Save(feedManager);
            // TODO: Upload here
            return Task.CompletedTask;
        }
    }
}
