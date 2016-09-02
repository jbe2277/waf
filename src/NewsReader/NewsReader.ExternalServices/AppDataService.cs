using Jbe.NewsReader.Applications.Services;
using System.Collections.Generic;
using System.Composition;
using Windows.Storage;

namespace Jbe.NewsReader.ExternalServices
{
    [Export(typeof(IAppDataService)), Export, Shared]
    public class AppDataService : IAppDataService
    {
        public IDictionary<string, object> LocalSettings => ApplicationData.Current.LocalSettings.Values;
    }
}
