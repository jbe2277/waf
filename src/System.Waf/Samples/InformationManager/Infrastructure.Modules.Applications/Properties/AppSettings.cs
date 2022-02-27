using System.Runtime.Serialization;
using System.Waf.Applications.Services;

namespace Waf.InformationManager.Infrastructure.Modules.Applications.Properties;

[DataContract]
public sealed class AppSettings : UserSettingsBase
{
    [DataMember] public double Left { get; set; }

    [DataMember] public double Top { get; set; }

    [DataMember] public double Height { get; set; }

    [DataMember] public double Width { get; set; }

    [DataMember] public bool IsMaximized { get; set; }

    protected override void SetDefaultValues() { }
}
