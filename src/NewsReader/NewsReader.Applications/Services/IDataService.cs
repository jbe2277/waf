namespace Waf.NewsReader.Applications.Services;

public interface IDataService
{
    Stream GetReadStream();

    Stream GetWriteStream();

    string GetHash(Stream dataStream);

    Task<T?> Load<T>(Stream dataStream) where T : class;

    void Save(object data, Stream target);
}
