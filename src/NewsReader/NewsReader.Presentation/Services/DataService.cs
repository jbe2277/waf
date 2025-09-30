using System.IO.Compression;
using System.Runtime.Serialization;
using System.Security.Cryptography;
using Waf.NewsReader.Applications.Services;

namespace Waf.NewsReader.Presentation.Services;

public class DataService : IDataService
{
    private static readonly string containerFileName = Path.Combine(FileSystem.AppDataDirectory, "data.zip");
    private const string itemFileName = "data.xml";

    public Stream GetReadStream() => File.OpenRead(containerFileName);

    public Stream GetWriteStream() => File.Create(containerFileName);

    public string GetHash(Stream dataStream)
    {
        using var sha1 = SHA1.Create();
        return Convert.ToHexString(sha1.ComputeHash(dataStream));
    }

    public async Task<T?> Load<T>(Stream dataStream) where T : class
    {
        Log.Default.Info("DataService.Load started.");
        using var memory = new MemoryStream();
        await dataStream.CopyToAsync(memory).ConfigureAwait(false);
        var result = LoadItem<T>(dataStream, itemFileName);
        Log.Default.Info("DataService.Load completed.");
        return result;
    }

    private static T LoadItem<T>(Stream archiveStream, string fileName) where T : class
    {
        using var archive = new ZipArchive(archiveStream, ZipArchiveMode.Read, leaveOpen: true);
        var entry = archive.GetEntry(fileName) ?? throw new InvalidOperationException($"Could not find {fileName} in archive.");
        using var stream = entry.Open();
        var serializer = new DataContractSerializer(typeof(T));
        return (T)(serializer.ReadObject(stream) ?? throw new InvalidOperationException($"Deserialize returned null."));
    }

    public void Save(object data, Stream target)
    {
        ArgumentNullException.ThrowIfNull(data);
        Log.Default.Info("DataService.Save started.");
        using var archive = new ZipArchive(target, ZipArchiveMode.Create, leaveOpen: true);
        var entry = archive.CreateEntry(itemFileName, CompressionLevel.Optimal);
        // Set always the same write time -> only content changes should result in a different hash.
        entry.LastWriteTime = new DateTimeOffset(2000, 1, 1, 0, 0, 0, TimeSpan.Zero);
        using var stream = entry.Open();
        var serializer = new DataContractSerializer(data.GetType());
        serializer.WriteObject(stream, data);
        Log.Default.Info("DataService.Save completed.");
    }
}
