using Jbe.NewsReader.Applications.Services;
using System.Composition;
using Windows.ApplicationModel.Resources;

namespace Jbe.NewsReader.Presentation.Services
{
    [Export(typeof(IResourceService)), Export, Shared]
    internal class ResourceService : IResourceService
    {
        public string GetString(string resource)
        {
            return ResourceLoader.GetForViewIndependentUse().GetString(resource);
        }
    }
}
