using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Jbe.NewsReader.Applications.Services
{
    public interface IAppDataService
    {
        IDictionary<string, object> LocalSettings { get; }


        Task<Stream> GetFileStreamForReadAsync(string fileName);

        T LoadCompressedFile<T>(Stream archiveStream, string fileName) where T : class;

        Task<T> LoadCompressedFileAsync<T>(string fileName) where T : class;

        Task SaveCompressedFileAsync(object data, string fileName);
    }
}
