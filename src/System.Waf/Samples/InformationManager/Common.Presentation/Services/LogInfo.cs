using System.IO;
using System.Waf.Applications;

namespace Waf.InformationManager.Common.Presentation.Services;

/// <summary>Provides information about the log environment.</summary>
public static class LogInfo
{
    private static string AppDataPath { get; } = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), ApplicationInfo.ProductName);

    /// <summary>Base file name of the log file.</summary>
    public static string LogBaseFileName { get; } = Path.Combine(AppDataPath, "Log", "InfoMan.log");

    /// <summary>Get the log file name of the actual log file.</summary>
    public static string GetLogFileName()
    {
        const char archiveSuffix = '_';
        var directory = Path.GetDirectoryName(LogBaseFileName);
        if (string.IsNullOrEmpty(directory) || !Directory.Exists(directory)) return LogBaseFileName;

        var baseName = Path.GetFileNameWithoutExtension(LogBaseFileName);
        var extension = Path.GetExtension(LogBaseFileName);
        return Directory.EnumerateFiles(directory, $"{baseName}*{extension}").MaxBy(f =>
        {
            var suffix = Path.GetFileNameWithoutExtension(f)[baseName.Length..];
            return int.TryParse(suffix.TrimStart(archiveSuffix), out var n) ? n : -1;
        }) ?? LogBaseFileName;
    }
}
