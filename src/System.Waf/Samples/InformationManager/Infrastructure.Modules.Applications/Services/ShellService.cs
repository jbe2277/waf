using System.ComponentModel.Composition;
using System.Waf.Foundation;
using System.Windows;
using System.Windows.Input;
using Waf.InformationManager.Infrastructure.Interfaces.Applications;

namespace Waf.InformationManager.Infrastructure.Modules.Applications.Services;

[Export(typeof(IShellService)), Export]
public class ShellService : Model, IShellService
{
    private readonly Lazy<IShellViewModel> shellViewModel;
    private object? contentView;

    [ImportingConstructor]
    public ShellService(Lazy<IShellViewModel> shellViewModel)
    {
        this.shellViewModel = shellViewModel;
    }

    public object ShellView => shellViewModel.Value.View;

    public object? ContentView
    {
        get => contentView;
        set => SetProperty(ref contentView, value);
    }

    public void AddToolBarCommands(IReadOnlyList<ToolBarCommand> commands) => shellViewModel.Value.AddToolBarCommands(commands);

    public void ClearToolBarCommands() => shellViewModel.Value.ClearToolBarCommands();

    public void CommitUIChanges()
    {
        var element = Keyboard.FocusedElement as FrameworkElement;
        element?.MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
        element?.Focus();
    }
}
