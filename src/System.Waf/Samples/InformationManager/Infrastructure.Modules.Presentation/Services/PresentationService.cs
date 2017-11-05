using System.ComponentModel.Composition;
using System.Globalization;
using System.Waf.Presentation;
using System.Windows;
using System.Windows.Markup;
using Waf.InformationManager.Common.Applications.Services;

namespace Waf.InformationManager.Infrastructure.Modules.Presentation.Services
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
            FrameworkElement.LanguageProperty.OverrideMetadata(typeof(FrameworkElement), new FrameworkPropertyMetadata(
                XmlLanguage.GetLanguage(CultureInfo.CurrentCulture.IetfLanguageTag)));

            var resourceAssembly = typeof(PresentationService).Assembly;
            var mergedDictionaries = Application.Current.Resources.MergedDictionaries;

            foreach (string resourcePath in moduleResources)
            {
                mergedDictionaries.Add(new ResourceDictionary()
                {
                    Source = ResourceHelper.GetPackUri(resourcePath, resourceAssembly)
                });
            }
        }
    }
}
