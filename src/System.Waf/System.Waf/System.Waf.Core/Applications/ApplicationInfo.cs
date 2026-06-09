using System.IO;
using System.Reflection;

namespace System.Waf.Applications
{
    /// <summary>This class provides information about the running application.</summary>
    public static class ApplicationInfo
    {
        private static readonly Lazy<string> productName = new(GetProductName);
        private static readonly Lazy<string> version = new(GetVersion);
        private static readonly Lazy<string> company = new(GetCompany);
        private static readonly Lazy<string> copyright = new(GetCopyright);
        private static readonly Lazy<string> applicationPath = new(GetApplicationPath);

        /// <summary>Gets the product name of the application.</summary>
        public static string ProductName => productName.Value;

        /// <summary>Gets the version number of the application.</summary>
        public static string Version => version.Value;

        /// <summary>Gets the company of the application.</summary>
        public static string Company => company.Value;
        
        /// <summary>Gets the copyright information of the application.</summary>
        public static string Copyright => copyright.Value;

        /// <summary>Gets the path for the executable file that started the application, not including the executable name.</summary>
        [Obsolete("This returns an empty string when you publish an app as a single file. Use AppContext.BaseDirectory instead.")]
        public static string ApplicationPath => applicationPath.Value;

        private static string GetProductName()
        {
            var entryAssembly = Assembly.GetEntryAssembly();
            if (entryAssembly != null)
            {
                var attribute = (AssemblyProductAttribute?)Attribute.GetCustomAttribute(entryAssembly, typeof(AssemblyProductAttribute));
                return attribute?.Product ?? "";
            }
            return "";
        }

        private static string GetVersion()
        {
            return Assembly.GetEntryAssembly()?.GetName().Version?.ToString() ?? "";
        }

        private static string GetCompany()
        {
            var entryAssembly = Assembly.GetEntryAssembly();
            if (entryAssembly != null)
            {
                var attribute = (AssemblyCompanyAttribute?)Attribute.GetCustomAttribute(entryAssembly, typeof(AssemblyCompanyAttribute));
                return attribute?.Company ?? "";
            }
            return "";
        }

        private static string GetCopyright()
        {
            var entryAssembly = Assembly.GetEntryAssembly();
            if (entryAssembly != null)
            {
                var attribute = (AssemblyCopyrightAttribute?)Attribute.GetCustomAttribute(entryAssembly, typeof(AssemblyCopyrightAttribute));
                return attribute?.Copyright ?? "";
            }
            return "";
        }

        private static string GetApplicationPath()
        {
            var entryAssembly = Assembly.GetEntryAssembly();
            if (entryAssembly != null)
            {
#pragma warning disable IL3000 // Avoid accessing Assembly file path when publishing as a single file
                return Path.GetDirectoryName(entryAssembly.Location) ?? "";
#pragma warning restore IL3000 // Avoid accessing Assembly file path when publishing as a single file
            }
            return "";
        }
    }
}
