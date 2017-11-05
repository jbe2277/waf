using System.IO.Packaging;
using System.Reflection;

namespace System.Waf.Presentation
{
    /// <summary>
    /// Provides helper methods to manage resources in WPF.
    /// </summary>
    public static class ResourceHelper
    {
        /// <summary>
        /// Gets the pack URI from a local resource path.
        /// </summary>
        /// <param name="resourceAssembly">The assembly containing the resource.</param>
        /// <param name="resourcePath">The local resource path (e.g. Subfolder/ResourceFile.xaml).</param>
        /// <returns>The pack uri.</returns>
        public static Uri GetPackUri(Assembly resourceAssembly, string resourcePath)
        {
            return GetPackUri(resourceAssembly.GetName().Name, resourcePath);
        }

        /// <summary>
        /// Gets the pack URI from a local resource path.
        /// </summary>
        /// <param name="assemblyName">The assembly containing the resource.</param>
        /// <param name="resourcePath">The local resource path (e.g. Subfolder/ResourceFile.xaml).</param>
        /// <returns>The pack uri.</returns>
        public static Uri GetPackUri(string assemblyName, string resourcePath)
        {
            return new Uri(PackUriHelper.UriSchemePack + "://application:,,,/" + assemblyName + ";Component/" + resourcePath);
        }
    }
}
