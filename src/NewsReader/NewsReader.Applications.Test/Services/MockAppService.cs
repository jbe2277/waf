using Jbe.NewsReader.Applications.Services;
using System;
using System.Composition;
using System.Threading.Tasks;

namespace Test.NewsReader.Applications.Services
{
    [Export(typeof(IAppService)), Export, Shared]
    public class MockAppService : IAppService
    {
        public Func<Task> DelayIdleAsyncStub { get; set; }


        public Task DelayIdleAsync()
        {
            return DelayIdleAsyncStub?.Invoke() ?? Task.Delay(1);
        }
    }
}
