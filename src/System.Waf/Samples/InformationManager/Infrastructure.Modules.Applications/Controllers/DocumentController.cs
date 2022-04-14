using System.ComponentModel.Composition;
using System.IO.Packaging;
using Waf.InformationManager.Infrastructure.Interfaces.Applications;
using Waf.InformationManager.Infrastructure.Modules.Applications.Services;

namespace Waf.InformationManager.Infrastructure.Modules.Applications.Controllers;

[Export(typeof(IDocumentService)), Export]
internal class DocumentController : IDocumentService
{
    private const string fileName = "InformationManager.datx";

    private readonly ISystemService systemService;
    private Package? package;

    [ImportingConstructor]
    public DocumentController(ISystemService systemService)
    {
        this.systemService = systemService;
        PackagePath = Path.Combine(systemService.DataDirectory, fileName);
    }

    internal string PackagePath { get; }

    public void Initialize()
    {
        var dataDirectory = systemService.DataDirectory;
        if (!Directory.Exists(dataDirectory)) Directory.CreateDirectory(dataDirectory);
        package = Package.Open(PackagePath, FileMode.OpenOrCreate);
    }

    public void Shutdown() => package?.Close();

    public Stream GetStream(string documentPartPath, string contentType, FileMode fileMode)
    {
        var documentUri = PackUriHelper.CreatePartUri(new Uri(documentPartPath, UriKind.Relative));
        var packagePart = package!.PartExists(documentUri)
            ? package.GetPart(documentUri)
            : package.CreatePart(documentUri, contentType, CompressionOption.Normal);
        return packagePart.GetStream(fileMode);
    }
}
