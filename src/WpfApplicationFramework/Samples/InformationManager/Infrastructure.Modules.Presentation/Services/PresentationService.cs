using System.ComponentModel.Composition;
using System.Globalization;
using System.Windows;
using System.Windows.Markup;
using Waf.InformationManager.Common.Applications.Services;
using Waf.InformationManager.Common.Presentation;

namespace Waf.InformationManager.Infrastructure.Modules.Presentation.Services
{
    [Export(typeof(IPresentationService))]
    internal class PresentationService : IPresentationService
    {
        // I would prefer to manage the list of ResourceDictionaries required by a module in an
        // own resource dictionary "ModuleResources.xaml". But this won't work because of this WPF bug:
        // http://connect.microsoft.com/VisualStudio/feedback/details/781727/wpf-resourcedictionary-mergeddictionaries-resolve-issue
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
