namespace Waf.BookLibrary.Reporting.Applications.Reports;

public interface IReport
{
    object Report { get; }

    object? ReportData { get; set; }
}
