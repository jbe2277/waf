using FlaUI.Core.AutomationElements;
using FlaUI.Core;

namespace UITest.InformationManager.Views;

public class BasicEmailAccountView(FrameworkAutomationElementBase element) : AutomationElement(element)
{
    public TextBox NameTextBox => this.Find("NameTextBox").AsTextBox();

    public TextBox EmailTextBox => this.Find("EmailTextBox").AsTextBox();

    public RadioButton Pop3RadioButton => this.Find("Pop3RadioButton").AsRadioButton();

    public RadioButton ExchangeRadioButton => this.Find("ExchangeRadioButton").AsRadioButton();
}
