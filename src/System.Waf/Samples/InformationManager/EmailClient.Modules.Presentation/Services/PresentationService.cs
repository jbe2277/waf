using System.ComponentModel.Composition;
using System.Waf.Presentation;
using Waf.InformationManager.Common.Applications.Services;

namespace Waf.InformationManager.EmailClient.Modules.Presentation.Services
{
    [Export(typeof(IPresentationService))]
    internal class PresentationService : IPresentationService
    {
        public void Initialize()
        {
            ResourceHelper.AddToApplicationResources(typeof(PresentationService).Assembly, "Resources/ConverterResources.xaml");
        }
    }
}
