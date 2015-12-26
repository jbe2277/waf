using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Waf.Applications;

namespace Waf.InformationManager.EmailClient.Modules.Applications.Views
{
    public interface INewEmailView : IView
    {
        void Show(object owner);

        void Close();
    }
}
