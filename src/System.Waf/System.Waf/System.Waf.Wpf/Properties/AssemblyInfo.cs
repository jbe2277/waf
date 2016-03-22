using System;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;
using System.Windows.Markup;


[assembly: AssemblyTitle("System.Waf.Wpf")]
[assembly: AssemblyDescription("System.Waf is a lightweight framework that helps you to create well-structured XAML applications. "
    + "This package contains the WPF (Windows Presentation Foundation) specific types.")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyProduct("System.Waf.Wpf")]


[assembly: Guid("331cc379-bc8e-4129-8b6a-5698bf93b774")]
[assembly: CLSCompliant(true)]


[assembly: XmlnsDefinition("http://waf.codeplex.com/schemas", "System.Waf.Presentation")]
[assembly: XmlnsDefinition("http://waf.codeplex.com/schemas", "System.Waf.Presentation.Converters")]


#if (!STRONG_NAME)
    [assembly: InternalsVisibleTo("System.Waf.Wpf.Test")]
#endif