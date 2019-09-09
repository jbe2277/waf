using System.Diagnostics;

namespace Waf.NewsReader.UWP
{
    public static class Log
    {
        public static TraceSource Default { get; } = new TraceSource("Uwp");
    }
}
