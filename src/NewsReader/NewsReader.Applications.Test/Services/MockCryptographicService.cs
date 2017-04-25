using Jbe.NewsReader.Applications.Services;
using System.Composition;
using System.Threading.Tasks;
using System.IO;

namespace Test.NewsReader.Applications.Services
{
    [Export(typeof(ICryptographicService)), Export, Shared]
    public class MockCryptographicService : ICryptographicService
    {
        public Task<Stream> DecryptAsync(Stream stream, string key, string salt, uint iterationCount)
        {
            return Task.FromResult((Stream)null);
        }

        public Task<Stream> EncryptAsync(Stream stream, string key, string salt, uint iterationCount)
        {
            return Task.FromResult((Stream)null);
        }

        public Task<string> HashAsync(Stream stream)
        {
            return Task.FromResult("12345");
        }
    }
}
