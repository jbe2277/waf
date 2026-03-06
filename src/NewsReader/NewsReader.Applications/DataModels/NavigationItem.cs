using System.Windows.Input;

namespace Waf.NewsReader.Applications.DataModels;

public class NavigationItem(string title, string iconGlyph, string automationId) : Model
{
    public string Title { get => title; set => SetProperty(ref title, value); }

    public string IconGlyph { get => iconGlyph; set => SetProperty(ref iconGlyph, value); }

    public string AutomationId => automationId;

    public ICommand? Command
    {
        get;
        set
        {
            var oldCommand = field;
            if (SetProperty(ref field, value))
            {
                oldCommand?.CanExecuteChanged -= CommandCanExecuteChanged;
                field?.CanExecuteChanged += CommandCanExecuteChanged;
                IsCommandEnabled = field?.CanExecute(null) ?? false;
            }
        }
    }

    public bool IsCommandEnabled { get; set => SetProperty(ref field, value); }

    private void CommandCanExecuteChanged(object? sender, EventArgs e) => IsCommandEnabled = Command?.CanExecute(null) ?? false;
}
