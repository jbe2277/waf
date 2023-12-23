using System.Waf.Applications;

namespace Waf.InformationManager.Common.Applications.Services;

/// <summary>Provides information about the App.</summary>
public static class AppInfo
{
    private static string AppDataPath { get; } = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), ApplicationInfo.ProductName);

    /// <summary>File name of the log file.</summary>
    public static string LogFileName { get; } = Path.Combine(AppDataPath, "Log", "InfoMan.log");
}
