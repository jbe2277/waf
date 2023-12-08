using Waf.NewsReader.Applications.Services;

namespace Test.NewsReader.Applications.Services;

internal class MockAppInfoService : IAppInfoService
{
    public string AppName => "TestApp";

    public string VersionString => "1.2.3";
}
