using System.Collections.Generic;
using System.Threading.Tasks;

namespace Jbe.NewsReader.Applications.Services
{
    public interface IAppDataService
    {
        IDictionary<string, object> LocalSettings { get; }


        Task<T> LoadCompressedRoamingFileAsync<T>(string fileName) where T : class;

        Task SaveCompressedRoamingFileAsync(object data, string fileName);
    }
}
