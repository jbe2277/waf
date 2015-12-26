using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Waf.Applications;

namespace Waf.BookLibrary.Library.Applications.Views
{
    public interface ILendToView : IView
    {
        void ShowDialog(object owner);

        void Close();
    }
}
