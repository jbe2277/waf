namespace Jbe.NewsReader.Applications.Services
{
    public interface IAppInfoService
    {
        string AppName { get; }

        string AppVersion { get; }
        
        string AppDescription { get; }

        string AppPublisherName { get; }
    }
}
