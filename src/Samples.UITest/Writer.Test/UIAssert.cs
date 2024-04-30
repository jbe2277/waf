using FlaUI.Core.Tools;
using System.Runtime.CompilerServices;

namespace UITest.Writer;

public class ElementFoundException(string message, Exception? innerException = null) : Exception(message, innerException) { }

public static class UIAssert
{
    public static void NotExists(Action accessElement, [CallerArgumentExpression(nameof(accessElement))]string? argumentExpression = null)
    {
        var notFound = false;
        var timeout = Retry.DefaultTimeout;
        Retry.DefaultTimeout = TimeSpan.Zero;
        try
        {
            accessElement();
        }
        catch (ElementNotFoundException)
        {
            notFound = true;
        }
        finally
        {
            Retry.DefaultTimeout = timeout;
        }
        if (!notFound) throw new ElementFoundException($"Element found with '{argumentExpression}'");
    }
}
