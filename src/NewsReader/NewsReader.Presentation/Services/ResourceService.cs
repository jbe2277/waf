using Jbe.NewsReader.Applications.Services;
using System.Composition;
using System.Globalization;
using Windows.ApplicationModel.Resources;

namespace Jbe.NewsReader.Presentation.Services
{
    [Export(typeof(IResourceService)), Export, Shared]
    internal class ResourceService : IResourceService
    {
        string IResourceService.GetString(string resource, params object[] args) => GetString(resource, null);
        

        internal static string GetString(string resource, params object[] args)
        {
            var resourceText = ResourceLoader.GetForViewIndependentUse().GetString(resource);
            if (args == null || args.Length == 0)
            {
                return resourceText;
            }
            return string.Format(CultureInfo.CurrentCulture, resourceText, args);
        }
    }
}
