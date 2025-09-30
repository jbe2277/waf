using System.Windows.Input;

namespace Waf.NewsReader.Applications.DataModels;

public class NavigationItem(string title, string iconGlyph, string automationId) : Model
{
    private ICommand? command;
    private bool isCommandEnabled;

    public string Title { get => title; set => SetProperty(ref title, value); }

    public string IconGlyph { get => iconGlyph; set => SetProperty(ref iconGlyph, value); }

    public string AutomationId => automationId;

    public ICommand? Command
    {
        get => command;
        set
        {
            var oldCommand = command;
            if (SetProperty(ref command, value))
            {
                if (oldCommand != null) oldCommand.CanExecuteChanged -= CommandCanExecuteChanged;
                if (command != null) command.CanExecuteChanged += CommandCanExecuteChanged;
                IsCommandEnabled = command?.CanExecute(null) ?? false;
            }
        }
    }

    public bool IsCommandEnabled { get => isCommandEnabled; set => SetProperty(ref isCommandEnabled, value); }

    private void CommandCanExecuteChanged(object? sender, EventArgs e) => IsCommandEnabled = command?.CanExecute(null) ?? false;
}
