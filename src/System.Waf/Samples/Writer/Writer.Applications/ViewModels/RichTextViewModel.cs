﻿using Waf.Writer.Applications.Documents;
using Waf.Writer.Applications.Services;
using Waf.Writer.Applications.Views;

namespace Waf.Writer.Applications.ViewModels;

public class RichTextViewModel(IRichTextView view, IShellService shellService) : ZoomViewModel<IRichTextView>(view, shellService), IEditingCommands
{
    private bool isBold;
    private bool isItalic;
    private bool isUnderline;
    private bool isNumberedList;
    private bool isBulletList;
    private bool isSpellCheckEnabled;

    public IRichTextDocument Document { get; set; } = null!;

    public bool IsBold
    {
        get => isBold;
        set => SetProperty(ref isBold, value);
    }

    public bool IsItalic
    {
        get => isItalic;
        set => SetProperty(ref isItalic, value);
    }

    public bool IsUnderline
    {
        get => isUnderline;
        set => SetProperty(ref isUnderline, value);
    }

    public bool IsNumberedList
    {
        get => isNumberedList;
        set => SetProperty(ref isNumberedList, value);
    }

    public bool IsBulletList
    {
        get => isBulletList;
        set => SetProperty(ref isBulletList, value);
    }

    public bool IsSpellCheckAvailable => true;

    public bool IsSpellCheckEnabled
    {
        get => isSpellCheckEnabled;
        set => SetProperty(ref isSpellCheckEnabled, value);
    }

    protected override void OnPropertyChanged(PropertyChangedEventArgs e)
    {
        base.OnPropertyChanged(e);
        if (e.PropertyName == nameof(IsVisible)) ShellService.ActiveEditingCommands = IsVisible ? this : null;
    }
}
