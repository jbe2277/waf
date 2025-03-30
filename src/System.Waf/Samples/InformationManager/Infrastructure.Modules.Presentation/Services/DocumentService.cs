using System.IO.Packaging;
using System.IO;
using Waf.InformationManager.Infrastructure.Interfaces.Applications;
using Waf.InformationManager.Infrastructure.Modules.Applications.Services;

namespace Waf.InformationManager.Infrastructure.Modules.Presentation.Services;

internal sealed class DocumentService : IDocumentService, IDisposable
{
    private const string fileName = "InformationManager.datx";

    private readonly Lazy<Package> package;

    public DocumentService(ISystemService systemService)
    {
        PackagePath = Path.Combine(systemService.DataDirectory, fileName);
        package = new(() =>
        {
            var dataDirectory = systemService.DataDirectory;
            if (!Directory.Exists(dataDirectory)) Directory.CreateDirectory(dataDirectory);
            return Package.Open(PackagePath, FileMode.OpenOrCreate);
        });
    }

    internal string PackagePath { get; }

    public Stream GetStream(string documentPartPath, string contentType, FileMode fileMode)
    {
        var documentUri = PackUriHelper.CreatePartUri(new Uri(documentPartPath, UriKind.Relative));
        var packagePart = package.Value.PartExists(documentUri)
            ? package.Value.GetPart(documentUri)
            : package.Value.CreatePart(documentUri, contentType, CompressionOption.Normal);
        return packagePart.GetStream(fileMode);
    }

    public void Dispose() 
    { 
        if (package.IsValueCreated) package.Value.Close(); 
    }
}
