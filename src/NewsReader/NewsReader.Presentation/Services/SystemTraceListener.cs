using System.Diagnostics;

namespace Waf.NewsReader.Presentation.Services;

public abstract class SystemTraceListener
{
    public abstract TraceListener Create();
}
