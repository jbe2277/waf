using System;
using System.ComponentModel.Composition;
using System.IO;
using Waf.InformationManager.Infrastructure.Interfaces.Applications;

namespace Test.InformationManager.Infrastructure.Modules.Applications.Services
{
    [Export(typeof(IDocumentService)), Export]
    public class MockDocumentService : IDocumentService
    {
        public Func<string, string, FileMode, Stream> GetStreamAction { get; set; }

        
        public Stream GetStream(string documentPartPath, string contentType, FileMode fileMode)
        {
            if (GetStreamAction == null)
            {
                return new MemoryStream();
            }
            return GetStreamAction(documentPartPath, contentType, fileMode);
        }
    }
}
