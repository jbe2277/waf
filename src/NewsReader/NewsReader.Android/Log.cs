using System.Diagnostics;

namespace Waf.NewsReader.Android
{
    public static class Log
    {
        public static TraceSource Default { get; } = new TraceSource("Android");
    }
}