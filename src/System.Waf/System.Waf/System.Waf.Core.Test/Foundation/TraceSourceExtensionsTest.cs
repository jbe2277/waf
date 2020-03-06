using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Waf.Foundation;

namespace Test.Waf.Foundation
{
    [TestClass]
    public class TraceSourceExtensionsTest
    {
        [TestMethod]
        public void IsTraceEnabledTest() => IsEnabledTestCore(x => x.IsTraceEnabled(), SourceLevels.Verbose, SourceLevels.Information);

        [TestMethod]
        public void IsInfoEnabledTest() => IsEnabledTestCore(x => x.IsInfoEnabled(), SourceLevels.Information, SourceLevels.Warning);

        [TestMethod]
        public void IsWarnEnabledTest() => IsEnabledTestCore(x => x.IsWarnEnabled(), SourceLevels.Warning, SourceLevels.Error);

        [TestMethod]
        public void IsErrorEnabledTest() => IsEnabledTestCore(x => x.IsErrorEnabled(), SourceLevels.Error, SourceLevels.Critical);

        private void IsEnabledTestCore(Func<TraceSource, bool> isEnabled, SourceLevels supported, SourceLevels unsupported)
        {
            var log = new TraceSource("UnitTest");
            log.Switch.Level = supported;
            Assert.IsTrue(isEnabled(log));
            log.Switch.Level = unsupported;
            Assert.IsFalse(isEnabled(log));
        }

        [TestMethod]
        public void TraceTest() => LogTestCore((l, m) => l.Trace(m), (l, f, a) => l.Trace(f, a), SourceLevels.Verbose, SourceLevels.Information);

        [TestMethod]
        public void InfoTest() => LogTestCore((l, m) => l.Info(m), (l, f, a) => l.Info(f, a), SourceLevels.Information, SourceLevels.Warning);

        [TestMethod]
        public void WarnTest() => LogTestCore((l, m) => l.Warn(m), (l, f, a) => l.Warn(f, a), SourceLevels.Warning, SourceLevels.Error);

        [TestMethod]
        public void ErrorTest() => LogTestCore((l, m) => l.Error(m), (l, f, a) => l.Error(f, a), SourceLevels.Error, SourceLevels.Critical);

        private void LogTestCore(Action<TraceSource, string> logAction1, Action<TraceSource, string, object[]> logAction2, SourceLevels supported, SourceLevels unsupported)
        {
            var log = new TraceSource("UnitTest");
            var listener = new StubTraceListener();
            log.Listeners.Clear();
            log.Listeners.Add(listener);
            log.Switch.Level = supported;

            logAction1(log, "TstMsg");
            logAction2(log, "Tst{0}", new object[] { 42 });
            Assert.AreEqual(0, listener.Writes.Count);
            Assert.AreEqual(2, listener.TraceEvents.Count);
            var expectedEventType = supported switch
            {
                SourceLevels.Verbose => TraceEventType.Verbose,
                SourceLevels.Information => TraceEventType.Information,
                SourceLevels.Warning => TraceEventType.Warning,
                SourceLevels.Error => TraceEventType.Error,
                SourceLevels.Critical => TraceEventType.Critical,
                _ => TraceEventType.Stop
            };

            foreach (var message in listener.TraceEvents)
            {
                Assert.AreEqual("UnitTest", message.Source);
                
                Assert.AreEqual(expectedEventType, message.EventType);
                Assert.AreEqual(0, message.Id);
            }
            Assert.AreEqual("TstMsg", listener.TraceEvents[0].Format);
            Assert.IsNull(listener.TraceEvents[0].Arguments);
            Assert.AreEqual("Tst{0}", listener.TraceEvents[1].Format);
            Assert.AreEqual(42, listener.TraceEvents[1].Arguments.Single());

            log.Switch.Level = unsupported;
            logAction1(log, "NextMsg");
            logAction2(log, "Next{0}", new object[] { 42 });
            Assert.AreEqual(2, listener.TraceEvents.Count);
        }

        private class StubTraceListener : TraceListener
        {
            public IList<string> Writes { get; } = new List<string>();

            public IList<LogMessage> TraceEvents { get; } = new List<LogMessage>();

            public override void Write(string message) => Writes.Add(message);

            public override void WriteLine(string message) => Writes.Add(message);

            public override void TraceEvent(TraceEventCache eventCache, string source, TraceEventType eventType, int id)
            {
                TraceEvents.Add(new LogMessage(eventCache, source, eventType, id));
            }

            public override void TraceEvent(TraceEventCache eventCache, string source, TraceEventType eventType, int id, string message)
            {
                TraceEvents.Add(new LogMessage(eventCache, source, eventType, id, message));
            }

            public override void TraceEvent(TraceEventCache eventCache, string source, TraceEventType eventType, int id, string format, params object[] args)
            {
                TraceEvents.Add(new LogMessage(eventCache, source, eventType, id, format, args));
            }
        }

        private class LogMessage
        {
            public LogMessage(TraceEventCache eventCache, string source, TraceEventType eventType, int id, string? format = null, object[]? arguments = null)
            {
                EventCache = eventCache;
                Source = source;
                EventType = eventType;
                Id = id;
                Format = format;
                Arguments = arguments;
            }

            public TraceEventCache EventCache { get; }

            public string Source { get; }

            public TraceEventType EventType { get; }

            public int Id { get; }

            public string? Format { get; }

            public object[]? Arguments { get; }
        }
    }
}
