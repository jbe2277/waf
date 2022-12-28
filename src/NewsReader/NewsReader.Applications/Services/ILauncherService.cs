namespace Waf.NewsReader.Applications.Services;

public interface ILauncherService
{
    Task LaunchBrowser(Uri uri);
}
