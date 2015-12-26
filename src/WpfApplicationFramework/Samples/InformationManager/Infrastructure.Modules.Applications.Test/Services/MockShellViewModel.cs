using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Waf.InformationManager.Infrastructure.Modules.Applications.Services;
using Waf.InformationManager.Infrastructure.Interfaces.Applications;

namespace Test.InformationManager.Infrastructure.Modules.Applications.Services
{
    public class MockShellViewModel : IShellViewModel
    {
        private readonly List<ToolBarCommand> toolBarCommands;
        
        
        public MockShellViewModel()
        {
            this.toolBarCommands = new List<ToolBarCommand>();
        }


        public IReadOnlyList<ToolBarCommand> ToolBarCommands { get { return toolBarCommands; } }
        
        public object View { get; set; }


        public void AddToolBarCommands(IReadOnlyList<ToolBarCommand> commands)
        {
            toolBarCommands.AddRange(commands);    
        }

        public void ClearToolBarCommands()
        {
            toolBarCommands.Clear();
        }
    }
}
