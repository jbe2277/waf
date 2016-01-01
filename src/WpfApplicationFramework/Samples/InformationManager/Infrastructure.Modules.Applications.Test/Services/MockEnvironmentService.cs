using System.ComponentModel.Composition;
using Waf.InformationManager.Infrastructure.Modules.Applications.Services;

namespace Test.InformationManager.Infrastructure.Modules.Applications.Services
{
    [Export(typeof(IEnvironmentService)), Export]
    public class MockEnvironmentService : IEnvironmentService
    {
        public string DataDirectory { get; set; }        
    }
}
