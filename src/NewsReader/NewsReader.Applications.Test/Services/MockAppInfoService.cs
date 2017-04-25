using Jbe.NewsReader.Applications.Services;
using System.Composition;

namespace Test.NewsReader.Applications.Services
{
    [Export(typeof(IAppInfoService)), Export, Shared]
    public class MockAppInfoService : IAppInfoService
    {
        public string AppDescription { get; set; }
        
        public string AppName { get; set; }
        
        public string AppPublisherName { get; set; }
        
        public string AppVersion { get; set; }
    }
}
