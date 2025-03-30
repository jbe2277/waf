using System.Waf.Applications;
using Waf.InformationManager.Infrastructure.Modules.Applications.Services;

namespace Test.InformationManager.Infrastructure.Modules.Applications.Services;

public class MockSystemService : ISystemService
{
    public string DataDirectory { get; set; } = null!;
}
