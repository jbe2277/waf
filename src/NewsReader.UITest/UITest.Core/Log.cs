using System.Diagnostics;
using Xunit;

namespace UITest;

public static class Log
{
    public static void WriteLine((string msg1, string? msg2) messageTuple) => WriteLine(messageTuple.msg1.PadRight(30) + messageTuple.msg2);

    public static void WriteLine(string message)
    {
        TestContext.Current.TestOutputHelper?.WriteLine(message);
        Trace.WriteLine(message);
    }

    public static void WriteLine(string format, params object[] args)
    {
        TestContext.Current.TestOutputHelper?.WriteLine(format, args);
        Trace.WriteLine(string.Format(format, args));
    }
}
