using Jbe.NewsReader.Applications.Services;
using System.Collections.Generic;
using System.Composition;
using System.IO;
using System.Threading.Tasks;

namespace Test.NewsReader.Applications.Services
{
    [Export(typeof(IAppDataService)), Export, Shared]
    public class MockAppDataService : IAppDataService
    {
        public IDictionary<string, object> LocalSettings { get; set; }


        public Task<Stream> GetFileStreamForReadAsync(string fileName)
        {
            return Task.FromResult((Stream)null);
        }

        public T LoadCompressedFile<T>(Stream archiveStream, string fileName) where T : class
        {
            return default(T);
        }

        public Task<T> LoadCompressedFileAsync<T>(string fileName) where T : class
        {
            return Task.FromResult(default(T));
        }

        public Task SaveCompressedFileAsync(object data, string fileName)
        {
            return Task.FromResult((object)null);
        }
    }
}
