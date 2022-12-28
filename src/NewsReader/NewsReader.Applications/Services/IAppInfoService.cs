namespace Waf.NewsReader.Applications.Services;

public interface IAppInfoService
{
    string AppName { get; }

    string VersionString { get; }
}
