using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Waf.BookLibrary.Library.Applications.Services;
using System.Waf.Foundation;

namespace Waf.BookLibrary.Library.Presentation.DesignData
{
    public class MockShellService : Model, IShellService
    {
        public object ShellView { get; set; }
        
        public object BookListView { get; set; }
        
        public object BookView { get; set; }
        
        public object PersonListView { get; set; }
        
        public object PersonView { get; set; }

        public bool IsReportingEnabled { get; set; }

        public Lazy<object> LazyReportingView { get; set; }
    }
}
