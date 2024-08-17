using FlaUI.Core;
using FlaUI.Core.AutomationElements;

namespace UITest.InformationManager.Views;

public class ShellWindow(FrameworkAutomationElementBase element) : Window(element)
{
    public Button AboutButton => this.Find("AboutButton").AsButton();

    public Button ExitButton => this.Find("ExitButton").AsButton();
}
