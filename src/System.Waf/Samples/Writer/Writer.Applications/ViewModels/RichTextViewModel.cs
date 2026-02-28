using Waf.Writer.Applications.Documents;
using Waf.Writer.Applications.Services;
using Waf.Writer.Applications.Views;

namespace Waf.Writer.Applications.ViewModels;

public class RichTextViewModel(IRichTextView view, IShellService shellService) : ZoomViewModel<IRichTextView>(view, shellService), IEditingCommands
{
    public IRichTextDocument Document { get; set; } = null!;

    public bool IsBold { get; set => SetProperty(ref field, value); }

    public bool IsItalic { get; set => SetProperty(ref field, value); }

    public bool IsUnderline { get; set => SetProperty(ref field, value); }

    public bool IsNumberedList { get; set => SetProperty(ref field, value); }

    public bool IsBulletList { get; set => SetProperty(ref field, value); }

    public bool IsSpellCheckAvailable => true;

    public bool IsSpellCheckEnabled { get; set => SetProperty(ref field, value); }

    protected override void OnPropertyChanged(PropertyChangedEventArgs e)
    {
        base.OnPropertyChanged(e);
        if (e.PropertyName == nameof(IsVisible)) ShellService.ActiveEditingCommands = IsVisible ? this : null;
    }
}
