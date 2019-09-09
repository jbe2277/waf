using System.Diagnostics;

namespace Waf.NewsReader.Applications
{
    public static class Log
    {
        public static TraceSource Default { get; } = new TraceSource("App");
    }
}