using System.ComponentModel.Composition;
using System.Globalization;
using System.Waf.Presentation;
using System.Windows;
using System.Windows.Markup;
using Waf.InformationManager.Common.Applications.Services;

namespace Waf.InformationManager.Infrastructure.Modules.Presentation.Services;

[Export(typeof(IPresentationService))]
internal sealed class PresentationService : IPresentationService
{
    public void Initialize()
    {
        FrameworkElement.LanguageProperty.OverrideMetadata(typeof(FrameworkElement), new FrameworkPropertyMetadata(XmlLanguage.GetLanguage(CultureInfo.CurrentCulture.IetfLanguageTag)));

        ResourceHelper.AddToApplicationResources(typeof(PresentationService).Assembly, "Resources/ConverterResources.xaml");
    }
}
