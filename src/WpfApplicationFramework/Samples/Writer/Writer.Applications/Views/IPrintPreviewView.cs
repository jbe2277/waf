using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Waf.Applications;

namespace Waf.Writer.Applications.Views
{
    public interface IPrintPreviewView : IView
    {
        void FitToWidth();
    }
}
