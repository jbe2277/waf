using Jbe.NewsReader.Applications.Services;
using System.Composition;
using System.Waf.Foundation;

namespace Test.NewsReader.Applications.Services
{
    [Export, Export(typeof(INetworkInfoService)), Shared]
    public class MockNetworkInfoService : Model, INetworkInfoService
    {
        private bool internetAccess = true;

        public bool InternetAccess
        {
            get => internetAccess;
            set => SetProperty(ref internetAccess, value);
        }
    }
}
