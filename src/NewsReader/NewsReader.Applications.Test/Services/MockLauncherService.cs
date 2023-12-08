using Waf.NewsReader.Applications.Services;

namespace Test.NewsReader.Applications.Services;
public class MockLauncherService : ILauncherService
{
    public Task LaunchBrowser(Uri uri) => throw new NotImplementedException();
}
