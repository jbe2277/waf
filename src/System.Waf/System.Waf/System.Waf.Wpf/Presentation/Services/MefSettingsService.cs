using System.ComponentModel.Composition;
using System.Waf.Applications.Services;

namespace System.Waf.Presentation.Services
{
    /// <summary>Service that is responsible to load and save user settings.</summary>
    [Export(typeof(ISettingsService)), Export(typeof(SettingsService)), Export]
    public sealed class MefSettingsService : SettingsService
    {
    }
}
