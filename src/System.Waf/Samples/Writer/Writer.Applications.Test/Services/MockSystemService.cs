using Waf.Writer.Applications.Services;

namespace Test.Writer.Applications.Services;

public class MockSystemService : ISystemService
{
    public string? DocumentFileName { get; set; }

    public bool FileExists(string? path) => false;
}
