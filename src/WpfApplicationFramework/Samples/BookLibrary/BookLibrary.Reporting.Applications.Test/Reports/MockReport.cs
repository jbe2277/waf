using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Waf.BookLibrary.Reporting.Applications.Reports;

namespace Test.BookLibrary.Reporting.Applications.Reports
{
    public class MockReport : IReport
    {
        public object Report { get { return this; } }

        public object ReportData { get; set; }
    }
}
