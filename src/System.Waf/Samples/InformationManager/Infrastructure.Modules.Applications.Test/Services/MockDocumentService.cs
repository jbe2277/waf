using Waf.InformationManager.Infrastructure.Interfaces.Applications;

namespace Test.InformationManager.Infrastructure.Modules.Applications.Services;

// TODO: Review this design
public class MockDocumentService : IDocumentService
{
    public Func<string, string, FileMode, Stream>? GetStreamAction { get; set; }

    public Stream GetStream(string documentPartPath, string contentType, FileMode fileMode)
    {
        return GetStreamAction == null ? new MemoryStream() : GetStreamAction(documentPartPath, contentType, fileMode);
    }
}
