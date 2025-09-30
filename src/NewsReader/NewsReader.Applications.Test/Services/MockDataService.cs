using Waf.NewsReader.Applications.Services;

namespace Test.NewsReader.Applications.Services;

public class MockDataService : IDataService
{
    public string GetHash(Stream dataStream) => throw new NotImplementedException();

    public Stream GetReadStream() => throw new NotImplementedException();

    public Stream GetWriteStream() => throw new NotImplementedException();

    public Task<T?> Load<T>(Stream? dataStream = null) where T : class => Task.FromResult<T?>(null);

    public void Save(object data, Stream target) => throw new NotImplementedException();
}
