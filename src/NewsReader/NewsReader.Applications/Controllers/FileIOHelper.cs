using System;
using System.IO;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using Windows.Storage;

namespace Jbe.NewsReader.Applications.Controllers
{
    public static class FileIOHelper
    {
        public static async Task<T> LoadAsync<T>(StorageFolder folder, string fileName) where T : class
        {
            if (folder == null) { throw new ArgumentNullException(nameof(folder)); }
            if (string.IsNullOrEmpty(fileName)) { throw new ArgumentException("String must not be null or empty.", nameof(fileName)); }

            try
            {
                using (var stream = await folder.OpenStreamForReadAsync(fileName))
                {
                    var serializer = new DataContractSerializer(typeof(T));
                    return (T)serializer.ReadObject(stream);
                }
            }
            catch (FileNotFoundException)
            {
                return null;
            }
        }

        public static async Task SaveAsync(object data, StorageFolder folder, string fileName)
        {
            if (data == null) { throw new ArgumentNullException(nameof(data)); }
            if (folder == null) { throw new ArgumentNullException(nameof(folder)); }
            if (string.IsNullOrEmpty(fileName)) { throw new ArgumentException("String must not be null or empty.", nameof(fileName)); }

            using (var stream = await folder.OpenStreamForWriteAsync(fileName, CreationCollisionOption.ReplaceExisting))
            {
                var serializer = new DataContractSerializer(data.GetType());
                serializer.WriteObject(stream, data);
                await stream.FlushAsync();
            }
        }
    }
}
