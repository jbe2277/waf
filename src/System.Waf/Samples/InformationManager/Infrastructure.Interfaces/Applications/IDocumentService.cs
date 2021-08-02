using System.IO;

namespace Waf.InformationManager.Infrastructure.Interfaces.Applications
{
    /// <summary>Provides access to the Information Manager document.</summary>
    public interface IDocumentService
    {
        /// <summary>Returns the content stream of the specified document part.</summary>
        /// <param name="documentPartPath">The path to the document part.</param>
        /// <param name="contentType">The MIME content type of the part data stream.</param>
        /// <param name="fileMode">The I/O mode in which to open the content stream.</param>
        /// <returns>The content stream of the document part.</returns>
        Stream GetStream(string documentPartPath, string contentType, FileMode fileMode);
    }
}
