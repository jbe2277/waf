using System.IO.Packaging;
using System.Reflection;
using System.Windows;

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
            if (resourceAssembly == null) throw new ArgumentNullException(nameof(resourceAssembly));
            return GetPackUri(resourceAssembly.GetName().Name, resourcePath);
        }

        /// <summary>
        /// Gets the pack URI from a local resource path.
        /// </summary>
        /// <param name="resourceAssemblyName">The assembly containing the resource.</param>
        /// <param name="resourcePath">The local resource path (e.g. Subfolder/ResourceFile.xaml).</param>
        /// <returns>The pack uri.</returns>
        public static Uri GetPackUri(string resourceAssemblyName, string resourcePath)
        {
            if (string.IsNullOrEmpty(resourceAssemblyName)) throw new ArgumentException("Null or empty is not allowed.", nameof(resourceAssemblyName));
            if (string.IsNullOrEmpty(resourcePath)) throw new ArgumentException("Null or empty is not allowed.", nameof(resourcePath));
            return new Uri(PackUriHelper.UriSchemePack + "://application:,,,/" + resourceAssemblyName + ";Component/" + resourcePath);
        }

        /// <summary>
        /// Adds a ResourceDictionary to the Application MergedDictionary collection.
        /// </summary>
        /// <param name="resourceAssembly">The assembly containing the resource dictionary.</param>
        /// <param name="resourceDictionaryPaths">The local resource dictionary path (e.g. Subfolder/ResourceFile.xaml).</param>
        public static void AddToApplicationResources(Assembly resourceAssembly, params string[] resourceDictionaryPaths)
        {
            if (resourceAssembly == null) throw new ArgumentNullException(nameof(resourceAssembly));
            AddToApplicationResources(resourceAssembly.GetName().Name, resourceDictionaryPaths);
        }

        /// <summary>
        /// Adds a ResourceDictionary to the Application MergedDictionary collection.
        /// </summary>
        /// <param name="resourceAssemblyName">The assembly containing the resource dictionary.</param>
        /// <param name="resourceDictionaryPaths">The local resource dictionary path (e.g. Subfolder/ResourceFile.xaml).</param>
        public static void AddToApplicationResources(string resourceAssemblyName, params string[] resourceDictionaryPaths)
        {
            if (string.IsNullOrEmpty(resourceAssemblyName)) throw new ArgumentException("Null or empty is not allowed.", nameof(resourceAssemblyName));
            if (resourceDictionaryPaths == null) throw new ArgumentNullException(nameof(resourceDictionaryPaths));

            foreach (string resourcePath in resourceDictionaryPaths)
            {
                Application.Current.Resources.MergedDictionaries.Add(new ResourceDictionary
                {
                    Source = GetPackUri(resourceAssemblyName, resourcePath)
                });
            }
        }
    }
}
