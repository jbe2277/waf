using Jbe.NewsReader.Applications.Services;
using System;
using System.Composition;
using System.Threading.Tasks;

namespace Jbe.NewsReader.Presentation.Services
{
    [Export(typeof(IAppService)), Shared]
    public class AppService : IAppService
    {
        public Task DelayIdleAsync()
        {
            return Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunIdleAsync(ha => { }).AsTask();
        }
    }
}
