using System.ComponentModel.Composition;
using System.Waf.Applications.Services;

namespace System.Waf.UnitTesting.Mocks
{
    /// <summary>Mock for the ISettingsService interface.</summary>
    [Export(typeof(ISettingsService)), Export(typeof(MockSettingsService)), Export]
    public class MefMockSettingsService : MockSettingsService
    {
    }
}
