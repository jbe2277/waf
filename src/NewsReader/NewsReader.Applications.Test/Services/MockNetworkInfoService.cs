using Waf.NewsReader.Applications.Services;

namespace Test.NewsReader.Applications.Services;

public class MockNetworkInfoService : Model, INetworkInfoService
{
    public bool InternetAccess { get; set => SetProperty(ref field, value); }
}
