using System.ComponentModel;

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
