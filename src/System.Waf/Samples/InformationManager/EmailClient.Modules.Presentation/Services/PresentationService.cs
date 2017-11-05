using System.ComponentModel.Composition;
using System.Waf.Presentation;
using System.Windows;
using Waf.InformationManager.Common.Applications.Services;

namespace Waf.InformationManager.EmailClient.Modules.Presentation.Services
{
    [Export(typeof(IPresentationService))]
    internal class PresentationService : IPresentationService
    {
        private static readonly string[] moduleResources = new string[]
        {
            "Resources/ConverterResources.xaml",
        };
        
        public void Initialize()
        {
            var resourceAssembly = typeof(PresentationService).Assembly;
            var mergedDictionaries = Application.Current.Resources.MergedDictionaries;

            foreach (string resourcePath in moduleResources)
            {
                mergedDictionaries.Add(new ResourceDictionary()
                {
                    Source = ResourceHelper.GetPackUri(resourceAssembly, resourcePath)
                });
            }
        }
    }
}
