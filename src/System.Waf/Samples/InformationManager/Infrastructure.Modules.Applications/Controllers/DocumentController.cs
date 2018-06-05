using System;
using System.ComponentModel.Composition;
using System.IO;
using System.IO.Packaging;
using Waf.InformationManager.Infrastructure.Interfaces.Applications;
using Waf.InformationManager.Infrastructure.Modules.Applications.Services;

namespace Waf.InformationManager.Infrastructure.Modules.Applications.Controllers
{    
    [Export(typeof(IDocumentService)), Export]
    internal class DocumentController : IDocumentService
    {
        private const string fileName = "InformationManager.datx";

        private readonly IEnvironmentService environmentService;
        private Package package;

        [ImportingConstructor]
        public DocumentController(IEnvironmentService environmentService)
        {
            this.environmentService = environmentService;
        }

        internal string PackagePath { get; private set; }

        public void Initialize()
        {
            var dataDirectory = environmentService.DataDirectory;
            if (!Directory.Exists(dataDirectory))
            {
                Directory.CreateDirectory(dataDirectory);
            }

            PackagePath = Path.Combine(dataDirectory, fileName);
            package = Package.Open(PackagePath, FileMode.OpenOrCreate);
        }

        public void Shutdown()
        {
            package.Close();
        }

        public Stream GetStream(string documentPartPath, string contentType, FileMode fileMode)
        {
            Uri documentUri = PackUriHelper.CreatePartUri(new Uri(documentPartPath, UriKind.Relative));
            PackagePart packagePart;
            if (!package.PartExists(documentUri))
            {
                packagePart = package.CreatePart(documentUri, contentType, CompressionOption.Normal);
            }
            else
            {
                packagePart = package.GetPart(documentUri);
            }
            return packagePart.GetStream(fileMode);
        }
    }
}
