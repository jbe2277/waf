namespace Waf.BookLibrary.Reporting.Presentation;

internal sealed class BuildFix
{
    public static void JustToCopyReferenceIntoOutput() => _ = typeof(Applications.Views.IReportView).GetHashCode();
}
