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

    public string GetHash()
    {
        using var stream = GetReadStream();
        using var sha1 = SHA1.Create();
        return BitConverter.ToString(sha1.ComputeHash(stream)).Replace("-", "", StringComparison.Ordinal);
    }

    public async Task<T?> Load<T>(Stream? dataStream = null) where T : class
    {
        try
        {
            return await Task.Run(() =>   // Use background thread -> otherwise, a Android.OS.NetworkOnMainThreadException occurs for network streams.
            {
                using var archiveStream = dataStream ?? GetReadStream();
                var result = LoadItem<T>(archiveStream, itemFileName);
                Log.Default.Info("DataService.Load completed. From {0}.", dataStream is null ? containerFileName : "stream");
                return result;
            }).ConfigureAwait(false);
        }
        catch (FileNotFoundException)
        {
            return null;
        }
    }

    private static T LoadItem<T>(Stream archiveStream, string fileName) where T : class
    {
        using var archive = new ZipArchive(archiveStream, ZipArchiveMode.Read, leaveOpen: true);
        var entry = archive.GetEntry(fileName) ?? throw new InvalidOperationException($"Could not find {fileName} in archive.");
        using var stream = entry.Open();
        var serializer = new DataContractSerializer(typeof(T));
        return (T)(serializer.ReadObject(stream) ?? throw new InvalidOperationException($"Deserialize returned null."));
    }

    public void Save(object data)
    {
        ArgumentNullException.ThrowIfNull(data);
        using var archiveStream = File.Create(containerFileName);
        using var archive = new ZipArchive(archiveStream, ZipArchiveMode.Create, leaveOpen: true);
        var entry = archive.CreateEntry(itemFileName, CompressionLevel.Optimal);
        // Set always the same write time -> only content changes should result in a different hash.
        entry.LastWriteTime = new DateTimeOffset(2000, 1, 1, 0, 0, 0, TimeSpan.Zero);
        using var stream = entry.Open();
        var serializer = new DataContractSerializer(data.GetType());
        serializer.WriteObject(stream, data);
        Log.Default.Info("DataService.Save completed. To {0}.", containerFileName);
    }
}
