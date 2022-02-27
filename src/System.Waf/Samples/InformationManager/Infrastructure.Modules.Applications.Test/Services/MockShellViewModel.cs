using Waf.InformationManager.Infrastructure.Modules.Applications.Services;
using Waf.InformationManager.Infrastructure.Interfaces.Applications;

namespace Test.InformationManager.Infrastructure.Modules.Applications.Services;

public class MockShellViewModel : IShellViewModel
{
    private readonly List<ToolBarCommand> toolBarCommands;

    public MockShellViewModel()
    {
        toolBarCommands = new List<ToolBarCommand>();
    }

    public IReadOnlyList<ToolBarCommand> ToolBarCommands => toolBarCommands;

    public object View { get; set; } = null!;

    public void AddToolBarCommands(IReadOnlyList<ToolBarCommand> commands) => toolBarCommands.AddRange(commands);

    public void ClearToolBarCommands() => toolBarCommands.Clear();
}
