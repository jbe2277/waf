using System.Reflection;
using System.Runtime.CompilerServices;

[assembly: AssemblyTitle("System.Waf.UnitTesting.Core")]
[assembly: AssemblyDescription("")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyProduct("System.Waf.UnitTesting.Core")]

#if (!STRONG_NAME)
    [assembly: InternalsVisibleTo("System.Waf.Core.Test")]
#endif