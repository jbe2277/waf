using System.ComponentModel.Composition;
using System.IO;
using Waf.Writer.Applications.Services;

namespace Waf.Writer.Presentation.Services;

[Export(typeof(ISystemService))]
internal sealed class SystemService : ISystemService
{
    private readonly Lazy<string?> documentFileName = new(() => Environment.GetCommandLineArgs().Skip(1).FirstOrDefault(x => !x.StartsWith('-')));

    public string? DocumentFileName => documentFileName.Value;

    public bool FileExists(string? path) => File.Exists(path);
}
