using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Waf.Applications;

namespace Waf.BookLibrary.Library.Applications.Views
{
    public interface IPersonListView : IView
    {
        void FocusFirstCell();
    }
}
