using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Waf.InformationManager.Common.Presentation
{
    /// <summary>
    /// Provides helper methods to manage resources in WPF.
    /// </summary>
    public static class ResourceHelper
    {
        /// <summary>
        /// Gets the pack URI from a local resource path.
        /// </summary>
        /// <param name="resourcePath">The local resource path.</param>
        /// <param name="resourceAssembly">The assembly containing the resource.</param>
        /// <returns>The pack uri.</returns>
        public static Uri GetPackUri(string resourcePath, Assembly resourceAssembly)
        {
            return new Uri("pack://application:,,,/" + resourceAssembly.GetName().Name + ";Component/" + resourcePath);
        }
    }
}
