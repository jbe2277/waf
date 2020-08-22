using System.ComponentModel;
using System.Diagnostics;

namespace System.Waf.Foundation
{
    /// <summary>Extension methods which simplify the usage of a TraceSource.</summary>
    public static class TraceSourceExtensions
    {
        /// <summary>Indicates, if a log message of type Trace (Verbose) will be processed.</summary>
        /// <param name="traceSource">The trace source.</param>
        /// <returns>Indicates, if a log message of type Trace (Verbose) will be processed.</returns>
        public static bool IsTraceEnabled(this TraceSource traceSource)
        {
            return traceSource.Switch.ShouldTrace(TraceEventType.Verbose);
        }

        /// <summary>Indicates, if a log message of type Information will be processed.</summary>
        /// <param name="traceSource">The trace source.</param>
        /// <returns>Indicates, if a log message of type Information will be processed.</returns>
        public static bool IsInfoEnabled(this TraceSource traceSource)
        {
            return traceSource.Switch.ShouldTrace(TraceEventType.Information);
        }

        /// <summary>Indicates, if a log message of type Warning will be processed.</summary>
        /// <param name="traceSource">The trace source.</param>
        /// <returns>Indicates, if a log message of type Warning will be processed.</returns>
        public static bool IsWarnEnabled(this TraceSource traceSource)
        {
            return traceSource.Switch.ShouldTrace(TraceEventType.Warning);
        }

        /// <summary>Indicates, if a log message of type Error will be processed.</summary>
        /// <param name="traceSource">The trace source.</param>
        /// <returns>Indicates, if a log message of type Error will be processed.</returns>
        public static bool IsErrorEnabled(this TraceSource traceSource)
        {
            return traceSource.Switch.ShouldTrace(TraceEventType.Error);
        }

        /// <summary>Log a message of type Trace (Verbose).</summary>
        /// <param name="traceSource">The trace source.</param>
        /// <param name="message">The log message.</param>
        public static void Trace(this TraceSource traceSource, [Localizable(false)] string message)
        {
            if (IsTraceEnabled(traceSource)) traceSource.TraceEvent(TraceEventType.Verbose, 0, message);
        }

        /// <summary>Log a message of type Trace (Verbose).</summary>
        /// <param name="traceSource">The trace source.</param>
        /// <param name="format">A composite format string.</param>
        /// <param name="arguments">An object array that contains zero or more objects to format.</param>
        public static void Trace(this TraceSource traceSource, [Localizable(false)] string format, params object?[] arguments)
        {
            if (IsTraceEnabled(traceSource)) traceSource.TraceEvent(TraceEventType.Verbose, 0, format, arguments);
        }

        /// <summary>Log a message of type Information.</summary>
        /// <param name="traceSource">The trace source.</param>
        /// <param name="message">The log message.</param>
        public static void Info(this TraceSource traceSource, [Localizable(false)] string message)
        {
            if (IsInfoEnabled(traceSource)) traceSource.TraceEvent(TraceEventType.Information, 0, message);
        }

        /// <summary>Log a message of type Information.</summary>
        /// <param name="traceSource">The trace source.</param>
        /// <param name="format">A composite format string.</param>
        /// <param name="arguments">An object array that contains zero or more objects to format.</param>
        public static void Info(this TraceSource traceSource, [Localizable(false)] string format, params object?[] arguments)
        {
            if (IsInfoEnabled(traceSource)) traceSource.TraceEvent(TraceEventType.Information, 0, format, arguments);
        }

        /// <summary>Log a message of type Warning.</summary>
        /// <param name="traceSource">The trace source.</param>
        /// <param name="message">The log message.</param>
        public static void Warn(this TraceSource traceSource, [Localizable(false)] string message)
        {
            if (IsWarnEnabled(traceSource)) traceSource.TraceEvent(TraceEventType.Warning, 0, message);
        }

        /// <summary>Log a message of type Warning.</summary>
        /// <param name="traceSource">The trace source.</param>
        /// <param name="format">A composite format string.</param>
        /// <param name="arguments">An object array that contains zero or more objects to format.</param>
        public static void Warn(this TraceSource traceSource, [Localizable(false)] string format, params object?[] arguments)
        {
            if (IsWarnEnabled(traceSource)) traceSource.TraceEvent(TraceEventType.Warning, 0, format, arguments);
        }

        /// <summary>Log a message of type Error.</summary>
        /// <param name="traceSource">The trace source.</param>
        /// <param name="message">The log message.</param>
        public static void Error(this TraceSource traceSource, [Localizable(false)] string message)
        {
            if (IsErrorEnabled(traceSource)) traceSource.TraceEvent(TraceEventType.Error, 0, message);
        }

        /// <summary>Log a message of type Error.</summary>
        /// <param name="traceSource">The trace source.</param>
        /// <param name="format">A composite format string.</param>
        /// <param name="arguments">An object array that contains zero or more objects to format.</param>
        public static void Error(this TraceSource traceSource, [Localizable(false)] string format, params object?[] arguments)
        {
            if (IsErrorEnabled(traceSource)) traceSource.TraceEvent(TraceEventType.Error, 0, format, arguments);
        }
    }
}
