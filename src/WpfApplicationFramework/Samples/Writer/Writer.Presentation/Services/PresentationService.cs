using System.ComponentModel.Composition;
using System.Globalization;
using System.Windows;
using System.Windows.Markup;
using Waf.Writer.Applications.Services;

namespace Waf.Writer.Presentation.Services
{
    [Export(typeof(IPresentationService))]
    public class PresentationService : IPresentationService
    {
        public double VirtualScreenWidth { get { return SystemParameters.VirtualScreenWidth; } }

        public double VirtualScreenHeight { get { return SystemParameters.VirtualScreenHeight; } }

        
        public void InitializeCultures()
        {
            FrameworkElement.LanguageProperty.OverrideMetadata(typeof(FrameworkElement), new FrameworkPropertyMetadata(
                XmlLanguage.GetLanguage(CultureInfo.CurrentCulture.IetfLanguageTag)));
        }
    }
}
