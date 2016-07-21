using Jbe.NewsReader.Applications.Services;
using System;
using System.Composition;
using System.Threading.Tasks;

namespace Jbe.NewsReader.Presentation.Services
{
    [Export(typeof(IAppService)), Shared]
    public class AppService : IAppService
    {
        public async Task DelayIdleAsync()
        {
            await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunIdleAsync(ha => { });
        }
    }
}
