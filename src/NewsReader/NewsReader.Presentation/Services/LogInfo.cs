namespace Waf.NewsReader.Presentation.Services;

internal static class LogInfo
{
    public static string LogBaseFileName { get; } = Path.Combine(FileSystem.CacheDirectory, "Logging", "AppLog.txt");

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
