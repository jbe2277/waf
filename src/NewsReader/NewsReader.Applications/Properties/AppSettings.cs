using System.Runtime.Serialization;
using System.Waf.Applications.Services;

namespace Waf.NewsReader.Applications.Properties;

[DataContract]
public sealed class AppSettings : UserSettingsBase
{
    [DataMember] public string? Language { get; set; }

    [DataMember] public string? WebStorageCTag { get; set; }

    [DataMember] public string? LastUploadedFileHash { get; set; }

    protected override void SetDefaultValues() { }
}
