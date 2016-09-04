using Jbe.NewsReader.Applications.Services;
using System.Collections.Generic;
using System.Composition;
using System.Threading.Tasks;
using Windows.Storage;

namespace Jbe.NewsReader.ExternalServices
{
    [Export(typeof(IAppDataService)), Export, Shared]
    public class AppDataService : IAppDataService
    {
        public IDictionary<string, object> LocalSettings => ApplicationData.Current.LocalSettings.Values;


        public async Task<T> LoadCompressedRoamingFileAsync<T>(string fileName) where T : class
        {
            await FileIOHelper.MigrateDataAsync(ApplicationData.Current.RoamingFolder, fileName);
            return await FileIOHelper.LoadCompressedAsync<T>(ApplicationData.Current.RoamingFolder, fileName);
        }

        public Task SaveCompressedRoamingFileAsync(object data, string fileName)
        {
            return FileIOHelper.SaveCompressedAsync(data, ApplicationData.Current.RoamingFolder, fileName);
        }
    }
}
