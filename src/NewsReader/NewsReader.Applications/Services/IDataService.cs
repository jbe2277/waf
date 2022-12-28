namespace Waf.NewsReader.Applications.Services;

public interface IDataService
{
    Stream GetReadStream();

    string GetHash();

    T? Load<T>(Stream? dataStream = null) where T : class;

    void Save(object data);
}
