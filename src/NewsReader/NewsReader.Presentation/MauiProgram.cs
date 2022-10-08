using Autofac;
using Autofac.Extensions.DependencyInjection;
using Waf.NewsReader.Applications;

namespace Waf.NewsReader.Presentation;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
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
#if ANDROID
                x.RegisterModule(new Platforms.Android.AndroidModule());
#elif IOS
                x.RegisterModule(new Platforms.iOS.IosModule());
#endif
            }));
        return builder.Build();
    }
}
