using System.ComponentModel.Composition;
using System.IO;
using System.Waf.Applications;
using Waf.InformationManager.Infrastructure.Modules.Applications.Services;

namespace Waf.InformationManager.Infrastructure.Modules.Presentation.Services;

[Export(typeof(ISystemService))]
internal sealed class SystemService : ISystemService
{
    private readonly Lazy<string> dataDirectory = new(() => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), ApplicationInfo.Company, ApplicationInfo.ProductName));

    public string DataDirectory => dataDirectory.Value;
}
