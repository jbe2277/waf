using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;


[assembly: AssemblyTitle("System.Waf.UnitTesting.Wpf")]
[assembly: AssemblyDescription("System.Waf is a lightweight framework that helps you to create well-structured XAML applications. "
    + "This package supports writing unit tests. It contains the WPF (Windows Presentation Foundation) specific types.")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyProduct("System.Waf.UnitTesting.Wpf")]


[assembly: Guid("497d4744-3d62-432d-81d9-7d77c0590f05")]
[assembly: CLSCompliant(true)]

#if (!STRONG_NAME)
[assembly: InternalsVisibleTo("WpfApplicationFramework.Test")]
#endif
