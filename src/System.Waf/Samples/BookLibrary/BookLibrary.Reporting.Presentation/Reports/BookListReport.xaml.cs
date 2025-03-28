﻿using Waf.BookLibrary.Reporting.Applications.Reports;

namespace Waf.BookLibrary.Reporting.Presentation.Reports;

public partial class BookListReport : IBookListReport
{
    private object? reportData;

    public BookListReport()
    {
        InitializeComponent();
        // Disconnect the flowDocument from the UserControl. The UserControl is only used for DesignTime support.
        Content = null;
    }

    public object Report => flowDocument;

    public object? ReportData
    {
        get => reportData;
        set
        {
            reportData = value;
            flowDocument.DataContext = value;
        }
    }
}
