using System.Diagnostics;

namespace Waf.NewsReader.iOS
{
    public static class Log
    {
        public static TraceSource Default { get; } = new TraceSource("iOS");
    }
}