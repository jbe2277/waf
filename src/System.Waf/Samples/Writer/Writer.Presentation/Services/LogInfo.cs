using NLog;
using NLog.Targets;
using NLog.Targets.Wrappers;
using System.IO;

namespace Waf.Writer.Presentation.Services;

internal static class LogInfo
{
    public static string GetLogFileName()
    {
        var config = LogManager.Configuration ?? throw new InvalidOperationException("LogManager.Configuration is null");
        var fileTarget = (FileTarget?)((AsyncTargetWrapper?)config.FindTargetByName("fileTarget"))?.WrappedTarget ?? throw new InvalidOperationException("NLog fileTarget is null");
        var archiveSuffix = fileTarget.ArchiveSuffixFormat[0];

        var baseFileName = fileTarget.FileName.Render(new LogEventInfo());
        var directory = Path.GetDirectoryName(baseFileName);
        if (string.IsNullOrEmpty(directory) || !Directory.Exists(directory)) return baseFileName;

        var baseName = Path.GetFileNameWithoutExtension(baseFileName);
        var extension = Path.GetExtension(baseFileName);

        return Directory.EnumerateFiles(directory, $"{baseName}*{extension}").MaxBy(f =>
        {
            var suffix = Path.GetFileNameWithoutExtension(f)[baseName.Length..];
            return int.TryParse(suffix.TrimStart(archiveSuffix), out var n) ? n : -1;
        }) ?? baseFileName;
    }
}
