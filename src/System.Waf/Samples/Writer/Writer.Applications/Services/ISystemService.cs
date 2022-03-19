namespace Waf.Writer.Applications.Services;

public interface ISystemService
{
    string? DocumentFileName { get; }

    bool FileExists(string? path);
}
