using Waf.NewsReader.Applications.Services;

namespace Test.NewsReader.Applications.Services;

public class MockNetworkInfoService : Model, INetworkInfoService
{
    private bool internetAccess;

    public bool InternetAccess { get => internetAccess; set => SetProperty(ref internetAccess, value); }
}
