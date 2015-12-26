using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;


[assembly: AssemblyTitle("WPF Application Framework (WAF) - UnitTesting Extension")]
[assembly: AssemblyDescription("")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyProduct("WPF Application Framework (WAF)")]


[assembly: Guid("497d4744-3d62-432d-81d9-7d77c0590f05")]

#if (!STRONG_NAME)
    [assembly: InternalsVisibleTo("WpfApplicationFramework.Test")]
#endif
