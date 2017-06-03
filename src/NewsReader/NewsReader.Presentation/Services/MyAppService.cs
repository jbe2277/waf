using System;
using Windows.ApplicationModel.AppService;
using Windows.ApplicationModel.Background;
using Windows.Foundation.Collections;

namespace Jbe.NewsReader.Presentation.Services
{
    public class MyAppService : IBackgroundTask
    {
        public void Run(IBackgroundTaskInstance taskInstance)
        {
            var deferral = taskInstance.GetDeferral();
            taskInstance.Canceled += (sender, e) => deferral.Complete();
            
            var trigger = (AppServiceTriggerDetails)taskInstance.TriggerDetails;
            var connection = trigger.AppServiceConnection;
            connection.RequestReceived += ConnectionRequestReceived;
        }

        private async void ConnectionRequestReceived(AppServiceConnection sender, AppServiceRequestReceivedEventArgs args)
        {
            var deferral = args.GetDeferral();
            try
            {
                var value = (int)args.Request.Message["value"];
                await args.Request.SendResponseAsync(new ValueSet { { "result", value * 2 } });
            }
            finally
            {
                deferral.Complete();
            }
        }
    }
}
