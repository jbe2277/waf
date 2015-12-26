using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Waf.Applications;

namespace Waf.InformationManager.AddressBook.Modules.Applications.Views
{
    public interface ISelectContactView : IView
    {
        void ShowDialog(object owner);

        void Close();
    }
}
