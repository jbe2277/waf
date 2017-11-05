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
        /// <param name="resourcePath">The local resource path (e.g. Subfolder/ResourceFile.xaml).</param>
        /// <param name="resourceAssembly">The assembly containing the resource.</param>
        /// <returns>The pack uri.</returns>
        public static Uri GetPackUri(string resourcePath, Assembly resourceAssembly)
        {
            return GetPackUri(resourcePath, resourceAssembly.GetName().Name);
        }

        /// <summary>
        /// Gets the pack URI from a local resource path.
        /// </summary>
        /// <param name="resourcePath">The local resource path (e.g. Subfolder/ResourceFile.xaml).</param>
        /// <param name="assemblyName">The assembly containing the resource.</param>
        /// <returns>The pack uri.</returns>
        public static Uri GetPackUri(string resourcePath, string assemblyName)
        {
            return new Uri(PackUriHelper.UriSchemePack + "://application:,,,/" + assemblyName + ";Component/" + resourcePath);
        }
    }
}
