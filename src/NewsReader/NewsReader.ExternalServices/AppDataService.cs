using Jbe.NewsReader.Applications.Services;
using System.Collections.Generic;
using System.Composition;
using System.IO;
using System.Threading.Tasks;
using Windows.Storage;

namespace Jbe.NewsReader.ExternalServices
{
    [Export(typeof(IAppDataService)), Export, Shared]
    public class AppDataService : IAppDataService
    {
        public IDictionary<string, object> LocalSettings => ApplicationData.Current.LocalSettings.Values;


        public async Task<Stream> GetFileStreamForReadAsync(string fileName)
        {
            var folder = ApplicationData.Current.LocalFolder;
            return await folder.OpenStreamForReadAsync(FileIOHelper.GetArchiveFileName(fileName));
        }

        public T LoadCompressedFile<T>(Stream archiveStream, string fileName) where T : class
        {
            return FileIOHelper.LoadCompressed<T>(archiveStream, fileName);
        }

        public async Task<T> LoadCompressedFileAsync<T>(string fileName) where T : class
        {
            await FileIOHelper.MigrateDataAsync(ApplicationData.Current.LocalFolder, fileName);
            return await FileIOHelper.LoadCompressedAsync<T>(ApplicationData.Current.LocalFolder, fileName);
        }

        public Task SaveCompressedFileAsync(object data, string fileName)
        {
            return FileIOHelper.SaveCompressedAsync(data, ApplicationData.Current.LocalFolder, fileName);
        }
    }
}
