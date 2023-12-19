using Waf.NewsReader.Applications.Services;

namespace Test.NewsReader.Applications.Services;

public class MockDataService : IDataService
{
    public string GetHash() => throw new NotImplementedException();

    public Stream GetReadStream() => throw new NotImplementedException();

    public T? Load<T>(Stream? dataStream = null) where T : class => null;

    public void Save(object data) => throw new NotImplementedException();
}
