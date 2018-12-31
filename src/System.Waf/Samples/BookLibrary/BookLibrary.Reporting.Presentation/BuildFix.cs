namespace Waf.BookLibrary.Reporting.Presentation
{
    internal class BuildFix
    {
        public static void JustToCopyReferenceIntoOutput()
        {
            typeof(Applications.Views.IReportView).GetHashCode();
        }
    }
}
