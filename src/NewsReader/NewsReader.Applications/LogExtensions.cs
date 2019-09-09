using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;

namespace Waf.NewsReader.Applications
{
    public static class LogExtensions
    {
        public static bool IsTraceEnabled(this TraceSource traceSource)
        {
            return traceSource.Switch.ShouldTrace(TraceEventType.Verbose);
        }

        public static bool IsInfoEnabled(this TraceSource traceSource)
        {
            return traceSource.Switch.ShouldTrace(TraceEventType.Information);
        }

        public static bool IsWarnEnabled(this TraceSource traceSource)
        {
            return traceSource.Switch.ShouldTrace(TraceEventType.Warning);
        }

        public static bool IsErrorEnabled(this TraceSource traceSource)
        {
            return traceSource.Switch.ShouldTrace(TraceEventType.Error);
        }

        public static void Trace(this TraceSource traceSource, [Localizable(false)] string message)
        {
            if (IsTraceEnabled(traceSource)) traceSource.TraceEvent(TraceEventType.Verbose, 0, message);
        }

        public static void Trace(this TraceSource traceSource, [Localizable(false)] string format, params object[] arguments)
        {
            if (IsTraceEnabled(traceSource)) traceSource.TraceEvent(TraceEventType.Verbose, 0, format, arguments);
        }

        public static void Info(this TraceSource traceSource, [Localizable(false)] string message)
        {
            if (IsInfoEnabled(traceSource)) traceSource.TraceEvent(TraceEventType.Information, 0, message);
        }

        public static void Info(this TraceSource traceSource, [Localizable(false)] string format, params object[] arguments)
        {
            if (IsInfoEnabled(traceSource)) traceSource.TraceEvent(TraceEventType.Information, 0, format, arguments);
        }

        public static void Warn(this TraceSource traceSource, [Localizable(false)] string message)
        {
            if (IsWarnEnabled(traceSource)) traceSource.TraceEvent(TraceEventType.Warning, 0, message);
        }

        public static void Warn(this TraceSource traceSource, [Localizable(false)] string format, params object[] arguments)
        {
            if (IsWarnEnabled(traceSource)) traceSource.TraceEvent(TraceEventType.Warning, 0, format, arguments);
        }

        public static void Error(this TraceSource traceSource, Exception exception, [Localizable(false)] string message)
        {
            if (IsErrorEnabled(traceSource)) traceSource.TraceEvent(TraceEventType.Error, 0, message + "  " + exception);
        }

        public static void Error(this TraceSource traceSource, Exception exception, [Localizable(false)] string format, params object[] arguments)
        {
            if (IsErrorEnabled(traceSource))
                traceSource.TraceEvent(TraceEventType.Error, 0, string.Format(CultureInfo.InvariantCulture, format, arguments) + "  " + exception);
        }
    }
}
