using System.Globalization;
using System.Windows;
using System.Windows.Markup;
using LocalizationSample.Domain;
using LocalizationSample.Presentation;
using LocalizationSample.Properties;

namespace LocalizationSample;

public partial class App
{
    public App()
    {
        InitializeCultures();
    }


    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        var person = new Person()
        {
            Name = "Luke",
            Birthday = new DateTime(2080, 2, 6)
        };

        var mainWindow = new ShellWindow() { DataContext = person };
        mainWindow.Show();
    }

    private static void InitializeCultures()
    {
        if (!string.IsNullOrEmpty(Settings.Default.Culture))
        {
            CultureInfo.CurrentCulture = CultureInfo.DefaultThreadCurrentCulture = new CultureInfo(Settings.Default.Culture);
        }
        if (!string.IsNullOrEmpty(Settings.Default.UICulture))
        {
            CultureInfo.CurrentUICulture = CultureInfo.DefaultThreadCurrentUICulture = new CultureInfo(Settings.Default.UICulture);
        }

        // Set XmlLanguage or use {waf:Bind}. The latter one supports also custom culture settings
        //FrameworkElement.LanguageProperty.OverrideMetadata(typeof(FrameworkElement), new FrameworkPropertyMetadata(
        //    XmlLanguage.GetLanguage(CultureInfo.CurrentCulture.IetfLanguageTag)));
    }
}
