using System.Waf.UnitTesting;

namespace Test.NewsReader.Applications;

// TODO: Consider to add this to WAF
public static class ContextExtensions
{
    public static T WaitForNotNull<T>(this UnitTestSynchronizationContext context, Func<T?> predicate) where T : class
            => WaitForNotNull(context, predicate, TimeSpan.FromSeconds(1));

    public static T WaitForNotNull<T>(this UnitTestSynchronizationContext context, Func<T?> predicate, TimeSpan timeout) where T : class
    {
        T? result = null;
        context.WaitFor(() =>
        {
            result = predicate();
            return result != null;
        }, timeout);
        return result!;
    }
}
