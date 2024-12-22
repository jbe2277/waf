using FlaUI.Core.AutomationElements;
using FlaUI.Core;

namespace UITest.InformationManager.Views;

public class ExchangeSettingsView(FrameworkAutomationElementBase element) : AutomationElement(element)
{
    public TextBox ServerPathTextBox => this.Find("ServerPathTextBox").AsTextBox();

    public TextBox UserNameTextBox => this.Find("UserNameTextBox").AsTextBox();
}
