using System.Windows.Input;

namespace Waf.InformationManager.Infrastructure.Interfaces.Applications;

/// <summary>Defines a tool bar command</summary>
/// <param name="Command">The command which is invoked when the user clicks on the tool bar button.</param>
/// <param name="Text">The text of the tool bar button.</param>
/// <param name="ToolTip">The tooltip of the tool bar button.</param>
public record ToolBarCommand(ICommand Command, string Text, string? ToolTip = null);
