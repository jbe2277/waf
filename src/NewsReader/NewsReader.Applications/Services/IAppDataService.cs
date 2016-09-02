using System.Collections.Generic;

namespace Jbe.NewsReader.Applications.Services
{
    public interface IAppDataService
    {
        IDictionary<string, object> LocalSettings { get; }
    }
}
