using System.IO.Compression;
using System.Runtime.Serialization;
using System.Security.Cryptography;
using Waf.NewsReader.Applications.Services;

namespace Waf.NewsReader.Presentation.Services;

public class DataService : IDataService
{
    private static readonly string containerFileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "data.zip");
    private const string itemFileName = "data.xml";

    public Stream GetReadStream()
    {
        return File.OpenRead(containerFileName);
    }

    public string GetHash()
    {
        using (var stream = GetReadStream())
        using (var sha1 = SHA1.Create())
        {
            return BitConverter.ToString(sha1.ComputeHash(stream)).Replace("-", "");
        }
    }

    public T? Load<T>(Stream? dataStream = null) where T : class
    {
        try
        {
            using var archiveStream = dataStream ?? GetReadStream();
            return LoadItem<T>(archiveStream, itemFileName);
        }
        catch (FileNotFoundException)
        {
            return null;
        }
    }

    private static T LoadItem<T>(Stream archiveStream, string fileName) where T : class
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

    public void Save(object data)
    {
        if (data == null) throw new ArgumentNullException(nameof(data));

        using (var archiveStream = File.Create(containerFileName))
        using (var archive = new ZipArchive(archiveStream, ZipArchiveMode.Create, leaveOpen: true))
        {
            var entry = archive.CreateEntry(itemFileName, CompressionLevel.Optimal);
            // Set always the same write time -> only content changes should result in a different hash.
            entry.LastWriteTime = new DateTimeOffset(2000, 1, 1, 0, 0, 0, TimeSpan.Zero);
            using (var stream = entry.Open())
            {
                var serializer = new DataContractSerializer(data.GetType());
                serializer.WriteObject(stream, data);
            }
        }
    }
}
