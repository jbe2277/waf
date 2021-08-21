namespace Waf.BookLibrary.Reporting.Presentation
{
    internal class BuildFix
    {
        public static void JustToCopyReferenceIntoOutput() => _ = typeof(Applications.Views.IReportView).GetHashCode();
    }
}
