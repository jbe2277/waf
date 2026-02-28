using System.Waf.Applications;
using System.Windows.Input;

namespace Waf.Writer.Applications.Services;

internal class ShellService : Model, IShellService
{
    public ShellService()
    {
        ActiveEditingCommands = new DisabledEditingCommands();
        ActiveZoomCommands = new DisabledZoomCommands();
    }

    public object ShellView { get; set; } = null!;

    public string? DocumentName { get; set => SetProperty(ref field, value); }

    [AllowNull] public IEditingCommands ActiveEditingCommands { get; set => SetProperty(ref field, value ?? new DisabledEditingCommands()); }

    [AllowNull]
    public IZoomCommands ActiveZoomCommands { get; set => SetProperty(ref field, value ?? new DisabledZoomCommands()); }


    private sealed class DisabledEditingCommands : Model, IEditingCommands
    {
        public bool IsBold { get; set; }

        public bool IsItalic { get; set; }

        public bool IsUnderline { get; set; }

        public bool IsNumberedList { get; set; }

        public bool IsBulletList { get; set; }

        public bool IsSpellCheckAvailable => false;

        public bool IsSpellCheckEnabled { get; set; }
    }

    private sealed class DisabledZoomCommands : Model, IZoomCommands
    {
        public IReadOnlyList<string> DefaultZooms => [];

        public double Zoom { get => 1; set { } }

        public ICommand ZoomInCommand => DelegateCommand.DisabledCommand;

        public ICommand ZoomOutCommand => DelegateCommand.DisabledCommand;

        public ICommand FitToWidthCommand => DelegateCommand.DisabledCommand;
    }
}
