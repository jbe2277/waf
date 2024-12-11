using FlaUI.Core;
using FlaUI.Core.AutomationElements;

namespace UITest.InformationManager.Views;

public class NewEmailWindow(FrameworkAutomationElementBase element) : Window(element)
{
    public Button SendButton => this.Find("SendButton").AsButton();

    public Button CloseButton => this.Find("CloseButton").AsButton();

    public ComboBox EmailAccountsComboBox => this.Find("EmailAccountsComboBox").AsComboBox();

    public TextBox ToTextBox => this.Find("ToTextBox").AsTextBox();

    public Button ToSelectContactButton => this.Find("ToSelectContactButton").AsButton();

    public TextBox CCTextBox => this.Find("CCTextBox").AsTextBox();

    public Button CCSelectContactButton => this.Find("CCSelectContactButton").AsButton();

    public TextBox BccTextBox => this.Find("BccTextBox").AsTextBox();

    public Button BccSelectContactButton => this.Find("BccSelectContactButton").AsButton();

    public TextBox TitleTextBox => this.Find("TitleTextBox").AsTextBox();

    public TextBox MessageTextBox => this.Find("MessageTextBox").AsTextBox();
}
