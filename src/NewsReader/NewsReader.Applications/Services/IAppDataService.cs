using System.Collections.Generic;
using System.Threading.Tasks;

namespace Jbe.NewsReader.Applications.Services
{
    public interface IAppDataService
    {
        IDictionary<string, object> LocalSettings { get; }


        Task<T> LoadCompressedFileAsync<T>(string fileName) where T : class;

        Task SaveCompressedFileAsync(object data, string fileName);
    }
}
