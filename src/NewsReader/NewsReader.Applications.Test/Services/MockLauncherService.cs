using Jbe.NewsReader.Applications.Services;
using System;
using System.Composition;
using System.Threading.Tasks;

namespace Test.NewsReader.Applications.Services
{
    [Export(typeof(ILauncherService)), Export, Shared]
    public class MockLauncherService : ILauncherService
    {
        public Func<Task<bool>> LaunchReviewAsyncStub { get; set; }

        public Func<Task<bool>> LaunchStoreAsyncStub { get; set; }

        public Func<Task<bool>> LaunchUriAsyncStub { get; set; }


        public Task<bool> LaunchReviewAsync()
        {
            return LaunchReviewAsyncStub?.Invoke() ?? Task.FromResult(true);
        }

        public Task<bool> LaunchStoreAsync()
        {
            return LaunchStoreAsyncStub?.Invoke() ?? Task.FromResult(true);
        }

        public Task<bool> LaunchUriAsync(Uri uri)
        {
            return LaunchUriAsyncStub?.Invoke() ?? Task.FromResult(true);
        }
    }
}
