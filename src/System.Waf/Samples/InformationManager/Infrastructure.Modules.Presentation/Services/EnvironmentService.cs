using System;
using System.ComponentModel.Composition;
using System.IO;
using System.Waf.Applications;
using Waf.InformationManager.Infrastructure.Modules.Applications.Services;

namespace Waf.InformationManager.Infrastructure.Modules.Presentation.Services
{
    [Export(typeof(IEnvironmentService))]
    internal class EnvironmentService : IEnvironmentService
    {
        private readonly Lazy<string> dataDirectory = new Lazy<string>(() => 
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), ApplicationInfo.Company, ApplicationInfo.ProductName));
        
        public string DataDirectory => dataDirectory.Value;
    }
}
