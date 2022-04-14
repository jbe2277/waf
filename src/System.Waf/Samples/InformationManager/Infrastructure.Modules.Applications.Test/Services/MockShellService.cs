using System.ComponentModel.Composition;
using System.Waf.Foundation;
using Waf.InformationManager.Infrastructure.Interfaces.Applications;

namespace Test.InformationManager.Infrastructure.Modules.Applications.Services;

[Export(typeof(IShellService)), Export]
public class MockShellService : Model, IShellService
{
    private readonly List<ToolBarCommand> toolBarCommands;

    public MockShellService()
    {
        toolBarCommands = new List<ToolBarCommand>();
    }

    public IReadOnlyList<ToolBarCommand> ToolBarCommands => toolBarCommands;

    public object ShellView { get; set; } = null!;

    public object? ContentView { get; set; }

    public void AddToolBarCommands(IReadOnlyList<ToolBarCommand> commands) => toolBarCommands.AddRange(commands);

    public void ClearToolBarCommands() => toolBarCommands.Clear();

    public void CommitUIChanges() { }
}
