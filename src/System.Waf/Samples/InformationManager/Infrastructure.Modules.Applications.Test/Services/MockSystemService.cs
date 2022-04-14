using System.ComponentModel.Composition;
using Waf.InformationManager.Infrastructure.Modules.Applications.Services;

namespace Test.InformationManager.Infrastructure.Modules.Applications.Services;

[Export(typeof(ISystemService)), Export]
public class MockSystemService : ISystemService
{
    public string DataDirectory { get; set; } = null!;
}
