using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Input;

namespace Waf.Writer.Applications.Services
{
    public interface IShellService : INotifyPropertyChanged
    {
        object ShellView { get; }

        string DocumentName { get; set; }

        IEditingCommands ActiveEditingCommands { get; set; }

        IZoomCommands ActiveZoomCommands { get; set; }
    }
}
