using Jbe.NewsReader.Applications.Services;
using System.Composition;
using System.Threading.Tasks;
using System.IO;

namespace Test.NewsReader.Applications.Services
{
    [Export(typeof(IWebStorageService)), Export, Shared]
    public class MockWebStorageService : IWebStorageService
    {
        public Task<string> DownloadFileAsync(string fileName, Stream destination, string token, string eTag)
        {
            return Task.FromResult("DummyETag");
        }

        public Task<string> UploadFileAsync(Stream source, string fileName, string token)
        {
            return Task.FromResult("DummyETag");
        }
    }
}
