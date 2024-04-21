using System.Runtime.Serialization;
using System.Waf.Applications;
using System.Waf.Applications.Services;

namespace Waf.Writer.Applications.Properties;

[DataContract]
public sealed class AppSettings : UserSettingsBase
{
    [DataMember] public double Left { get; set; }

    [DataMember] public double Top { get; set; }

    [DataMember] public double Height { get; set; }

    [DataMember] public double Width { get; set; }

    [DataMember] public bool IsMaximized { get; set; }

    [DataMember] public string? UICulture { get; set; }

    [DataMember] public RecentFileList? RecentFileList { get; set; }

    public void ResetToDefault() => SetDefaultValues();

    protected override void SetDefaultValues() 
    {
        (Left, Top, Height, Width) = (0, 0, 0, 0);
        IsMaximized = false;
        (UICulture, RecentFileList) = (null, null);
    }
}
