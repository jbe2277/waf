using Autofac;
using Autofac.Extensions.DependencyInjection;
using Waf.NewsReader.Applications;
using Waf.NewsReader.Presentation;

namespace Waf.NewsReader.MauiSystem;

internal static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .UseWindowsAutomationTreeFix()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                fonts.AddFont("materialdesignicons-webfont.ttf", "MaterialIcons");
            })
            .ConfigureContainer(new AutofacServiceProviderFactory(x =>
            {
                x.RegisterModule(new ApplicationsModule());
                x.RegisterModule(new PresentationModule());
                x.RegisterModule(new MauiModule());
#if ANDROID
                x.RegisterModule(new Platforms.Android.AndroidModule());
#elif IOS
                x.RegisterModule(new Platforms.iOS.IosModule());
#elif WINDOWS
                x.RegisterModule(new Platforms.Windows.WindowsModule());
#endif
            }));
        return builder.Build();
    }
}


internal static class AppBuilderExtensions
{
    public static MauiAppBuilder UseWindowsAutomationTreeFix(this MauiAppBuilder builder)
    {
#if WINDOWS
        Microsoft.Maui.Handlers.ViewHandler.ViewMapper.AppendToMapping(nameof(IView.AutomationId), MapAutomationId);
#endif
        return builder;
    }


#if WINDOWS
    private static void MapAutomationId(IViewHandler handler, IView view)
    {
        // Workaround for https://github.com/dotnet/maui/issues/4715; Layouts, Pages and ContentViews are not exposed in AutomationTree
        if (string.IsNullOrEmpty(view.AutomationId) || view is not Microsoft.Maui.ILayout and not Page and not ContentView) return;
        var platformView = (Microsoft.UI.Xaml.FrameworkElement?)handler.PlatformView;
        if (platformView is null) return;
        Microsoft.UI.Xaml.Automation.AutomationProperties.SetName(platformView, view.AutomationId);
    }
#endif
}
