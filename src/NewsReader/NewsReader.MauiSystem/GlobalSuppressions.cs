
#if IOS
[assembly: SuppressMessage("Naming", "CA1711:Identifiers should not have incorrect suffix", Justification = "<Pending>", Scope = "type", Target = "~T:Waf.NewsReader.MauiSystem.Platforms.iOS.AppDelegate")]
[assembly: SuppressMessage("Performance", "CA1812:AvoidUninstantiatedInternalClasses", Scope = "type", Target = "~T:Waf.NewsReader.MauiSystem.Platforms.iOS.AppDelegate")]
[assembly: SuppressMessage("Performance", "CA1812:AvoidUninstantiatedInternalClasses", Scope = "type", Target = "~T:Waf.NewsReader.MauiSystem.Platforms.iOS.Services.LocalizationService")]

#elif ANDROID

#elif WINDOWS
[assembly: SuppressMessage("Maintainability", "CA1515:Consider making public types internal", Justification = "<Pending>", Scope = "type", Target = "~T:Waf.NewsReader.MauiSystem.Platform.Windows.App")]

#endif
