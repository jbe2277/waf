using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Waf.Applications;
using Waf.InformationManager.Infrastructure.Interfaces.Applications;

namespace Waf.InformationManager.Infrastructure.Modules.Applications.Views
{
    public interface IShellView : IView
    {
        double VirtualScreenWidth { get; }

        double VirtualScreenHeight { get; }
        
        double Left { get; set; }

        double Top { get; set; }

        double Width { get; set; }

        double Height { get; set; }

        bool IsMaximized { get; set; }


        event EventHandler Closed;

        
        void Show();

        void Close();

        void AddToolBarCommands(IReadOnlyList<ToolBarCommand> commands);

        void ClearToolBarCommands();
    }
}
