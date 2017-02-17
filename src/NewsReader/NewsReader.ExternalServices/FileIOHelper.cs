using System;
using System.IO;
using System.IO.Compression;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using Windows.Storage;

namespace Jbe.NewsReader.ExternalServices
{
    internal static class FileIOHelper
    {
        public static string GetArchiveFileName(string fileName)
        {
            return fileName + ".zip";
        }

        public static T LoadCompressed<T>(Stream archiveStream, string fileName) where T : class
        {
            using (var archive = new ZipArchive(archiveStream, ZipArchiveMode.Read, leaveOpen: true))
            {
                var entry = archive.GetEntry(fileName);
                using (var stream = entry.Open())
                {
                    var serializer = new DataContractSerializer(typeof(T));
                    return (T)serializer.ReadObject(stream);
                }
            }
        }

        public static async Task<T> LoadCompressedAsync<T>(StorageFolder folder, string fileName) where T : class
        {
            if (folder == null) { throw new ArgumentNullException(nameof(folder)); }
            if (string.IsNullOrEmpty(fileName)) { throw new ArgumentException("String must not be null or empty.", nameof(fileName)); }

            try
            {
                using (var archiveStream = await folder.OpenStreamForReadAsync(GetArchiveFileName(fileName)).ConfigureAwait(false))
                {
                    return LoadCompressed<T>(archiveStream, fileName);
                }
            }
            catch (FileNotFoundException)
            {
                return null;
            }
        }

        public static async Task SaveCompressedAsync(object data, StorageFolder folder, string fileName)
        {
            if (data == null) { throw new ArgumentNullException(nameof(data)); }
            if (folder == null) { throw new ArgumentNullException(nameof(folder)); }
            if (string.IsNullOrEmpty(fileName)) { throw new ArgumentException("String must not be null or empty.", nameof(fileName)); }

            using (var archiveStream = await folder.OpenStreamForWriteAsync(GetArchiveFileName(fileName), CreationCollisionOption.ReplaceExisting))
            using (var archive = new ZipArchive(archiveStream, ZipArchiveMode.Create, leaveOpen: true))
            {
                var entry = archive.CreateEntry(fileName, CompressionLevel.Optimal);
                entry.LastWriteTime = new DateTimeOffset(2000, 1, 1, 0, 0, 0, TimeSpan.Zero);
                using (var stream = entry.Open())
                {
                    var serializer = new DataContractSerializer(data.GetType());
                    serializer.WriteObject(stream, data);
                    await stream.FlushAsync().ConfigureAwait(false);
                }
            }
        }
    }
}
