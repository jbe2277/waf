using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;

namespace Waf.Writer.Applications.Services
{
    public interface IShellService : INotifyPropertyChanged
    {
        object ShellView { get; }

        string? DocumentName { get; set; }

        [AllowNull]
        IEditingCommands ActiveEditingCommands { get; set; }

        [AllowNull]
        IZoomCommands ActiveZoomCommands { get; set; }
    }
}
