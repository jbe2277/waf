

#if IOS
[assembly: SuppressMessage("Naming", "CA1711:Identifiers should not have incorrect suffix", Justification = "<Pending>", Scope = "type", Target = "~T:Waf.NewsReader.MauiSystem.Platforms.iOS.AppDelegate")]

#elif ANDROID
[assembly: SuppressMessage("Performance", "CA1812:Avoid uninstantiated internal classes", Justification = "<Pending>", Scope = "type", Target = "~T:Waf.NewsReader.MauiSystem.Platforms.Android.Services.AndroidSystemTraceListener")]

#elif WINDOWS
[assembly: SuppressMessage("Security", "CA5392:Use DefaultDllImportSearchPaths attribute for P/Invokes", Justification = "<Pending>", Scope = "member", Target = "~M:Waf.NewsReader.MauiSystem.Platform.Windows.Program.XamlCheckProcessRequirements")]
[assembly: SuppressMessage("Performance", "CA1812:Avoid uninstantiated internal classes", Justification = "<Pending>", Scope = "type", Target = "~T:Waf.NewsReader.MauiSystem.Platforms.Windows.Services.WindowsSystemTraceListener")]

#endif