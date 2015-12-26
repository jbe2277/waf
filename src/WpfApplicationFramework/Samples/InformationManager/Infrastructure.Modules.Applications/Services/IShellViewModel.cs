using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Waf.InformationManager.Infrastructure.Interfaces.Applications;

namespace Waf.InformationManager.Infrastructure.Modules.Applications.Services
{
    public interface IShellViewModel
    {
        object View { get; }

        
        void AddToolBarCommands(IReadOnlyList<ToolBarCommand> commands);

        void ClearToolBarCommands();
    }
}
